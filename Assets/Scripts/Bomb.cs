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
        if (collider.gameObject.GetComponent<BreakablePlatform>() != null)
        {
            collider.gameObject.GetComponent<BreakablePlatform>().ActivateRocks();
            foreach (Rock rock in collider.gameObject.GetComponent<BreakablePlatform>().rocks)
            {
                rock.rb.AddForce((rock.gameObject.transform.position - transform.position).normalized * explosionForce, ForceMode.Impulse);
            }
        }
        Explode();
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, explosionLayers);

        //Debug.Log(colliders.Length);

        if (colliders.Length > 0)
        {
            foreach (Collider collider in colliders)
            {
                DungeonPlayerController bp = collider.GetComponent<DungeonPlayerController>();
                if (bp != null)
                {
                    //Debug.Log("adding force");
                    bp.SetLaunched();

                    Rigidbody bprb = bp.gameObject.GetComponent<Rigidbody>();
                    if (bprb.velocity.y < 0)
                    {
                        bprb.velocity = new Vector3(bprb.velocity.x, 0, bprb.velocity.z);
                    }
                    bprb.AddForce((bp.gameObject.transform.position - transform.position).normalized * explosionForce, ForceMode.Impulse);
                    continue;
                }

                BreakableWall bw = collider.GetComponent<BreakableWall>();
                if (bw != null)
                {
                    //Debug.Log("attempt rock break");
                    bw.ActivateRocks();
                    foreach (Rock rock in bw.rocks)
                    {
                        rock.rb.AddForce((rock.gameObject.transform.position - transform.position).normalized * explosionForce, ForceMode.Impulse);
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
