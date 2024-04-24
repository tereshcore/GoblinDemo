using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : MonoBehaviour, IDamageable
{
    [SerializeField] private ParticleSystem _particleSystem;
    public void TakeDamage(int damage)
    {
        GetComponent<Animation>().Play();
        _particleSystem.Play();
    }
    public void Dead()
    {
        //Destroy(gameObject);
    }
}
