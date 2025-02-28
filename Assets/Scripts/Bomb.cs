using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;

public class Bomb : MonoBehaviour
{
    [SerializeField] GameObject particles;
    [SerializeField] float explosionRadius = 5f;
    [SerializeField] LayerMask explosionLayers;
    [SerializeField] float explosionForce = 5f;

    private void OnTriggerEnter(Collider collider)
    {
        Explode();
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, explosionLayers);

        Debug.Log(colliders.Length);

        if (colliders.Length > 0)
        {
            foreach (Collider collider in colliders)
            {
                DungeonPlayerController bp = collider.GetComponent<DungeonPlayerController>();
                if (bp != null)
                {
                    Debug.Log("adding force");
                    bp.SetLaunched();
                    bp.gameObject.GetComponent<Rigidbody>().AddForce((bp.gameObject.transform.position - transform.position).normalized * explosionForce, ForceMode.Impulse);
                    continue;
                }

                BreakableWall bw = collider.GetComponent<BreakableWall>();
                if (bw != null)
                {
                    bw.ActivateRocks();
                    foreach (Rigidbody rb in bw.rbs)
                    {
                        rb.AddForce((rb.gameObject.transform.position - transform.position).normalized * explosionForce, ForceMode.Impulse);
                    }
                    continue;
                }
            }
        }

        if (particles != null)
        {
            Instantiate(particles, transform.position, transform.rotation);
        }

        Destroy(gameObject);
    }
}
