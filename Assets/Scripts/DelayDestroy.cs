using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DelayDestroyCoroutine());
    }

    private IEnumerator DelayDestroyCoroutine()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
