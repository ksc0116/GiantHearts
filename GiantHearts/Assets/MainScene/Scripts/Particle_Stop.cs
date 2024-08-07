using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Particle_Stop : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem newparticleSystem;
    void Start()
    {
        if (newparticleSystem == null)
        {
            Debug.Log("null");
        }
    }

    void OnEnable()
    {
        newparticleSystem.Play();
    }

    void Update()
    {
        if(newparticleSystem.isStopped)
        {
            Manager.Instance.objectPool.GiveBackObject(gameObject);
            if(Manager.Instance== null)
            {
                Debug.Log("manager null");
            }

            if (Manager.Instance.objectPool == null)
            {
                Debug.Log("objectPool null");
            }
        }
    }
}
