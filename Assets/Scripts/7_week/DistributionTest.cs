using UnityEngine;

public class DistributionTest : MonoBehaviour
{
    // 정규 분포 : 평균 중심으로 대칭 f(x) = (1 / (σ√2π)) * e^(-(x - μ)² / (2σ²))
    float NormalDistribution(float mean, float stdDev)
    {
        float u1 = Random.value;
        float u2 = Random.value;
        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Cos(2.0f * Mathf.PI * u2);
        return mean + stdDev * randStdNormal;
    }

    // 푸아송 분포 : 단위 시간/공간 내 사건이 몇 번 발생하는지 P(k; λ) = (λ^k * e^(-λ)) / k!
    int PoissonDistribution(float lamda)
    {
        int k = 0;
        float p = 1f;
        float L = Mathf.Exp(-lamda);    // e^(-L)를 의미
        while (p > L)   // L은 무조건 1미만이기 때문에 1회는 진행함
        {
            k++;
            p *= Random.value;  // 랜덤 밸류가 낮게 자주 뜨면 p가 빠르게 감소하기 때문에 k도 같이 낮아짐
        }
        return k-1;     // 그래서 여기에서 -1을 해서 리턴해주는 것
    }

    // 이항 분포 : n번의 시도 중 k번 성공할 확률 P(k; n, p) = C(n, k) * p^k * (1 - p)^(n - k)
    int BinomialDistribution(int trials, float chance)
    {
        int successes = 0;
        for (int i = 0; i < trials; i++)
        {
            if (Random.value < chance)
                successes++;
        }
        return successes;
    }
    // 푸아송 분포와 이항 분포의 차이
    // 이항 분포는 10번 중 몇 번 성공? 과 같이 시도 횟수를 고정한다.
    // 푸아송 분포는 1분 동안 몇 번 일어났나? 와 같이 시간/공간 단위에서 측정한다.

    // 베르누이 분포 : 성공/실패 두 가지 결과만 있는 단일 시행(이항 분포의 기본 단위) P(x) = p^x * (1 - p)^(1 - x) (x는 0또는 1)
    bool Bernoullidistribution(float p)
    {
        return Random.value < p;
    }

    // 기하 분포 : 첫 성공이 나올 때까지 반복한 횟수 P(k) = (1 - p)^(k - 1) * p
    int GeometricDistribution(float p)
    {
        int tries = 1;
        while(Random.value >= p)
        {
            tries++;
        }
        return tries;
    }

    // 균등 분포 : 모든 값이 동일한 확률로 나올 때 (상수함수)
    int UniformDistribution(int min, int max)
    {
        return Random.Range(min, max);
    }

    void Start()
    {
        // 정규 분포
        /*for (int i = 0; i < 10; i++)
        {
            float sample = NormalDistribution(50f, 5f);
            Debug.Log($"Normal Sample {i + 1}: {sample:F2}");
        }*/

        // 푸아송 분포
        /*for (int i = 0; i < 10; i++)
        {
            int count = PoissonDistribution(3f);
            Debug.Log($"Minute {i + 1}: {count} events");
        }*/

        // 이항 분포
        /*int result = BinomialDistribution(10, 0.3f);
        Debug.Log($"Successes out of 10 trials: {result}");*/

        // 베르누이 분포
        /*bool result = Bernoullidistribution(0.2f);
        Debug.Log($"Trial result: {(result ? "Success" : "Fail")}");*/

        // 기하 분포
        /*int result = GeometricDistribution(0.1f);
        Debug.Log($"Tried until first success: {result}");*/

        // 균등 분포
        /*int result = UniformDistribution(0, 4);
        Debug.Log($"Uniform Sample: {result}");*/
    }
}
