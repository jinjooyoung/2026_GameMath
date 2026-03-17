using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 moveInput;

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void Update()
    {
        Vector3 direction = new Vector3(moveInput.x, moveInput.y, 0);
        transform.Translate(NormalizationVector(direction) * moveSpeed * Time.deltaTime);
    }

    public Vector3 NormalizationVector(Vector3 vector)
    {
        float sqrMagnitude = vector.x * vector.x + vector.y * vector.y + vector.z * vector.z;
        float magnitude = Mathf.Sqrt(sqrMagnitude);

        if (magnitude > 0)
            return vector / magnitude;
        else
            return Vector3.zero;
    }
}
