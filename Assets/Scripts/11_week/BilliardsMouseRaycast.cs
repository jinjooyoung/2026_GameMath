using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BilliardsMouseRaycast : MonoBehaviour
{
    [Header("공 리스트")]
    public List<BallState> balls;

    public BilliardsScoreManager scoreManager;

    public float stopThreshold = 0.1f;

    public float rayDistance = 100f;
    public float forcePower = 10f;

    bool turnStarted = false;
    bool turnProcessing = false;

    int turnPlayer = 1;

    public void OnClick(InputValue value)
    {
        if (turnProcessing)
            return;

        if (!value.isPressed)
            return;

        Vector2 mousePosition = Mouse.current.position.ReadValue();

        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
        {
            Debug.DrawLine(
                ray.origin,
                hit.point,
                Color.red,
                2f);

            // 히트한 오브젝트 태그 얻기
            string hitTag = hit.collider.tag;

            // 현재 턴 플레이어 공인지 검사
            if (hitTag != $"{turnPlayer}p")
                return;

            foreach (var ball in balls)
            {
                ball.ResetTouched();
            }

            Rigidbody rb = hit.collider.attachedRigidbody;

            if (rb != null)
            {
                Vector3 dir = (hit.collider.transform.position - hit.point).normalized;
                rb.AddForce(dir * forcePower, ForceMode.Impulse);

                turnProcessing = true;
                turnStarted = false;
            }
        }
    }

    private void Update()
    {
        Debug.Log(turnProcessing);

        if (!turnProcessing)
            return;

        bool allStopped = true;

        foreach (var ball in balls)
        {
            if (ball.IsMoving(stopThreshold))
            {
                allStopped = false;
                turnStarted = true;
            }
        }

        if (turnStarted && allStopped)
        {
            EndTurn();
        }
    }

    void EndTurn()
    {
        Debug.Log("엔드턴 호출됨");
        turnProcessing = false;

        int touchedTargetCount = 0;
        bool touchedEnemy = false;

        foreach (var ball in balls)
        {
            if (!ball.isTouched)
                continue;

            // 흰 공(0p) 맞춘 개수 체크
            if (ball.CompareTag("0p"))
            {
                touchedTargetCount++;
            }

            // 상대 공 맞췄는지 체크
            if (ball.CompareTag($"{(turnPlayer == 1 ? 2 : 1)}p"))
                touchedEnemy = true;
        }

        // 상대 공 맞췄으면 무조건 -1
        if (touchedEnemy)
        {
            scoreManager.AddScore(turnPlayer, -1);
        }
        // 0p 공 2개 모두 맞췄고 상대 공 안 맞췄으면 +1
        else if (touchedTargetCount >= 2)
        {
            scoreManager.AddScore(turnPlayer, 1);
        }

        turnPlayer =
            turnPlayer == 1 ? 2 : 1;
    }
}
