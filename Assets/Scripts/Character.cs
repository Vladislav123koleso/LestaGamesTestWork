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
        
        // Начальное оружие в зависимости от класса
        weapon = GetInitialWeapon(modifiers.weapon);

        monstersDefeated = 0;
    }

    private Weapon GetInitialWeapon(WeaponType weaponType)
    {
        switch (weaponType)
        {
            case WeaponType.Dagger:
                return new Weapon("Кинжал", WeaponType.Dagger, 1, 4);
            case WeaponType.Sword:
                return new Weapon("Меч", WeaponType.Sword, 2, 5);
            case WeaponType.Club:
                return new Weapon("Дубина", WeaponType.Club, 3, 6);
            default:
                return new Weapon("Кулаки", WeaponType.Club, 1, 2);
        }
    }

    public void LevelUp()
    {
        level++;
        ClassModifiers modifiers = GetModifiers(characterClass);
        maxHp += modifiers.hp + stamina;
        currentHp = maxHp;

        // Применение бонусов за уровень
        if (level == 2)
        {
            if (characterClass == CharacterClass.Rogue) agility++;
            if (characterClass == CharacterClass.Warrior) { /* Логика щита в бою */ }
            if (characterClass == CharacterClass.Barbarian) { /* Логика каменной кожи в бою */ }
        }
        else if (level == 3)
        {
            if (characterClass == CharacterClass.Rogue) { /* Логика яда в бою */ }
            if (characterClass == CharacterClass.Warrior) strength++;
            if (characterClass == CharacterClass.Barbarian) stamina++;
        }
    }
}