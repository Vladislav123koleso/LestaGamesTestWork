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
        Spear   // Копье
    }

    public enum MobType
    {
        Goblin,
        Skeleton,
        Slime
    }

    public struct ClassModifiers
    {
        public int lvl;
        public int hp;
        public int strength;
        public int stamina;
        public int agility;
        public WeaponType weapon;
    }

    private static readonly Dictionary<CharacterClass, ClassModifiers> classModifiers = new Dictionary<CharacterClass, ClassModifiers>
    {
        { CharacterClass.Rogue, new ClassModifiers { lvl = 1, hp = 4, strength = 1, agility = 1, stamina = 1, weapon = WeaponType.Dagger } },
        { CharacterClass.Warrior, new ClassModifiers { lvl = 1, hp = 5, strength = 1, agility = 1, stamina = 1, weapon = WeaponType.Sword } },
        { CharacterClass.Barbarian, new ClassModifiers { lvl = 1, hp = 6, strength = 1, agility = 1, stamina = 1, weapon = WeaponType.Club } },
    };

    public static ClassModifiers GetModifiers(CharacterClass characterClass)
    {
        return classModifiers[characterClass];
    }
}

