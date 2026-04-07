using TMPro;
using UnityEngine;

public class DamageSimulatorUI : MonoBehaviour
{
    [Header("UI 오브젝트")]
    public TextMeshProUGUI statusDisplay;
    public TextMeshProUGUI logDisplay;
    public TextMeshProUGUI resultDisplay;
    public TextMeshProUGUI rangeDisplay;

    [Header("참조 스크립트")]
    public WeaponDB weaponDB;
    public DamageSimulator simulator;

    public void UpdateUI()
    {
        WeaponSO weapon = simulator.currentWeapon;
        int level = simulator.level;
        float baseDamage = simulator.baseDamage;
        int attackCount = simulator.attackCount;
        int critCount = simulator.critCount;
        int weakCount = simulator.weakCount;
        int failCount = simulator.failCount;
        float totalDamage = simulator.totalDamage;
        float maximumDamage = simulator.maximumDamage;

        statusDisplay.text =
            $"Level: {level} / 무기: {weapon.name}\n" +
            $"기본 데미지: {baseDamage} / 치명타: {weapon.critRate * 100}% (x{weapon.critMult})";

        rangeDisplay.text =
            $"예상 일반 데미지 범위: [{(baseDamage - (3 * baseDamage * weapon.stdDevMult)).ToString("F1")} ~ " +
            $"{(baseDamage + (3 * baseDamage * weapon.stdDevMult)).ToString("F1")}]";

        float dpa = attackCount > 0 ? totalDamage / attackCount : 0;
        resultDisplay.text =
            $"누적 데미지: {totalDamage.ToString("F1")}\n" +
            $"공격 횟수: {attackCount}\n" +
            $"평균 DPA: {dpa.ToString("F2")}\n" +
            $"약점 공격 횟수: {weakCount}\n" +
            $"공격 실패 횟수: {failCount}\n" +
            $"전체 크리티컬 횟수: {critCount}\n" +
            $"최대 데미지: {maximumDamage.ToString("F1")}\n";
    }

    public void UpdateLvUpLogUI()
    {
        int level = simulator.level;

        logDisplay.text = string.Format("레벨업! 현재 레벨: {0}", level);
    }

    public void UpdateAttackLogUI(bool _isCrit, bool _isWeak, bool _isFail, float damage)
    {
        string critMark = _isCrit ? "<color=red>치명타 </color> " : "";
        string weakMark = _isWeak ? "<color=yellow>약점 </color> " : "";

        if (_isFail)
        {
            logDisplay.text = "공격 실패! 데미지: 0";
        }
        else
        {
            logDisplay.text = string.Format("{0}{1}공격! 데미지: {2:F1}", critMark, weakMark, damage);
        }
    }
}
