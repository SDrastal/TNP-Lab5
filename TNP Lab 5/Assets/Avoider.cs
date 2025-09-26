using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Avoider : MonoBehaviour
{
    private NavMeshAgent agent;
    public GameObject avoidee;
    public float safeDistance = 5f;
    public float avoidSpeed = 3.5f;
    private Vector3 avoideePosition;
    private Vector3 avoiderPosition;
    private bool detected;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("Object is not a NavMeshAgent");
        }
        if (avoidee == null)
        {
            Debug.LogError("Avoidee GameObject is not assigned");
        }

        agent.speed = avoidSpeed;
    }

    public bool IsTooClose(GameObject obj1, GameObject obj2, float givenDistance)
    {
        float distance = Vector3.Distance(obj1.transform.position, obj2.transform.position);
        return distance <= givenDistance;
    }

    private void Update()
    {
        avoideePosition = avoidee.transform.position;
        avoiderPosition = transform.position;

        detected = IsTooClose(gameObject, avoidee, safeDistance);

        if (detected)
            StartCoroutine(CastRaysAroundObject());
        else
        {
            agent.SetDestination(avoiderPosition);
            StopAllCoroutines();
        }
    }

    IEnumerator CastRaysAroundObject()
    {
        int numberOfRays = 64;
        float rayDistance = safeDistance;

        for (int i = 0; i < numberOfRays; i++)
        {
            float angle = i * (360f / numberOfRays);
            Vector3 direction = Quaternion.Euler(0, angle, 0) * transform.forward;

            RaycastHit hit;
            int layerMask = Physics.DefaultRaycastLayers; // Use all layers to ensure detection
            if (Physics.Raycast(transform.position, direction, out hit, rayDistance, layerMask))
            {
                if (hit.collider.gameObject == avoidee)
                {
                    Debug.Log("Hit avoidee: " + hit.collider.name);
                    Debug.DrawRay(transform.position, direction * hit.distance, Color.red);

                    StartCoroutine(PoissonDiscMove());
                }
            }
            else
            {
                Debug.DrawRay(transform.position, direction * rayDistance, Color.green);
            }
        }
        yield return null;
    }
    bool CheckIfSeen(Vector3 sample)
    {
        RaycastHit hit;
        Vector3 direction = (avoideePosition - sample).normalized;
        int layerMask = Physics.DefaultRaycastLayers; // Use all layers to ensure detection
        if (Physics.Raycast(sample, direction, out hit, Mathf.Infinity, layerMask))
        {
            if (hit.collider.gameObject == avoidee)
            {
                Debug.DrawRay(sample, direction * hit.distance, Color.purple);
                return true;
            }
            else
            {
                Debug.DrawRay(sample, direction * hit.distance, Color.blue);
                return false;
            }
        }
        return false;
    }

    IEnumerator PoissonDiscMove()
    {
        List<Vector3> samples = new List<Vector3>();

        for (int j = 0; j < 10; j++)
        {
            samples.Add(new Vector3(j * 2, 0, 0));
            samples.Add(-new Vector3(j * 2, 0, 0));

            for (int k = 0; k < 10; k++)
            {
                samples.Add(new Vector3(j * 2, 0, k * 2));
                samples.Add(-new Vector3(j * 2, 0, k * 2));
            }
        }
        Vector3 closestPoint = samples[99];
        Vector3 previousClosestPoint = closestPoint;

        foreach (var sample in samples)
        {
            if (!CheckIfSeen(sample))
            {
                // Find the sample point that is closest
                if ((sample - transform.position).sqrMagnitude <
                    (closestPoint - transform.position).sqrMagnitude)
                {
                    Debug.Log("New closest safe point found: " + sample);
                    closestPoint = sample;
                }
            }
        }
        Debug.Log("Closest safe point: " + closestPoint);
        if (previousClosestPoint != closestPoint)
            agent.SetDestination(closestPoint);

        previousClosestPoint = closestPoint;

        samples.Clear();
        yield return null;
    }
}
