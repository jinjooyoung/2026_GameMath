using UnityEngine;

public class SolarSystemInitialization : MonoBehaviour
{
    [Header("행성 오브젝트")]
    public GameObject[] planets;    // 11개

    [Header("크기")]
    public float[] sizes;

    [Header("공전 반지름")]
    public float[] distances;

    [Header("공전 속도")]
    public float[] revolutionSpeeds;

    [Header("자전 속도")]
    public float[] rotationSpeeds;

    void Awake()
    {
        sizes = new float[] { 5f, 0.5f, 0.7f, 0.8f, 0.3f, 0.6f, 2f, 1.7f, 1.3f, 1.2f, 0.2f };
        distances = new float[] { 0f, 8f, 12f, 16f, 2f, 20f, 28f, 36f, 44f, 52f, 60f };
        revolutionSpeeds = new float[] { 0f, 50f, 40f, 30f, 100f, 25f, 15f, 10f, 7f, 5f, 3f };
        rotationSpeeds = new float[] { 10f, 35f, 30f, 25f, 60f, 20f, 12f, 8f, 6f, 5f, 2f };
        Initialize();
    }

    void Initialize()
    {
        if (planets.Length != 11)
        {
            Debug.LogError("행성 개수가 11개가 아닙니다.");
            return;
        }

        // 0번 = 태양
        GameObject sun = planets[0];

        // 크기,위치 초기화
        for (int i = 0; i < planets.Length; i++)
        {
            planets[i].transform.localScale = Vector3.one * sizes[i];

            if (i == 0)
            {
                planets[i].transform.position = Vector3.zero;
            }
            else if (i == 4)    // 달
            {
                planets[i].transform.position =
                    planets[3].transform.position + new Vector3(distances[i], 0, 0);
            }
            else
            {
                planets[i].transform.position =
                    new Vector3(distances[i], 0, 0);
            }
        }

        // SolarSystem 스크립트 변수 할당
        for (int i = 0; i < planets.Length; i++)
        {
            SolarSystem ss = planets[i].GetComponent<SolarSystem>();

            if (ss == null)
            {
                Debug.LogWarning(planets[i].name + "에 SolarSystem 없음");
                continue;
            }

            // 중심점 설정
            if (i == 0)
            {
                ss.centerPlanet = null; // 태양은 중심 없음 근데 짜피 솔라시스템 스크립트 없어서 넘어감
            }
            else if (i == 4)    // 달
            {
                ss.centerPlanet = planets[3].transform; // 지구
            }
            else
            {
                ss.centerPlanet = sun.transform; // 나머지는 태양
            }

            // 수치 할당
            ss.revolutionRadius = distances[i];
            ss.revolutionSpeed = revolutionSpeeds[i];
            ss.rotationSpeed = rotationSpeeds[i];
        }
    }
}
