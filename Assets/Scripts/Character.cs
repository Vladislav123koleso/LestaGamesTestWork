using UnityEngine;
using static Classes;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Character
{
    public Dictionary<CharacterClass, int> classLevels;
    public int TotalLevel => classLevels.Values.Sum();

    public int maxHp;
    public int currentHp;
    public int strength;
    public int agility;
    public int stamina;
    public Weapon weapon;
    public int monstersDefeated;

    public Character(CharacterClass startingClass)
    {
        classLevels = new Dictionary<CharacterClass, int>
        {
            { startingClass, 1 }
        };
        
        strength = Random.Range(1, 4);
        agility = Random.Range(1, 4);
        stamina = Random.Range(1, 4);

        // Начальное оружие зависит от стартового класса
        weapon = GetInitialWeapon(GetModifiers(startingClass).weapon);

        monstersDefeated = 0;
        RecalculateStats();
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

    public void ApplyLevelUp(CharacterClass classToLevel)
    {
        if (classLevels.ContainsKey(classToLevel))
        {
            classLevels[classToLevel]++;
        }
        else
        {
            classLevels.Add(classToLevel, 1);
        }
        RecalculateStats();
    }

    public void RecalculateStats()
    {
        // Сброс бонусов перед пересчетом
        int bonusStrength = 0;
        int bonusAgility = 0;
        int bonusStamina = 0;
        int baseHp = 0;

        foreach (var classLevelPair in classLevels)
        {
            CharacterClass charClass = classLevelPair.Key;
            int level = classLevelPair.Value;
            ClassModifiers modifiers = GetModifiers(charClass);
            
            baseHp += modifiers.hp * level;

            // Применяем бонусы за каждый уровень в классе
            for (int i = 2; i <= level; i++)
            {
                if (i == 2)
                {
                    if (charClass == CharacterClass.Rogue) bonusAgility++;
                }
                else if (i == 3)
                {
                    if (charClass == CharacterClass.Warrior) bonusStrength++;
                    if (charClass == CharacterClass.Barbarian) bonusStamina++;
                }
            }
        }

        // Обновляем итоговые характеристики
        maxHp = baseHp + stamina + (bonusStamina * TotalLevel); // Выносливость дает HP за каждый общий уровень
        currentHp = maxHp;
    }

    public string GetClassLevelSummary()
    {
        StringBuilder summary = new StringBuilder();
        foreach (var pair in classLevels)
        {
            summary.Append($"{pair.Key} (Ур. {pair.Value}) ");
        }
        return summary.ToString().Trim();
    }
}