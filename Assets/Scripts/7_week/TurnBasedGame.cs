using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnBasedGame : MonoBehaviour
{
    [Header("UI 변수")]
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI rewardText;

    int totalEnemyCount = 0;
    int killedEnemyCount = 0;
    int totalHits = 0;
    int successHits = 0;
    int totalCrit = 0;
    float maxDamage = float.MinValue;
    float minDamage = float.MaxValue;

    int potionCount = 0;
    int goldCount = 0;
    int weaponNormal = 0;
    int weaponRare = 0;
    int armorNormal = 0;
    int armorRare = 0;

    [SerializeField] float critChance = 0.2f;
    [SerializeField] float meanDamage = 20f;
    [SerializeField] float stdDevDamage = 5f;
    [SerializeField] float enemyHP = 100f;
    [SerializeField] float poissonLambda = 2f;
    [SerializeField] float hitRate = 0.6f;
    [SerializeField] float critDamageRate = 2f;
    [SerializeField] int maxHitsPerTurn = 5;

    int turn = 0;
    bool rareItemObtained = false;

    float rareChance = 0.05f;

    string[] rewards = { "Gold", "Weapon", "Armor", "Potion" };

    public void StartSimulation()
    {
        // 기하분포 샘플링: 레어 아이템이 나올 때까지 반복하는 구조
        rareItemObtained = false;
        turn = 0;
        rareChance = 0.05f;

        ResetAll();

        while (!rareItemObtained)
        {
            SimulateTurn();
            turn++;

            rareChance += 0.05f;    // 턴마다 레어 아이템 확률 5% 증가
        }

        Debug.Log($"레어 아이템 {turn} 턴에 획득");
        UpdateResultUI();
        UpdateRewardUI();
    }

    void SimulateTurn()
    {
        Debug.Log($"--- Turn {turn + 1} ---");

        // 푸아송 샘플링: 적 등장 수
        int enemyCount = SamplePoisson(poissonLambda);
        Debug.Log($"적 등장 : {enemyCount}");
        totalEnemyCount += enemyCount;

        for (int i = 0; i < enemyCount; i++)
        {
            // 이항 샘플링: 명중 횟수
            int hits = SampleBinomial(maxHitsPerTurn, hitRate);
            float totalDamage = 0f;

            totalHits += maxHitsPerTurn;
            successHits += hits;

            for (int j = 0; j < hits; j++)
            {
                float damage = SampleNormal(meanDamage, stdDevDamage);

                // 최대 최소 갱신
                maxDamage = Mathf.Max(maxDamage, damage);
                minDamage = Mathf.Min(minDamage, damage);

                // 베르누이 분포 샘플링: 확률 기반 치명타 발생
                if (Random.value < critChance)
                {
                    damage *= critDamageRate;
                    totalCrit++;
                    Debug.Log($" 크리티컬 hit! {damage:F1}");
                }
                else
                    Debug.Log($" 일반 hit! {damage:F1}");

                totalDamage += damage;
            }

            if (totalDamage >= enemyHP)
            {
                Debug.Log($"적 {i + 1} 처치! (데미지: {totalDamage:F1})");
                killedEnemyCount++;

                // 균등 분포 샘플링: 보상 결정
                string reward = rewards[UnityEngine.Random.Range(0, rewards.Length)];
                Debug.Log($"보상: {reward}");

                switch (reward)
                {
                    case "Gold":
                        goldCount++;
                        break;

                    case "Potion":
                        potionCount++;
                        break;

                    case "Weapon":
                        if (Random.value < rareChance)
                        {
                            weaponRare++;
                            rareItemObtained = true;
                        }
                        else
                        {
                            weaponNormal++;
                        }
                        break;

                    case "Armor":
                        if (Random.value < rareChance)
                        {
                            armorRare++;
                            rareItemObtained = true;
                        }
                        else
                        {
                            armorNormal++;
                        }
                        break;
                }
            }
        }
    }

    // --- UI 갱신 함수들 ---
    void UpdateResultUI()
    {
        float hitRateResult = (float)successHits / totalHits * 100f;
        float critRateResult = (float)totalCrit / successHits * 100f;

        resultText.text =
            $"총 진행 턴 수 : {turn}\n" +
            $"발생한 적 : {totalEnemyCount}\n" +
            $"처치한 적 : {killedEnemyCount}\n" +
            $"공격 명중률 : {hitRateResult:F2}%\n" +
            $"치명타율 : {critRateResult:F2}%\n" +
            $"최대 데미지 : {maxDamage:F2}\n" +
            $"최소 데미지 : {minDamage:F2}";
    }

    void UpdateRewardUI()
    {
        rewardText.text =
            $"포션 : {potionCount}\n" +
            $"골드 : {goldCount}\n" +
            $"무기 - 일반 : {weaponNormal}\n" +
            $"무기 - 레어 : {weaponRare}\n" +
            $"방어구 - 일반 : {armorNormal}\n" +
            $"방어구 - 레어 : {armorRare}";
    }

    void ResetAll()
    {
        turn = 0;
        rareChance = 0.05f;
        rareItemObtained = false;

        totalEnemyCount = 0;
        killedEnemyCount = 0;
        totalHits = 0;
        successHits = 0;
        totalCrit = 0;

        maxDamage = float.MinValue;
        minDamage = float.MaxValue;

        potionCount = 0;
        goldCount = 0;
        weaponNormal = 0;
        weaponRare = 0;
        armorNormal = 0;
        armorRare = 0;
    }

    // --- 분포 샘플 함수들 ---
    int SamplePoisson(float lambda)
    {
        int k = 0;
        float p = 1f;
        float L = Mathf.Exp(-lambda);
        while (p > L)
        {
            k++;
            p *= Random.value;
        }
        return k - 1;
    }

    int SampleBinomial(int n, float p)
    {
        int success = 0;
        for (int i = 0; i < n; i++)
            if (Random.value < p) success++;
        return success;
    }

    float SampleNormal(float mean, float stdDev)
    {
        float u1 = Random.value;
        float u2 = Random.value;
        float z = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Cos(2.0f * Mathf.PI * u2);
        return mean + stdDev * z;
    }
}