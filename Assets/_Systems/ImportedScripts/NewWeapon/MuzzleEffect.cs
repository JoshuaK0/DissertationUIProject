using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleEffect : MonoBehaviour
{
    [SerializeField] List<ParticleSystem> effects = new List<ParticleSystem>();
    [SerializeField] float lifeTime;

    float currentLife;

    void Update()
    {
        if(currentLife <= 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            currentLife -= Time.deltaTime;
        }
    }

    public void PlayEffect()
    {
        currentLife = lifeTime;
        foreach(ParticleSystem p in effects)
        {
            p.Play();
        }
    }
}
