using System.Collections.Generic;
using UnityEngine;

public class WeaponDB : MonoBehaviour
{
    [Header("무기 SO 리스트")]
    public List<WeaponSO> weapons = new List<WeaponSO>();

    private Dictionary<int, WeaponSO> weaponById;

    public void Initialize()
    {
        weaponById = new Dictionary<int, WeaponSO>();

        foreach (var weapon in weapons)
        {
            weaponById[weapon.id] = weapon;
        }
    }

    public WeaponSO GetWeaponById(int id)
    {
        if (weaponById == null)
        {
            Initialize();
        }

        if (weaponById.TryGetValue(id, out WeaponSO weapon))
            return weapon;

        return null;
    }
}
