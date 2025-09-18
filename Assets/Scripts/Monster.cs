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
        // ��������� ��� �������
        type = (MobType)Random.Range(0, System.Enum.GetValues(typeof(MobType)).Length);

        // �������������� ������� ������� �� ������ ������
        strength = Random.Range(playerLevel, playerLevel + 2);
        agility = Random.Range(playerLevel, playerLevel + 2);
        stamina = Random.Range(playerLevel, playerLevel + 2);
        maxHp = 5 + (stamina * 2);
        currentHp = maxHp;
        weapon = GetWeaponForMonsterType(playerLevel);
    }

    private Weapon GetWeaponForMonsterType(int playerLevel)
    {
        int minDmg = playerLevel;
        int maxDmg = playerLevel + 3;

        switch (type)
        {
            case MobType.Goblin:
                return new Weapon("������ ������", WeaponType.Dagger, minDmg, maxDmg);
            case MobType.Skeleton:
                return new Weapon("������ ���", WeaponType.Sword, minDmg + 1, maxDmg);
            case MobType.Slime:
                return new Weapon("�������� �����", WeaponType.Spear, minDmg, maxDmg + 1);
            default:
                return new Weapon("����������� ������", WeaponType.Club, 1, 2);
        }
    }
}