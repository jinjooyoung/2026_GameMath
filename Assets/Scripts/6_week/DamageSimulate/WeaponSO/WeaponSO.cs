using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "Scriptable Objects/WeaponSO")]
public class WeaponSO : ScriptableObject
{
    [Header("고유 ID")]
    public int id;

    [Header("이름")]
    public string nameKr;

    [Header("수치")]
    public float stdDevMult;
    public float critRate;
    public float critMult;
}
