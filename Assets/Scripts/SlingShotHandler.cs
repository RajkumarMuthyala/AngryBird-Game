

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class SlingShotHandler : MonoBehaviour
{
  [Header("Line Renderers")]
  [SerializeField] private LineRenderer _LeftLineRenderer;
  [SerializeField] private LineRenderer _RightLineRenderer;
  [Header("Transform References")]
  [SerializeField] private Transform _leftStartPosition;
  [SerializeField] private Transform _RightStartPosition;
  [SerializeField] private Transform _centerPosition;
  [SerializeField] private Transform _idlePostion;
  [SerializeField] private Transform _elasticTransfrom;
  [Header("Slingshot stats")]
  [SerializeField] private float _maxDistance = 3.5f;
  [SerializeField] private float _shotForce = 5f;
  [SerializeField] private float _timeBetweenBirdRespawns = 2f;
  [SerializeField] private float _elasticDivider = 1.2f;
  [SerializeField] private AnimationCurve _elasticCurve;
  [SerializeField] private float _maxAnimationtime =1f;
  [Header("Scripts")]
  [SerializeField] private SligShotArea _slingshotArea;
  [SerializeField] private CameraManager _cameraManager;
  [Header("Bird")]
  [SerializeField] private AngryBird _angryBirdPrefab;
  [SerializeField] private float _angryBirdPositionOffset = 2f;
  [Header("Sounds")]
  [SerializeField] private AudioClip _elasticPulledClip;
  [SerializeField] private AudioClip[] _elasticReleasedClips;

  private Vector2 _slingShotLinePosition;
  private Vector2 _direction;
  private Vector2 _directionNormalized;

  private bool _clickedWithinArea;
  private bool _birdOnSlingshot;
  private AngryBird _spanwedAngryBird;
  private AudioSource _audioSource;
  
  private void Awake()
  {
    _audioSource = GetComponent<AudioSource>();
    _LeftLineRenderer.enabled = false;
    _RightLineRenderer.enabled = false;
    SpawnAngryBird();
  }
    private void Update()
    {
      if(InputManager.WasLeftMouseButtonPressed && _slingshotArea.IsWithInSlingshotArea())
      {
          _clickedWithinArea = true;
          if(_birdOnSlingshot)
          {
            SoundManager.instance.PlayClip(_elasticPulledClip, _audioSource);
            _cameraManager.SwitchToFollowCam(_spanwedAngryBird.transform);
          }
      }
        if(InputManager.IsLeftMousePressed && _clickedWithinArea && _birdOnSlingshot)
        {
          DrawSlingShot();
          PositionAndRotateAngryBird();
        }
        if(InputManager.WasLeftMouseButtonReleased && _birdOnSlingshot && _clickedWithinArea)
        {
          if(GameManager.instance.HasEnoughShots())
          {
          _clickedWithinArea = false;
          _birdOnSlingshot = false;

          _spanwedAngryBird.LaunchBird(_direction, _shotForce);
          SoundManager.instance.PlayRandomCliip(_elasticReleasedClips, _audioSource);
          GameManager.instance.UsedShots();
          AnimateSlingShot();
          if(GameManager.instance.HasEnoughShots())
          {
          StartCoroutine(SpawnAngryBirdAfterTime());

          }

          }

        }
    }
    #region Slingshot methods
    private void DrawSlingShot()
    {
      
      Vector3 touchPosition = Camera.main.ScreenToWorldPoint(InputManager.MousePosition);
      _slingShotLinePosition = _centerPosition.position + Vector3.ClampMagnitude(touchPosition - _centerPosition.position, _maxDistance);
      SetLines(_slingShotLinePosition);

      _direction = (Vector2)_centerPosition.position - _slingShotLinePosition;
      _directionNormalized = _direction.normalized;
    }
    private void SetLines(Vector2 position)
    {
      if(!_LeftLineRenderer.enabled && !_RightLineRenderer.enabled)
      {
        _LeftLineRenderer.enabled = true;
        _RightLineRenderer.enabled = true;
      }
      _LeftLineRenderer.SetPosition(0, position);
      _LeftLineRenderer.SetPosition(1, _leftStartPosition.position);

      _RightLineRenderer.SetPosition(0, position);
      _RightLineRenderer.SetPosition(1, _RightStartPosition.position);
    }
    #endregion
   
    #region Angry Bird Methods

    private void SpawnAngryBird()
    {
      _elasticTransfrom.DOComplete();
      SetLines(_idlePostion.position);
      
      Vector2 dir = (_centerPosition.position - _idlePostion.position).normalized;
      Vector2 spawnPosition = (Vector2)_idlePostion.position + dir * _angryBirdPositionOffset;
      
      _spanwedAngryBird = Instantiate(_angryBirdPrefab, spawnPosition, Quaternion.identity);
      _spanwedAngryBird.transform.right = dir;
      _birdOnSlingshot = true;
    }

    private void PositionAndRotateAngryBird()
    {
      _spanwedAngryBird.transform.position = _slingShotLinePosition + _directionNormalized * _angryBirdPositionOffset;
      _spanwedAngryBird.transform.right = _directionNormalized;
    }
    private IEnumerator SpawnAngryBirdAfterTime()
    {
      yield return new WaitForSeconds(_timeBetweenBirdRespawns);
      SpawnAngryBird();

      _cameraManager.SwitchToIdleCam();
    }

    #endregion

    #region Animate SlingShot

    private void AnimateSlingShot()
    {
        _elasticTransfrom.position = _LeftLineRenderer.GetPosition(0);

        float dist = Vector2.Distance(_elasticTransfrom.position, _centerPosition.position);

        float time = dist/_elasticDivider;

        _elasticTransfrom.DOMove(_centerPosition.position, time).SetEase(_elasticCurve);
        StartCoroutine(AnimateSlingShotLines(_elasticTransfrom, time));

    }

    private IEnumerator AnimateSlingShotLines(Transform trans, float time)
    {
      float elapseTime = 0f;
      while(elapseTime < time && elapseTime < _maxAnimationtime)
      {
        elapseTime +=Time.deltaTime;

        SetLines(trans.position);

        yield return null;
      }
    }

#endregion
}

