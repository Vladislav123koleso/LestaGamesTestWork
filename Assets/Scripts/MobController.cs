using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobController : MonoBehaviour
{
    public MobData mobData;
    private int currentHealth;

    void Start()
    {
        if (mobData != null)
        {
            currentHealth = mobData.Health;
        }
    }

    public void TakeDamage(int baseDamage, int weaponDamage, Classes.WeaponType weaponType)
    {
        int totalDamage = baseDamage;
        int modifiedWeaponDamage = weaponDamage;

        if (mobData.MobType == Classes.MobType.Skeleton && weaponType == Classes.WeaponType.Club)
        {
            modifiedWeaponDamage *= 2;
        }
        else if (mobData.MobType == Classes.MobType.Slime && weaponType == Classes.WeaponType.Sword)
        {
            modifiedWeaponDamage = 0;
        }

        totalDamage += modifiedWeaponDamage;

        currentHealth -= totalDamage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Логика смерти моба, например, проигрывание анимации, выпадение лута и т.д.
        Debug.Log(mobData.MobName + " has been defeated!");
        Destroy(gameObject);
    }
}
