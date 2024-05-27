using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AngryBird : MonoBehaviour
{
    [SerializeField] AudioClip __hitClip;
    private Rigidbody2D _rb;
    private CircleCollider2D _circleCollider;
    private bool _hasBeenLaunched;
    private bool _shouldFaceVelocityDirection;
    private AudioSource _audioSource;
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _rb = GetComponent<Rigidbody2D>();
        _circleCollider = GetComponent<CircleCollider2D>();
    }
    private void Start()
    {
        _rb.isKinematic = true;
        _circleCollider.enabled = false;
    }

    private void FixedUpdate()
    {
        if(_hasBeenLaunched && _shouldFaceVelocityDirection)
        {
        transform.right = _rb.velocity;
        }
    }
    public void LaunchBird(Vector2 direction, float force)
    {
        _rb.isKinematic = false;
        _circleCollider.enabled = true;

        //apply force
        _rb.AddForce(direction*force, ForceMode2D.Impulse);
        _hasBeenLaunched = true;
        _shouldFaceVelocityDirection = true;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        _shouldFaceVelocityDirection = false;
        SoundManager.instance.PlayClip(__hitClip, _audioSource);
        Destroy(this);
    }
}
