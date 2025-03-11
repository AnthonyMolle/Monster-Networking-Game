using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;

public class BombPlayer : DungeonPlayerController
{
    [SerializeField] GameObject bombPrefab;

    bool canThrow = true;
    bool holdingThrow = false;
    bool doThrow = false;

    [SerializeField] float throwForce = 10f;
    [SerializeField] Animator bombAnimator;

    protected override void Update()
    {
        base.Update();

        if (actionPrimaryPressed)
        {

            if (canThrow && !holdingThrow)
            {
                holdingThrow = true;
                doThrow = true;
                canThrow = false;

                bombAnimator.Play("Throw");
            }
        }
        else
        {
            if (holdingThrow)
            {
                holdingThrow = false;
            }
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (doThrow)
        {
            ThrowBomb();

            doThrow = false;
        }
    }

    private void ThrowBomb()
    {
        if (((GetCameraForwardVector().normalized * throwForce) + rb.velocity).magnitude < (GetCameraForwardVector().normalized * throwForce).magnitude)
        {
            Instantiate(bombPrefab, GetCameraPosition(), GetHeldObjectTransform().rotation).GetComponent<Rigidbody>().AddForce(GetCameraForwardVector().normalized * throwForce, ForceMode.Impulse);
        }
        else
        {
            Instantiate(bombPrefab, GetCameraPosition(), GetHeldObjectTransform().rotation).GetComponent<Rigidbody>().AddForce((GetCameraForwardVector().normalized * throwForce) + rb.velocity, ForceMode.Impulse);
        }
    }

    public void ResetThrow()
    {
        canThrow = true;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }
}
