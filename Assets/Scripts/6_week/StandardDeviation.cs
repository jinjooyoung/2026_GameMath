using System.Linq;
using UnityEngine;

public class StandardDeviation : MonoBehaviour
{
    [Header("[실습1] : 시행횟수, 최소최댓값")]
    public int sampleCount = 10000;
    public int randomMin;
    public int randomMax;

    [Header("[실습2] : 평균, 표준편차")]
    public float mean;
    public float stdDev;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StandardDeviationTest();
    }

    // 실습1
    void StandardDeviationTest()
    {
        float[] samples = new float[sampleCount];
        for(int i = 0; i < sampleCount; i++)
        {
            samples[i] = Random.Range(randomMin, randomMax);
        }

        float mean = samples.Average();
        float sumOfSquares = samples.Sum(x => Mathf.Pow(x - mean, 2));
        float stdDev = Mathf.Sqrt(sumOfSquares / sampleCount);

        Debug.Log($"평균: {mean}, 표준편차: {stdDev}");
    }

    // 실습2
    float GenerateGaussian(float mean, float stdDev)
    {
        float u1 = 1.0f - Random.value; // 0보다 큰 난수
        float u2 = 1.0f - Random.value; // 0보다 큰 난수

        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) *
                            Mathf.Sin(2.0f * Mathf.PI * u2);        // 표준 정규 분포

        return mean + stdDev * randStdNormal;   // 원하는 평균과 표준편차로 변환
    }

    public void OnButtonClick()
    {
        Debug.Log($"평균: {mean}, 표준편차: {stdDev}, 해당 랜덤 값 : {GenerateGaussian(mean, stdDev)}");
    }
}
