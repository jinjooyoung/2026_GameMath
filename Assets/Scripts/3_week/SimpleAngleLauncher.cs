using TMPro;
using UnityEngine;

public class SimpleAngleLauncher : MonoBehaviour
{
    public TMP_InputField angleInputField;
    public GameObject spherePrefab;
    public Transform firePoint;
    public float force = 15;

    public void Launch()
    {
        float angle = float.Parse(angleInputField.text);
        float rad = angle * Mathf.Deg2Rad;

        Vector3 dir = new Vector3(Mathf.Cos(rad), 0f, Mathf.Sin(rad));

        GameObject sphere = Instantiate(spherePrefab, firePoint.position, Quaternion.identity);
        Rigidbody rb = sphere.GetComponent<Rigidbody>();

        rb.AddForce((dir + Vector3.up * 0.3f).normalized * force, ForceMode.Impulse);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        /*float degrees = 45f;
        float radians = degrees * Mathf.Deg2Rad;
        Debug.Log("45도 -> 라디안 : " + radians);

        float radianValue = Mathf.PI / 3;
        float degreeValue = radianValue * Mathf.Rad2Deg;
        Debug.Log("파이/3 라디안 -> 도 변환 : " + degreeValue);*/
    }
}
