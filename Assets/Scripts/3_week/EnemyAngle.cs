using System.Collections;
using UnityEngine;

public class EnemyAngle : MonoBehaviour
{
    [Header("플레이어 위치")]
    public Transform player;

    [Header("시야 설정")]
    public float viewAngle = 60f;
    public float viewDistance = 10f;

    [Header("스케일")]
    public float normalScale = 1f;
    public float alertScale = 2f;

    [Header("이동 지점")]
    public Transform startPoint;
    public Transform endPoint;

    [Header("이동 설정")]
    public float moveSpeed = 2f;
    public float waitTime = 3f;

    private Transform targetPoint;
    private Animator animator;
    private bool isWaiting = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        targetPoint = endPoint;

        // 시작 위치 고정
        transform.position = startPoint.position;
    }

    void Update()
    {
        CheckPlayerInView();
        if (isWaiting) return;

        Move();
    }

    void CheckPlayerInView()
    {
        Vector3 toPlayer = (player.position - transform.position);

        float distance = toPlayer.magnitude;

        // 거리 제한
        if (distance > viewDistance)
        {
            SetNormal();
            return;
        }

        // 방향 벡터
        Vector3 dirToPlayer = toPlayer.normalized;
        Vector3 forward = transform.forward;

        // 내적 계산
        float dot = Dot(forward, dirToPlayer);

        // 각도 계산
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

        // 시야각 체크
        if (angle < viewAngle / 2f)
        {
            SetAlert();
        }
        else
        {
            SetNormal();
        }
    }

    float Dot(Vector3 a, Vector3 b)
    {
        return a.x * b.x + a.y * b.y + a.z * b.z;
    }

    void SetAlert()
    {
        transform.localScale = Vector3.one * alertScale;
    }

    void SetNormal()
    {
        transform.localScale = Vector3.one * normalScale;
    }

    //==========================================================

    void Move()
    {
        Vector3 direction = (targetPoint.position - transform.position);
        float distance = direction.magnitude;

        Vector3 dir = direction.normalized;

        // 이동
        transform.Translate(dir * moveSpeed * Time.deltaTime, Space.World);

        // 바라보기
        if (dir != Vector3.zero)
        {
            transform.forward = dir;
        }

        // 애니메이션
        float speed = moveSpeed;
        animator.SetFloat("Speed", speed);

        // 도착 체크
        if (distance < 0.1f)
        {
            StartCoroutine(WaitAndTurn());
        }
    }

    IEnumerator WaitAndTurn()
    {
        isWaiting = true;

        // 멈춤 애니메이션
        animator.SetFloat("Speed", 0f);

        yield return new WaitForSeconds(waitTime);

        // 목표 변경
        targetPoint = (targetPoint == startPoint) ? endPoint : startPoint;

        isWaiting = false;
    }

    // 이동 경로 Gizmos
    void OnDrawGizmos()
    {
        // 적 시야 범위
        Gizmos.color = Color.yellow;

        Vector3 forward = transform.forward * viewDistance;

        // 왼쪽 시야 경계
        Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle / 2, 0) * forward;
        // 오른쪽 시야 경계
        Vector3 rightBoundary = Quaternion.Euler(0, viewAngle / 2, 0) * forward;

        Gizmos.DrawRay(transform.position, leftBoundary);
        Gizmos.DrawRay(transform.position, rightBoundary);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, forward);

        // 적 이동
        if (startPoint == null || endPoint == null) return;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(startPoint.position, endPoint.position);

        Gizmos.DrawSphere(startPoint.position, 0.3f);
        Gizmos.DrawSphere(endPoint.position, 0.3f);
    }
}
