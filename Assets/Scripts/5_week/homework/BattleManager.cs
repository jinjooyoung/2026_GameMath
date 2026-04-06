using UnityEditor.EditorTools;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public int maxHp = 300;
    public int currentHp;

    public GachaSystem gachaSystem;

    void Awake()
    {
        currentHp = maxHp;
    }

    public void TakeDamage(int damage, bool isCrit)
    {
        currentHp -= damage;

        Debug.Log($"데미지: {damage} {(isCrit ? "크리티컬!" : "")}");

        if (currentHp <= 0)
        {
            OnEnemyDead();
        }
    }

    void OnEnemyDead()
    {
        Debug.Log("적 사망!");

        gachaSystem.DropItem();

        // 적 리스폰
        currentHp = maxHp;
    }
}
