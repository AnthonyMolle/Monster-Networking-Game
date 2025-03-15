using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Rock : MonoBehaviour
{
    public Rigidbody rb;
    bool shrink = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }    

    public void ActivateRock(float rockShrinkDelay)
    {
        rb.isKinematic = false;
        StartCoroutine(RockShrinkDelay(rockShrinkDelay));
    }

    private IEnumerator RockShrinkDelay(float rockShrinkDelay)
    {
        yield return new WaitForSeconds(rockShrinkDelay);
        shrink = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (shrink)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, 0.75f * Time.deltaTime);
            if (transform.localScale.magnitude < 0.1f)
            {
                Destroy(gameObject);
            }
        }
    }
}
