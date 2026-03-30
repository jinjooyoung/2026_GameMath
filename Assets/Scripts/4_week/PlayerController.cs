using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 30f;
    private Vector2 moveInput;

    public float parryWindow = 0.2f; // 패링 유효 시간

    public float leftParryTimer = 0f;
    public float rightParryTimer = 0f;

    public bool isLeftRarrying = false;
    public bool isRightParrying = false;

    public Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetFloat("Speed", 0f);
    }

    // 왼쪽 패링 입력
    public void OnLeftParry(InputValue value)
    {
        if (value.isPressed)
        {
            Debug.Log("왼쪽 패링 입력됨!");
            leftParryTimer = parryWindow;
        }
        //isLeftRarrying = value.isPressed;
    }

    // 오른쪽 패링 입력
    public void OnRightParry(InputValue value)
    {
        if (value.isPressed)
        {
            Debug.Log("오른쪽 패링 입력됨!");
            rightParryTimer = parryWindow;
        }
        //isRightParrying = value.isPressed;
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void Update()
    {
        float rotation = moveInput.x * rotationSpeed * Time.deltaTime;
        transform.Rotate(0f, rotation, 0f);

        Vector3 moveDir = moveInput.y * moveSpeed * Time.deltaTime * transform.forward;
        transform.position += moveDir;

        // 패링 타이머 감소
        if (leftParryTimer > 0f)
            leftParryTimer -= Time.deltaTime;

        if (rightParryTimer > 0f)
            rightParryTimer -= Time.deltaTime;

        float speed = moveInput.magnitude;
        animator.SetFloat("Speed", speed);
    }
}
