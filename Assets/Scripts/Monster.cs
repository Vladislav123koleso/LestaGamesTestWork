using UnityEngine;
using static Classes;

public class Monster
{
    public MobType type;
    public int maxHp;
    public int currentHp;
    public int strength;
    public int agility;
    public int stamina;
    public Weapon weapon;

    public Monster(int playerLevel)
    {
        // Случайный тип монстра
        type = (MobType)Random.Range(0, System.Enum.GetValues(typeof(MobType)).Length);

        // Характеристики монстра зависят от уровня игрока
        strength = Random.Range(playerLevel, playerLevel + 2);
        agility = Random.Range(playerLevel, playerLevel + 2);
        stamina = Random.Range(playerLevel, playerLevel + 2);
        maxHp = 5 + (stamina * 2);
        currentHp = maxHp;
        weapon = GetRandomWeapon();
    }

    private Weapon GetRandomWeapon()
    {
        WeaponType randomType = (WeaponType)Random.Range(0, System.Enum.GetValues(typeof(WeaponType)).Length);
        int minDamage = Random.Range(1, 4);
        int maxDamage = Random.Range(minDamage, minDamage + 3);
        return new Weapon($"{randomType} монстра", randomType, minDamage, maxDamage);
    }
}   