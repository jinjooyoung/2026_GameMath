using System.Collections;
using TMPro;
using UnityEditor.EditorTools;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIManager : MonoBehaviour
{
    [Header("정보 텍스트")]
    public TextMeshProUGUI battleSummaryText;
    public TextMeshProUGUI itemProbabilityText;
    public TextMeshProUGUI droppedItemText;
    public TMP_InputField repeatInput;      // 테스트 횟수 입력

    [Header("몬스터 정보")]
    public TextMeshProUGUI hpText;
    public Image monsterImage;

    [Header("참조 스크립트")]
    public BattleManager battleManager;
    public CriticalManager critManager;
    public GachaSystem gachaSystem;

    [Header("공격 연출 수치")]
    public float shakeDuration = 0.2f;
    public float shakeStrength = 10f;

    private void Start()
    {
        UpdateUI();
    }

    // 텍스트에 수치 업데이트
    public void UpdateUI()
    {
        float actualRate = 0f;
        if (critManager.totalHits > 0)
            actualRate = (float)critManager.critHits / critManager.totalHits;

        battleSummaryText.text =
            $"총 공격 횟수 : {critManager.totalHits}\n" +
            $"크리티컬 횟수 : {critManager.critHits}\n" +
            $"설정 확률 : {critManager.targetRate * 100f:F2}%\n" +
            $"실제 확률 : {actualRate * 100f:F2}%";

        hpText.text = $"체력 : {battleManager.currentHp}/{battleManager.maxHp}";

        var table = gachaSystem.GetTable();

        itemProbabilityText.text =
            "현재 아이템 확률\n\n" +
            $"일반 : {table["일반"]:F1}%\n" +
            $"고급 : {table["고급"]:F1}%\n" +
            $"희귀 : {table["희귀"]:F1}%\n" +
            $"전설 : {table["전설"]:F1}%";

        int totalItemCount = gachaSystem.normalCount + gachaSystem.rareCount + gachaSystem.epicCount + gachaSystem.legendaryCount;

        droppedItemText.text =
            $"현재 드롭된 아이템 (총 {totalItemCount}개)\n\n" +
            $"일반 : {gachaSystem.normalCount}\n" +
            $"고급 : {gachaSystem.rareCount}\n" +
            $"희귀 : {gachaSystem.epicCount}\n" +
            $"전설 : {gachaSystem.legendaryCount}";
    }

    // 타격 효과
    public void PlayHitEffect(bool isCrit)
    {
        RectTransform rect = monsterImage.rectTransform;

        // 기존 트윈 제거
        rect.DOKill();
        monsterImage.DOKill();

        float duration = isCrit ? shakeDuration * 1.5f : shakeDuration;
        float strength = isCrit ? shakeStrength * 2f : shakeStrength;

        Color originalColor = monsterImage.color;

        // 좌우 흔들림
        rect.DOShakeAnchorPos(duration, new Vector2(strength, 0f), 20, 90, false, true);

        // 스케일 튕김
        rect.DOPunchScale(Vector3.one * 0.2f, duration, 10, 1);

        // 빨간색 번쩍
        monsterImage.DOColor(Color.red, 0.05f)
            .OnComplete(() =>
            {
                monsterImage.DOColor(originalColor, 0.1f);
            });
    }
}
