using UnityEngine;
using UnityEngine.AI;
public class Avoidee : MonoBehaviour
{
    private NavMeshAgent agent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("Object is not a NavMeshAgent");
        }
        agent.speed = 3.5f;
        agent.SetDestination(new Vector3(10, this.transform.position.y, -10));
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.remainingDistance < 0.5f)
        {
            Vector3 newDestination = new Vector3(Random.Range(-10, 10), this.transform.position.y, Random.Range(-10, 10));
            agent.SetDestination(newDestination);
        }
    }
}
