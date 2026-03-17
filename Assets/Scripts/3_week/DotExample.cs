using UnityEngine;

public class DotExample : MonoBehaviour
{
    public Transform player;
    public float viewAngle = 60f;   // 시야각
    public Material material;

    // Update is called once per frame
    void Update()
    {
        Vector3 toPlayer = (player.position - transform.position).normalized;
        Vector3 forward = transform.forward;

        float dot = TestDot(forward, toPlayer);
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

        material.color = Color.white;

        if (angle < viewAngle / 2)
        {
            //Debug.Log("플레이어가 시야 안에 있음!");
            material.color = Color.red;
        }

        /*forward.Normalize();
        toPlayer.Normalize();

        float dot = Vector3.Dot(forward, toPlayer);

        if (dot >  0f)
        {
            Debug.Log("플레이어가 적의 앞쪽에 있음");
        }
        else if (dot < 0f)
        {
            Debug.Log("플레이어가 적의 뒤쪽에 있음");
        }
        else
        {
            Debug.Log("플레이어가 적의 옆쪽에 있음");
        }*/
    }

    private float TestDot(Vector3 a, Vector3 b)
    {
        return a.x * b.x + a.y * b.y + a.z * b.z;
    }
}
