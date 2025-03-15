using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GhostEnemy : MonoBehaviour
{
    [SerializeField] GameObject deathParticlesPrefab;
    [SerializeField] Transform particlePoint;
    [SerializeField] GameObject keyModel;
    [SerializeField] GameObject keyPickupPrefab;

    [SerializeField] bool hasKey;

    private void Start()
    {
        if (hasKey)
        {
            keyModel.SetActive(true);
        }
        else
        {
            keyModel.SetActive(false);
        }
    }

    public void Die()
    {
        if (hasKey)
        {
            Instantiate(keyPickupPrefab, particlePoint.position, particlePoint.rotation);
        }
        //Instantiate(deathParticlesPrefab, particlePoint);
        gameObject.SetActive(false);
    }

    public void Reset()
    {
        //transform.localPosition = Vector3.zero;
    }
}
