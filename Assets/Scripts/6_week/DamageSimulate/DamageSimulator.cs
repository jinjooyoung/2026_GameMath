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

    [Header("공격 횟수")]
    public int attackCount = 0;

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
        attackCount = 0;
        level = 1;
        baseDamage = level * DamageCoefficient;
    }

    public void SetWeapon(int id)
    {
        ResetData();
        currentWeapon = db.GetWeaponById(id);
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

    public void OnAttack()
    {
        // 정규분포 데미지 계산
        float sd = baseDamage * currentWeapon.stdDevMult;
        float normalDamage = GetNormalStdDevDamage(baseDamage, sd);

        // 치명타 판정
        bool isCrit = Random.value < currentWeapon.critRate;
        float finalDamage = isCrit ? normalDamage * currentWeapon.critMult : normalDamage;

        // 통계 누적
        attackCount++;
        totalDamage += finalDamage;

        // 로그 및 UI 업데이트
        ui.UpdateAttackLogUI(isCrit, finalDamage);
        ui.UpdateUI();
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
