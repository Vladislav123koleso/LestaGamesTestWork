using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Classes
{
    public enum CharacterClass
    {
        Warrior,
        Barbarian,
        Rogue
    }
    public enum WeaponType
    {
        Sword,  // Рубящее
        Club,   // Дробящее
        Dagger, // Колющее
        Spear,  // Копье
        Axe     // Рубящее
    }

    public enum MobType
    {
        Goblin,
        Skeleton,
        Slime,
        Ghost,
        Golem,
        Dragon
    }

    public struct ClassModifiers
    {
        public int lvl;
        public int hp;
        public int strength;
        public int agility;
        public int stamina;
        public WeaponType weapon;
    }

    private static readonly Dictionary<CharacterClass, ClassModifiers> classModifiers = new Dictionary<CharacterClass, ClassModifiers>
    {
        { CharacterClass.Rogue, new ClassModifiers { lvl = 1, hp = 4, strength = 0, agility = 0, stamina = 0, weapon = WeaponType.Dagger } },
        { CharacterClass.Warrior, new ClassModifiers { lvl = 1, hp = 5, strength = 0, agility = 0, stamina = 0, weapon = WeaponType.Sword } },
        { CharacterClass.Barbarian, new ClassModifiers { lvl = 1, hp = 6, strength = 0, agility = 0, stamina = 0, weapon = WeaponType.Club } },
    };

    public static ClassModifiers GetModifiers(CharacterClass characterClass)
    {
        return classModifiers[characterClass];
    }
    
}

