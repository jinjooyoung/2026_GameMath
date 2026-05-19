using UnityEngine;

public class BallState : MonoBehaviour
{
    public bool isTouched = false;

    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("0p") || collision.collider.CompareTag("1p") || collision.collider.CompareTag("2p"))
            isTouched = true;
    }

    public bool IsMoving(float threshold)
    {
        Debug.Log($"IsMoving È£ÃâµÊ : {(rb.linearVelocity.magnitude > threshold || rb.angularVelocity.magnitude > threshold)}");

        return
        rb.linearVelocity.magnitude > threshold ||
        rb.angularVelocity.magnitude > threshold;
    }

    public void ResetTouched()
    {
        isTouched = false;
    }
}
