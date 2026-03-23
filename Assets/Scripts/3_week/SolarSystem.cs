using UnityEngine;

public class SolarSystem : MonoBehaviour
{
    [Header("공전 중심점")]
    public Transform centerPlanet;          // 공전 중심 위치

    [Header("수치")]
    public float revolutionRadius = 5f;     // 공전 반지름
    public float revolutionSpeed = 50f;     // 공전 속도
    public float rotationSpeed = 100f;      // 자전 속도

    private float angle;

    private void Update()
    {
        Revolution();
        SelfRotation();
    }

    void Revolution()
    {
        float revolutionSpeed = this.revolutionSpeed;

        angle += revolutionSpeed * Time.deltaTime;

        float rad = angle * Mathf.Deg2Rad;

        float x = Mathf.Cos(rad) * revolutionRadius;
        float z = Mathf.Sin(rad) * revolutionRadius;

        transform.position = centerPlanet.position + new Vector3(x, 0, z);
    }

    void SelfRotation()
    {
        float rotateSpeed = rotationSpeed;

        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
    }
}
