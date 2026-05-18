using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bezier : MonoBehaviour
{
    // 실습1
    public Transform p0;            // 시작점 (고정)
    public Transform p3;            // 도착점 (고정)

    [Header("Random Ranges")]
    public float p1Radius = 2f;     // p0 근처에서 뽑는 반경
    public float p2Radius = 2f;     // p3 근처에서 뽑는 반경
    public float p1Height = 3f;     // p1 Y축 추가 높이 (선택)
    public float p2Height = 3f;     // p2 Y축 추가 높이 (선택)

    // 결과 제어점
    [HideInInspector] public Vector3 p1;
    [HideInInspector] public Vector3 p2;

    List<Vector3> points;
    float time = 0f;

    private void Awake()
    {
        GenerateRandomControlPoints();
        points = new List<Vector3> {p0.position, p1, p2, p3.position};
    }

    private void Update()
    {
        time += Time.deltaTime / 2f;
        transform.position = DeCasteljau(points, time);
    }

    void GenerateRandomControlPoints()
    {
        Vector2 rand1 = Random.insideUnitCircle * p1Radius;
        p1 = p0.position + new Vector3(rand1.x, 0f, rand1.y);
        p1.y += p1Height;       // 살짝 위로 띄워 궤적 상승

        Vector2 rand2 = Random.insideUnitCircle * p2Radius;
        p2 = p3.position + new Vector3(rand2.x, 0f, rand2.y);
        p2.y += p2Height;       // 도착 직전 살짝 꺾이도록
    }

    Vector3 DeCasteljau(List<Vector3> p, float t)
    {
        while (p.Count > 1)
        {
            int last = p.Count - 1;

            var next = new List<Vector3>(last);
            for (int i = 0; i < last; i++)
                next.Add(Vector3.Lerp(p[i], p[i + 1], t));
            p = next;
        }

        return p[0];
    }


    //=================================이론 수업========================================

    /*[Header("3차 베지어 곡선")]
    public Transform point0;
    public Transform point1;
    public Transform point2;
    public Transform point3;
    float timeValue3 = 0f;

    [Header("4차 베지어 곡선")]
    public List<Transform> points = new List<Transform>();
    List<Vector3> pointPositions = new List<Vector3>();
    float timeValue4 = 0f;


    private void Awake()
    {
        foreach (var pt in points)
        {
            if (pt != null)
                pointPositions.Add(pt.position);
        }
    }

    void Update()
    {
        timeValue4 += Time.deltaTime / 2f;
        transform.position = DeCasteljau(pointPositions, timeValue4);

        //timeValue3 += Time.deltaTime / 2f;
        //transform.position = GetPointOnBezierCurve(point0.position, point1.position, point2.position, point3.position, timeValue3);
    }

    // 3차 베지어 곡선 구하기
    // P01, P12, P23, P0112, P1223, 최종 P0112와 P1223의 Lerp를 전개하면
    // B(t) = (1 - t)³P₀ + 3(1 - t)²tP₁ + 3(1 - t)t²P₂ + t³P₃ 라는 식이 나오므로 Mathf.Pow를 사용해서 (1-t)와 t의 제곱과 세제곱에
    // 각 p0,1,2,3을 곱해주는 방식으로 Vector3를 구한다
    Vector3 GetPointOnBezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        return Mathf.Pow(1 - t, 3) * p0
            + Mathf.Pow(1 - t, 2) * 3 * t * p1
            + Mathf.Pow(t, 2) * 3 * (1-t) * p2
            + Mathf.Pow(t, 3) * p3;
    }

    // 4차 이상
    Vector3 DeCasteljau(List<Vector3> p, float t)
    {
        while (p.Count > 1)
        {
            int last = p.Count - 1;     // 마지막 점 인덱스

            var next = new List<Vector3>(last);
            for (int i = 0; i < last; i++)
                next.Add(Vector3.Lerp(p[i], p[i + 1], t));
            p = next;                   // 한 단계 줄이기
        }

        // count가 1이 되면 p[0]에 남은 점이 곡선의 위치

        return p[0];
    }*/
}
