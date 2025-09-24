using UnityEngine;
using UnityEngine.AI;

public class Avoider : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject avoidee;
    public float safeDistance = 5f;
    private Vector3 avoideePosition;
    private Vector3 avoiderPosition;
    private bool detected;

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
    }
}
