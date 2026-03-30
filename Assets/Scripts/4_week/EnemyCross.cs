using UnityEngine;

public class EnemyCross : MonoBehaviour
{
    public Transform target;

    private void Update()
    {
        Vector3 forward = transform.forward;
        Vector3 dirToTarget = (target.position - transform.position).normalized;

        Vector3 crossProduct = CrossProduct(forward, dirToTarget);

        if (crossProduct.y > 0.1f)
        {
            Debug.Log("플레이어가 오른쪽에 있습니다.");
        }
        else if (crossProduct.y < -0.1f)
        {
            Debug.Log("플레이어가 왼쪽에 있습니다.");
        }
    }

    Vector3 CrossProduct(Vector3 A, Vector3 B)
    {
        return new Vector3(
            A.y * B.z - A.z * B.y,
            A.z * B.x - A.x * B.z,
            A.x * B.y - A.y * B.x
            );
    }
}
