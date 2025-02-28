using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BombPlayer : DungeonPlayerController
{
    [SerializeField] GameObject bombPrefab;

    bool canThrow = true;
    bool holdingThrow = false;
    bool doThrow = false;

    [SerializeField] float throwForce = 10f;

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
            }
        }
        else
        {
            if (holdingThrow)
            {
                holdingThrow = false;
                //temp
                canThrow = true;
                //temp
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
        Instantiate(bombPrefab, GetCameraPosition(), Quaternion.identity).GetComponent<Rigidbody>().AddForce(GetCameraForwardVector().normalized * throwForce, ForceMode.Impulse);
    }

    public void resetThrow()
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
