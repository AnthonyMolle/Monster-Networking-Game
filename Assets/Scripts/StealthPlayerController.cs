using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthPlayerController : DungeonPlayerController
{
    [SerializeField] Animator daggerAnimator;

    private bool inEnemyRange;
    private List<GhostEnemy> ghostsInRange = new List<GhostEnemy>();
    private GhostEnemy targetedEnemy;

    [SerializeField] LayerMask enemyLayers;

    public bool daggerRaised = false;
    public bool daggerLowered = false;
    public bool daggerRecovering = false;

    public LockedDoor activeDoor;
    public bool hasKey = false;

    protected override void Update()
    {
        base.Update();

        if (actionPrimaryPressed)
        {
            if (inEnemyRange && targetedEnemy != null && ghostsInRange.Contains(targetedEnemy) && !daggerRecovering)
            {
                daggerAnimator.Play("DaggerStrike");
                daggerRecovering = true;
                daggerLowered = true;
                daggerRaised = false;
                ghostsInRange.Remove(targetedEnemy);
                if (ghostsInRange.Count <= 0)
                {
                    inEnemyRange = false;
                }
                targetedEnemy.Die();
                targetedEnemy = null;
            }
        }

        if (canInteract && interactPressed)
        {
            if (activeDoor != null && hasKey)
            {
                activeDoor.OpenDoor();
                hasKey = false;
                activeDoor = null;
            }
        }

        if (inEnemyRange)
        {
            RaycastHit hit;
            if (Physics.Raycast(GetCameraPosition(), GetCameraForwardVector(), out hit, 3f, enemyLayers))
            {
                GhostEnemy ghost = hit.collider.gameObject.GetComponentInParent<GhostEnemy>();

                if (ghost != null)
                {
                    targetedEnemy = ghost;
                    if (!daggerRaised && !daggerRecovering)
                    {
                        daggerAnimator.Play("DaggerRaise");
                        daggerRaised = true;
                        daggerLowered = false;
                    }
                }
            }
            else
            {
                if (!daggerLowered && !daggerRecovering)
                {
                    daggerAnimator.Play("DaggerLower");
                    daggerLowered = true;
                    daggerRaised = false;
                }
                targetedEnemy = null;
            }
        }
        else
        {
            if (!daggerLowered && !daggerRecovering)
            {
                daggerAnimator.Play("DaggerLower");
                daggerLowered = true;
                daggerRaised = false;
            }

            if (targetedEnemy != null)
            {
                targetedEnemy = null;
            }
        }
    }

    public void AddInRangeEnemy(GhostEnemy enemy)
    {
        ghostsInRange.Add(enemy);
        
        if (!inEnemyRange)
        {
            inEnemyRange = true;
        }
    }

    public void RemoveInRangeEnemy(GhostEnemy enemy)
    {
        ghostsInRange.Remove(enemy);

        if (ghostsInRange.Count <= 0)
        {
            inEnemyRange = false;
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
