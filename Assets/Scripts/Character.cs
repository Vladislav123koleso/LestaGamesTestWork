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

    // Ѕазовые значени€, полученные при создании персонажа
    private int baseStrength;
    private int baseAgility;
    private int baseStamina;

    public Character(CharacterClass startingClass)
    {
        classLevels = new Dictionary<CharacterClass, int>
        {
            { startingClass, 1 }
        };
        
        baseStrength = Random.Range(1, 4);
        baseAgility = Random.Range(1, 4);
        baseStamina = Random.Range(1, 4);

        // Ќачальное оружие зависит от стартового класса
        weapon = GetInitialWeapon(startingClass);

        monstersDefeated = 0;
        RecalculateStats();
    }

    private Weapon GetInitialWeapon(CharacterClass startingClass)
    {
        switch (startingClass)
        {
            case CharacterClass.Warrior:
                return WeaponDatabase.GetWeapon(WeaponDatabase.WeaponID.PlayerSword);
            case CharacterClass.Barbarian:
                return WeaponDatabase.GetWeapon(WeaponDatabase.WeaponID.PlayerClub);
            case CharacterClass.Rogue:
                return WeaponDatabase.GetWeapon(WeaponDatabase.WeaponID.PlayerDagger);
            default:
                return new Weapon(" улаки", WeaponType.Club, 1, 1);
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
        int bonusStrength = 0;
        int bonusAgility = 0;
        int bonusStamina = 0;
        int baseHpFromClasses = 0;

        foreach (var classLevelPair in classLevels)
        {
            CharacterClass charClass = classLevelPair.Key;
            int level = classLevelPair.Value;
            ClassModifiers modifiers = GetModifiers(charClass);
            
            baseHpFromClasses += modifiers.hp * level;

            // ѕримен€ем бонусы за каждый уровень в классе
            if (charClass == CharacterClass.Rogue && level >= 2) bonusAgility++;
            if (charClass == CharacterClass.Warrior && level >= 3) bonusStrength++;
            if (charClass == CharacterClass.Barbarian && level >= 3) bonusStamina++;
        }

        // ќбновл€ем итоговые характеристики
        strength = baseStrength + bonusStrength;
        agility = baseAgility + bonusAgility;
        stamina = baseStamina + bonusStamina;

        // «доровье = (базовое от классов) + (выносливость * общий уровень персонажа)
        maxHp = baseHpFromClasses + (stamina * TotalLevel);
        currentHp = maxHp;
    }

    public string GetClassLevelSummary()
    {
        StringBuilder summary = new StringBuilder();
        foreach (var pair in classLevels)
        {
            summary.Append($"{pair.Key} (”р. {pair.Value}) ");
        }
        return summary.ToString().Trim();
    }
}