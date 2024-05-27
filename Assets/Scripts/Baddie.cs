using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Baddie : MonoBehaviour
{
    [SerializeField] private float _maxHealth = 3f;
    [SerializeField] private float _damageThreshold = 0.2f;
    [SerializeField] private GameObject _baddieDeathParticle;
    [SerializeField] private AudioClip _deathClip;
    private float _currentHealth;
    
    public void Awake()
    {
        _currentHealth = _maxHealth;
    } 
    public void Damagebaddie(float damageAmount)
    {
        _currentHealth -= damageAmount;

        if(_currentHealth <= 0f)
        {
            Die();
        }
    }
    public void Die()
    {
        GameManager.instance.RemoveBaddie(this);
        Instantiate(_baddieDeathParticle, transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(_deathClip, transform.position);
        Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        float impactVelocity = collision.relativeVelocity.magnitude;

        if(impactVelocity > _damageThreshold)
        {
            Damagebaddie(impactVelocity);
        }
    }
}
