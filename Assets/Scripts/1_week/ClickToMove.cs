using UnityEngine;
using UnityEngine.InputSystem;

public class ClickToMove : MonoBehaviour
{
    public float moveSpeed;
    private Vector2 mouseScreenPosition;
    private Vector3 targetPosition;
    private Vector3 direction = Vector3.zero;
    private bool isMoving = false;
    private bool isSprinting = false;

    public float dashDistance = 5f;
    public float dashSpeed = 15f;
    private bool isDashing = false;
    private Vector3 dashTarget;

    public void OnPoint(InputValue value)
    {
        mouseScreenPosition = value.Get<Vector2>();
    }

    public void OnClick(InputValue value)
    {
        if (value.isPressed)
        {
            Ray ray = Camera.main.ScreenPointToRay(mouseScreenPosition);
            RaycastHit[] hits = Physics.RaycastAll(ray);

            foreach(RaycastHit hit in hits)
            {
                if (hit.collider.gameObject != gameObject)
                {
                    targetPosition = hit.point;
                    targetPosition.y = transform.position.y;
                    isMoving = true;

                    break;
                }
            }
        }
    }

    public void OnSprint(InputValue value)
    {
        isSprinting = value.isPressed;
    }

    public void OnJump(InputValue value)
    {
        if (isDashing) return;

        isDashing = value.isPressed;
        isMoving = false;
        isSprinting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing)
        {
            transform.position += NormalizationVector((direction == Vector3.zero ? Vector3.forward : direction) * dashDistance) * dashSpeed * Time.deltaTime;

            if (GetMagnitude(dashTarget - transform.position) < 0.1f)
            {
                isDashing = false;
            }

            return;
        }

        if (isMoving)
        {
            direction = targetPosition - transform.position;
            transform.position += NormalizationVector(direction) * moveSpeed * (isSprinting ? 3f : 1f) * Time.deltaTime;
            
            if (GetMagnitude(targetPosition - transform.position) < 0.1f)
            {
                isMoving = false;
            }
        }
    }



    public Vector3 NormalizationVector(Vector3 vector)
    {
        float magnitude = Mathf.Sqrt(GetMagnitude(vector));

        if (magnitude > 0)
            return vector / magnitude;
        else
            return Vector3.zero;
    }

    public float GetMagnitude(Vector3 vector)
    {
        return (float)Mathf.Sqrt(vector.x * vector.x + vector.y * vector.y + vector.z * vector.z);
    }
}
