using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GachaSystem : MonoBehaviour
{
    Dictionary<string, float> defaultTable = new()
    {
        { "일반", 50f },
        { "고급", 30f },
        { "희귀", 15f },
        { "전설", 5f }
    };

    Dictionary<string, float> runTimeTable = new();

    public int normalCount, rareCount, epicCount, legendaryCount;

    void Start()
    {
        ResetTable();
    }

    public void DropItem()
    {
        string result = RollByWeight();

        switch (result)
        {
            case "일반": normalCount++; break;
            case "고급": rareCount++; break;
            case "희귀": epicCount++; break;
            case "전설": legendaryCount++; break;
        }

        if (result == "전설")
            ResetTable();
        else
            BuffLegendary();

        Debug.Log($"{result} 획득");
    }

    // 실습 1-4 확률 가중치 딕셔너리 등급 테이블 코드 참고 하여 가중치로 확률 조정
    string RollByWeight()
    {
        float totalWeight = runTimeTable.Values.Sum();      // 전체 가중치 합
        float roll = Random.Range(0f, totalWeight);         // 랜덤 값
        float accumulator = 0f;

        foreach (var pair in runTimeTable)
        {
            accumulator += pair.Value;

            if (roll <= accumulator)
            {
                return pair.Key;
            }
        }

        return "일반";    // fallback
    }

    void ResetTable()
    {
        runTimeTable.Clear();

        foreach (var item in defaultTable)
        {
            runTimeTable.Add(item.Key, item.Value);
        }
    }

    void BuffLegendary()
    {
        runTimeTable["전설"] += 1.5f;

        runTimeTable["일반"] = Mathf.Max(0f, runTimeTable["일반"] - 0.5f);
        runTimeTable["고급"] = Mathf.Max(0f, runTimeTable["고급"] - 0.5f);
        runTimeTable["희귀"] = Mathf.Max(0f, runTimeTable["희귀"] - 0.5f);
    }

    public Dictionary<string, float> GetTable() => runTimeTable;
}