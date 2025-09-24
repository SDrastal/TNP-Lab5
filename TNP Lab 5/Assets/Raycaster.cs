using UnityEngine;

public class Raycaster : MonoBehaviour
{

    public float rayDistance = 30f;
    public int numberOfRays = 30;
    public LayerMask hitLayers;

    // Update is called once per frame
    void Update()
    {
        CastRaysAroundObject();
    }

    void CastRaysAroundObject()
    {
        Vector2 center = transform.position;

        for (int i = 0; i < numberOfRays; i++)
        {
            float angle = i * (360f / numberOfRays);
            Vector3 direction = Quaternion.Euler(0, angle, 0) * transform.forward;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction, out hit, rayDistance, hitLayers))
            {
                Debug.DrawRay(transform.position, direction * hit.distance, Color.red);
                Debug.Log("Hit: " + hit.collider.name);
            }
            else
            {
                Debug.DrawRay(transform.position, direction * rayDistance, Color.green);
            }
        }
    }


}

