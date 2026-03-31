using TMPro;
using UnityEngine;

public class RandomTest : MonoBehaviour
{
    public TextMeshProUGUI[] labels = new TextMeshProUGUI[6];
    int[] counts = new int[6];

    public int trials = 100;

    void Simulate()
    {
        /*// Unity Random (균등 분포)
        float chance = Random.value;    // 0~1 float
        int dice = Random.Range(1, 7);  // 1~6 int

        // System.Random
        System.Random sysRand = new System.Random();
        int number = sysRand.Next(1, 7);    // 1~6 int

        Debug.Log("Unity Random (Random.value) : " + chance);
        Debug.Log("Unity Random (Random.Range) : " + dice);
        Debug.Log("System Random (Next) : " + number);  // 1~6 int*/

        for (int i = 0; i < trials; i++)
        {
            int result = Random.Range(1, 7);
            counts[result - 1]++;
        }
        for (int i = 0; i <counts.Length; i++)
        {
            float percent = (float)counts[i] / trials * 100f;
            string result = $"{i + 1}: {counts[i]}회 {percent:F2}%";
            labels[i].text = result;
        }
    }

    public void ButtonClicked()
    {
        for(int i = 0; i <6; i++)
        {
            counts[i] = 0;
        }
        Simulate();
    }
}
