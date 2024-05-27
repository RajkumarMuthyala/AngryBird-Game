using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
   public int MaxNumberOfShots = 3;
   [SerializeField] private float _secondsToWaitBeforeDeathCheck = 3f;
   [SerializeField] private GameObject _restartScreenObject;
   [SerializeField] private SlingShotHandler _slingShotHandler;
   [SerializeField] private Image _nextlevel;
   private int _useNumberOfShots;
   private IconHandler _iconHandler;
   private List<Baddie> _baddies = new List<Baddie>();
   private void Awake()
   {
    if(instance == null)
    {
        instance = this;
    }
    _iconHandler = FindAnyObjectByType<IconHandler>();
    Baddie[] baddies = FindObjectsOfType<Baddie>();
    for(int i=0; i < baddies.Length; i++ )
    {
        _baddies.Add(baddies[i]);
    }
     _nextlevel.enabled = false;
   }
   public void UsedShots()
   {
    _useNumberOfShots++;
    _iconHandler.UsedShot(_useNumberOfShots);
    CheckForLastShot();
   }
   public bool HasEnoughShots()
   {
     if(_useNumberOfShots < MaxNumberOfShots)
     {
        return true;
     }
     else{
        return false;
     }
   }
   public void CheckForLastShot()
   {
    if(_useNumberOfShots == MaxNumberOfShots)
    {
        StartCoroutine(CheckAfterWaitTime());
    }
   }
   private IEnumerator CheckAfterWaitTime()
   {
        yield return new WaitForSeconds(_secondsToWaitBeforeDeathCheck);

        if(_baddies.Count == 0)
        {
            WinGame();
        }
        else
        {
            RestartGame();
        }
   }
   public void RemoveBaddie(Baddie baddie)
   {
    _baddies.Remove(baddie);
    CheckForAllDeadBaddies();
   }
   private void CheckForAllDeadBaddies()
   {
    if(_baddies.Count == 0)
    {
        WinGame();
    }
   }
   #region win/loss
    private void WinGame()
    {
        _restartScreenObject.SetActive(true);
        _slingShotHandler.enabled = false;

        int currentSceneIndex =SceneManager.GetActiveScene().buildIndex;
        int maxLevels = SceneManager.sceneCountInBuildSettings;
        if(currentSceneIndex  +1 < maxLevels)
        {
            _nextlevel.enabled = true;
        } 
    }

    public void RestartGame()
    {
        DOTween.Clear(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
   #endregion
}
