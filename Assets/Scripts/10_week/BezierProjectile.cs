using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어에서 공격 입력이 들어왔을때 생성할 구 오브젝트 프리팹에 붙이는 스크립트
public class BezierProjectile : MonoBehaviour
{
    List<Vector3> points;

    public float radius = 3f;
    float time;
    float duration = 2f;

    // 생성 시 호출하는 시작점, 도착점, 중간 랜덤 제어점 초기화 함수
    public void Init(Vector3 start, Vector3 target, float duration)
    {
        Vector3 rand1 = Random.insideUnitSphere * radius;
        rand1.y = Mathf.Abs(rand1.y);               // 시작점보다 낮게 날아가지 않았으면 좋겠어서 양수만 나오게 함

        Vector3 rand2 = Random.insideUnitSphere * radius;
        rand2.y = Mathf.Abs(rand2.y);

        Vector3 p1 = start + rand1;

        Vector3 p2 = target + rand2;

        points = new List<Vector3>()
        {
            start,
            p1,
            p2,
            target
        };

        this.duration = duration;
    }

    void Update()
    {
        if (points == null)
            return;

        time += Time.deltaTime / duration;

        transform.position = DeCasteljau(points, time);

        if (time >= 1f)
            StartCoroutine(DestroyRoutine());
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

    IEnumerator DestroyRoutine()
    {
        GetComponent<MeshRenderer>().enabled = false;

        yield return new WaitForSeconds(1f);

        Destroy(gameObject);
    }
}