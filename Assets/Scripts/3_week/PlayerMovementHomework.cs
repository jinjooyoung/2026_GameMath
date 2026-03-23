using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementHomework : MonoBehaviour
{
    [Header("이동 속도")]
    public float moveSpeed = 5f;

    [Header("마우스 감도")]
    public float mouseSensitivity = 200f;

    private Vector2 moveInput;
    private float mouseX;

    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {
        mouseX = value.Get<Vector2>().x;
    }

    void Update()
    {
        Rotate();
        Move();
    }

    void Rotate()
    {
        // Y축 회전
        float rotation = mouseX * mouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * rotation);
    }

    void Move()
    {
        // 플레이어 기준 방향
        Vector3 direction = transform.forward * moveInput.y + transform.right * moveInput.x;

        Vector3 move = NormalizationVector(direction) * moveSpeed * Time.deltaTime;
        transform.Translate(move, Space.World);

        // 애니메이션
        float speed = move.magnitude / Time.deltaTime;
        animator.SetFloat("Speed", speed);
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
