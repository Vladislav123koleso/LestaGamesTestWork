using UnityEngine;
using static Classes;

public class Character
{
    public CharacterClass characterClass;
    public int level;
    public int maxHp;
    public int currentHp;
    public int strength;
    public int agility;
    public int stamina;
    public Weapon weapon;
    public int monstersDefeated;

    public Character(CharacterClass characterClass)
    {
        this.characterClass = characterClass;
        this.level = 1;
        
        strength = Random.Range(1, 4);
        agility = Random.Range(1, 4);
        stamina = Random.Range(1, 4);

        ClassModifiers modifiers = GetModifiers(characterClass);
        maxHp = modifiers.hp + stamina;
        currentHp = maxHp;
        
        // ��������� ������ � ����������� �� ������
        weapon = GetInitialWeapon(modifiers.weapon);

        monstersDefeated = 0;
    }

    private Weapon GetInitialWeapon(WeaponType weaponType)
    {
        switch (weaponType)
        {
            case WeaponType.Dagger:
                return new Weapon("������", WeaponType.Dagger, 1, 4);
            case WeaponType.Sword:
                return new Weapon("���", WeaponType.Sword, 2, 5);
            case WeaponType.Club:
                return new Weapon("������", WeaponType.Club, 3, 6);
            default:
                return new Weapon("������", WeaponType.Club, 1, 2);
        }
    }

    public void LevelUp()
    {
        level++;
        ClassModifiers modifiers = GetModifiers(characterClass);
        maxHp += modifiers.hp + stamina;
        currentHp = maxHp;

        // ���������� ������� �� �������
        if (level == 2)
        {
            if (characterClass == CharacterClass.Rogue) agility++;
            if (characterClass == CharacterClass.Warrior) { /* ������ ���� � ��� */ }
            if (characterClass == CharacterClass.Barbarian) { /* ������ �������� ���� � ��� */ }
        }
        else if (level == 3)
        {
            if (characterClass == CharacterClass.Rogue) { /* ������ ��� � ��� */ }
            if (characterClass == CharacterClass.Warrior) strength++;
            if (characterClass == CharacterClass.Barbarian) stamina++;
        }
    }
}