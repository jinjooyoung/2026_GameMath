using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public int attackDamage = 30;
    public int maxPlayCount = 500;
    public CriticalManager critManager;
    public BattleManager battleManager;
    public BattleUIManager uiManager;

    public void OnAttackButton()
    {
        int count = 1; // 기본값

        // 입력값 파싱
        if (!string.IsNullOrEmpty(uiManager.repeatInput.text))
        {
            if (!int.TryParse(uiManager.repeatInput.text, out count))
            {
                Debug.LogWarning("숫자만 입력하세요!");
                return;
            }
        }

        // 범위 제한
        if (count < 1)
        {
            Debug.LogWarning("최소 1 이상 입력하세요.");
            return;
        }

        // 너무 많이 시행하면 컴이 죽을까봐...
        if (count > maxPlayCount)
        {
            Debug.LogWarning($"최대 {maxPlayCount}까지 가능합니다.");
            count = maxPlayCount;
        }

        for(int i =0; i < count; i++)
        {
            bool isCrit = critManager.RollCrit();

            int damage = attackDamage;

            if (isCrit)
            {
                damage *= 2;
            }

            battleManager.TakeDamage(damage, isCrit);
        }

        uiManager.UpdateUI();
        uiManager.PlayHitEffect(false);     // 원래 isCrit 넣으려고 했는데 여러번 시행하게 수정했더니 여러번 호출하면 부담될까봐 그냥 false로 고정 해둠
    }
}
