using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class GhostEnemy : MonoBehaviour
{
    [SerializeField] GameObject deathParticlesPrefab;
    [SerializeField] Transform particlePoint;
    [SerializeField] Transform keyPoint;
    [SerializeField] GameObject keyModel;
    [SerializeField] GameObject keyPickupPrefab;

    [SerializeField] bool hasKey;
    [SerializeField] bool movePatrol;
    [SerializeField] bool rotatePatrol;

    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float rotationSpeed = 0.5f;
    [SerializeField] float stopWaitTime = 2f;

    [SerializeField] Transform waypointA;
    [SerializeField] Transform waypointB;

    [SerializeField] float rotationA;
    [SerializeField] float rotationB;

    float currentRotation;
    Transform currentWaypoint;

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

        currentWaypoint = waypointA;
        currentRotation = rotationA;
    }

    bool stopping = false;

    private void Update()
    {
        if (movePatrol)
        {
            Vector3 currentWaypointFlat = new Vector3(currentWaypoint.position.x, transform.position.y, currentWaypoint.position.z);

            if (Vector3.Distance(transform.position, currentWaypointFlat) < 0.1f && !stopping)
            {
                StartCoroutine(StopAtPoint());
                stopping = true;
            }
            else if (!stopping)
            {
                transform.position = Vector3.MoveTowards(transform.position, currentWaypointFlat, moveSpeed * Time.deltaTime);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(transform.position - currentWaypointFlat), rotationSpeed * Time.deltaTime);
            }
            
        }
        else if (rotatePatrol)
        {
            Vector3 targetRotation = new Vector3(transform.rotation.x, currentRotation, transform.rotation.z);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetRotation), rotationSpeed * Time.deltaTime);

            if (transform.rotation.y == currentRotation)
            {
                if (currentRotation == rotationA)
                {
                    currentRotation = rotationB;
                }
                else
                {
                    currentRotation = rotationA;
                }
            }
        }
    }

    private IEnumerator StopAtPoint()
    {
        yield return new WaitForSeconds(stopWaitTime);
        if (currentWaypoint == waypointA)
        {
            currentWaypoint = waypointB;
        }
        else
        {
            currentWaypoint = waypointA;
        }

        stopping = false;
    }

    public void Die()
    {
        if (hasKey)
        {
            Instantiate(keyPickupPrefab, keyPoint.position, keyPoint.rotation);
        }
        Instantiate(deathParticlesPrefab, particlePoint.position, particlePoint.rotation);
        gameObject.SetActive(false);
    }

    public void Reset()
    {
        //transform.localPosition = Vector3.zero;
    }
}
