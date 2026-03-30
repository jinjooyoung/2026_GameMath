using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ChaserEnemy : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 3f;
    public float detectionAngle = 60f;
    public float dashSpeed = 5f;
    public float parryuingCheckDistance = 2f;
    public bool isDashing = false;

    public Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetFloat("Speed", 0f);
    }

    private void Update()
    {
        if (!isDashing) // 회전 모드
        {
            //transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
            // [과제] 내적을 사용하여 '전방 시야 60도 이내' 판정 함수 추가
            CheckPlayerInView();
        }
        else // Dash 모드 일 때 플레이어 쪽으로 가기. (거리판단해서 플레이어랑 가까우면 CheckParry 수행)
        {
            // 플레이어 방향 벡터
            Vector3 dir = (player.position - transform.position).normalized;

            // 플레이어 바라보기 (회전)
            transform.forward = dir;

            // 플레이어 쪽으로 이동 (돌진)
            transform.position += dir * dashSpeed * Time.deltaTime;
            animator.SetFloat("Speed", dashSpeed);

            // 거리 체크
            float distance = (player.position - transform.position).magnitude;

            if (distance < parryuingCheckDistance)
            {
                CheckParry();
            }
        }
    }

    void CheckPlayerInView()
    {
        Vector3 toPlayer = (player.position - transform.position);

        float distance = toPlayer.magnitude;

        // 거리 제한
        if (distance > detectionRange)
        {
            return;
        }

        // 방향 벡터
        Vector3 dirToPlayer = toPlayer.normalized;
        Vector3 forward = transform.forward;

        // 내적 계산
        float dot = DotProduct(forward, dirToPlayer);

        // 각도 계산
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

        // 시야각 체크
        if (angle < detectionAngle / 2f)
        {
            isDashing = true;
        }
    }

    void CheckParry()
    {
        PlayerController pc = player.GetComponent<PlayerController>();
        // [과제] 외적을 사용하여 플레이어 기준 왼쪽/오른쪽 패링 판정 추가
        Vector3 forward = player.forward;
        Vector3 dirToTarget = (transform.position - player.position).normalized;

        Vector3 crossProduct = CrossProduct(forward, dirToTarget);

        if (crossProduct.y > 0.1f)
        {
            //Debug.Log("플레이어의 오른쪽에 있습니다.");
            if (pc.rightParryTimer > 0f)
            {
                Debug.Log("패링성공!");
                Destroy(gameObject);
            }
        }
        else if (crossProduct.y < -0.1f)
        {
            //Debug.Log("플레이어의 왼쪽에 있습니다.");
            if (pc.leftParryTimer > 0f)
            {
                Debug.Log("패링성공!");
                Destroy(gameObject);
            }
        }
    }

    float DotProduct(Vector3 a, Vector3 b)
    {
        return a.x * b.x + a.y * b.y + a.z * b.z;
    }

    Vector3 CrossProduct(Vector3 A, Vector3 B)
    {
        return new Vector3(
            A.y * B.z - A.z * B.y,
            A.z * B.x - A.x * B.z,
            A.x * B.y - A.y * B.x
            );
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }
    }

    void OnDrawGizmos()
    {
        DrawForwardRay();
    }

    private void DrawForwardRay()
    {
        Vector3 startPos = transform.position;
        Vector3 forwardDir = transform.forward * detectionRange;
        Vector3 endPos = startPos + forwardDir;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(startPos, forwardDir);

        // 적 시야 범위
        Gizmos.color = Color.yellow;

        Vector3 forward = transform.forward * detectionRange;

        // 왼쪽 시야 경계
        Vector3 leftBoundary = Quaternion.Euler(0, -detectionAngle / 2f, 0) * transform.forward * detectionRange;
        // 오른쪽 시야 경계
        Vector3 rightBoundary = Quaternion.Euler(0, detectionAngle / 2f, 0) * transform.forward * detectionRange;

        Gizmos.DrawRay(transform.position, leftBoundary);
        Gizmos.DrawRay(transform.position, rightBoundary);

        // 적 감지 범위
        DrawCircle(transform.position, parryuingCheckDistance);
    }

    void DrawCircle(Vector3 center, float radius, int segments = 30)
    {
        Gizmos.color = Color.red;

        float angle = 0f;
        float angleStep = 360f / segments;

        Vector3 prevPoint = center + new Vector3(Mathf.Cos(0), 0, Mathf.Sin(0)) * radius;

        for (int i = 1; i <= segments; i++)
        {
            angle += angleStep * Mathf.Deg2Rad;

            Vector3 newPoint = center + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;

            Gizmos.DrawLine(prevPoint, newPoint);
            prevPoint = newPoint;
        }
    }
}
