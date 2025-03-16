using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Rock : MonoBehaviour
{
    public Rigidbody rb;
    bool shrink = false;

    Vector3 initialPosition;
    Vector3 initialRotation;
    Vector3 initialScale;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        initialPosition = transform.position;
        initialRotation = transform.rotation.eulerAngles;
        initialScale = transform.localScale;
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
                Reset();
            }
        }
    }

    public void Reset()
    {
        shrink = false;
        rb.isKinematic = true;
        transform.position = initialPosition;
        transform.rotation = Quaternion.Euler(initialRotation);
        transform.localScale = initialScale;

        gameObject.SetActive(false);
    }
}
