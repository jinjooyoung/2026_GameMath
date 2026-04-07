using TMPro;
using UnityEngine;

public class DamageSimulator : MonoBehaviour
{
    [Header("레벨")]
    public int level = 1;

    [Header("현재 장착중인 무기")]
    public WeaponSO currentWeapon;

    [Header("데미지 수치")]
    public float baseDamage;
    public const float DamageCoefficient = 20f;
    public float totalDamage = 0;
    public float maximumDamage = 0;

    [Header("공격 횟수")]
    public int attackCount = 0;
    public int critCount = 0;
    public int weakCount = 0;
    public int failCount = 0;

    [Header("참조 스크립트")]
    public WeaponDB db;
    public DamageSimulatorUI ui;


    void Start()
    {
        SetWeapon(0);
    }

    private void ResetData()
    {
        totalDamage = 0;
        maximumDamage = 0;
        attackCount = 0;
        critCount = 0;
        weakCount = 0;
        failCount = 0;
        level = 1;
        baseDamage = level * DamageCoefficient;
    }

    public void SetWeapon(int id)
    {
        ResetData();
        currentWeapon = db.GetWeaponById(id);
        ui.UpdateLvUpLogUI();
        ui.UpdateUI();
    }

    public void LevelUp()
    {
        totalDamage = 0;
        attackCount = 0;
        level++;
        baseDamage = level * DamageCoefficient;
        ui.UpdateLvUpLogUI();
        ui.UpdateUI();
    }

    public void OnAttackTimes(int times)
    {
        if (times < 0) return;

        for (int i = 0; i < times; i++)
        {
            OnAttack();
        }
    }

    public void OnAttack()
    {
        // 정규분포 데미지 계산
        float sd = baseDamage * currentWeapon.stdDevMult;
        float normalDamage = GetNormalStdDevDamage(baseDamage, sd);

        // 치명타 판정
        bool isCrit = Random.value < currentWeapon.critRate;
        float finalDamage = isCrit ? normalDamage * currentWeapon.critMult : normalDamage;

        if (isCrit) critCount++;

        bool isWeak = false;
        bool isFail = false;

        // 일반 데미지가 정규분포 +2σ 초과 (약점 공격)
        if (normalDamage > baseDamage + (sd * 2))
        {
            isWeak = true;
            finalDamage *= 2;
            attackCount++;
            weakCount++;
            totalDamage += finalDamage;
            SaveMaximumDamage(finalDamage);
        }
        // 일반 데미지가 정규분포 -2σ 미만 (공격 실패)
        else if (normalDamage < baseDamage - (sd * 2))
        {
            isFail = true;
            finalDamage = 0;
            if (isCrit) critCount--;
            failCount++;
        }
        // 기타 일반 or 크리 공격
        else
        {
            attackCount++;
            totalDamage += finalDamage;
            SaveMaximumDamage(finalDamage);
        }

        // 로그 및 UI 업데이트
        ui.UpdateAttackLogUI(isCrit, isWeak, isFail, finalDamage);
        ui.UpdateUI();
    }

    private void SaveMaximumDamage(float damage)
    {
        if (maximumDamage < damage)
        {
            maximumDamage = damage;
        }
    }

    float GetNormalStdDevDamage(float mean, float stdDev)
    {
        float u1 = 1.0f - Random.value; // 0보다 큰 난수
        float u2 = 1.0f - Random.value; // 0보다 큰 난수

        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) *
                            Mathf.Sin(2.0f * Mathf.PI * u2);        // 표준 정규 분포

        return mean + stdDev * randStdNormal;   // 원하는 평균과 표준편차로 변환
    }
}
