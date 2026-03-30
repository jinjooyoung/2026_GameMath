using UnityEngine;

public class AutoRotation : MonoBehaviour
{
    public float angle = 45f;

    // Y축을 기준으로 1초마다 angle도 회전
    void Update()
    {
        transform.Rotate(0, angle * Time.deltaTime, 0);
    }
}
