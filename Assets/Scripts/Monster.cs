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
    public int damage; 
    public Weapon droppedWeapon; // Оружие, которое выпадает после смерти

    public Monster(int playerLevel)
    {
        // Случайный тип монстра
        type = (MobType)Random.Range(0, System.Enum.GetValues(typeof(MobType)).Length);

        switch (type)
        {
            case MobType.Goblin:
                strength = 1;
                agility = 1;
                stamina = 1;
                maxHp = 5;
                damage = 2;
                droppedWeapon = WeaponDatabase.GetWeapon(WeaponDatabase.WeaponID.GoblinDagger);
                break;

            case MobType.Skeleton:
                strength = 2;
                agility = 2;
                stamina = 1;
                maxHp = 10;
                damage = 2;
                droppedWeapon = WeaponDatabase.GetWeapon(WeaponDatabase.WeaponID.SkeletonAxe);
                break;

            case MobType.Slime:
                strength = 3;
                agility = 1;
                stamina = 2;
                maxHp = 8;
                damage = 1;
                droppedWeapon = WeaponDatabase.GetWeapon(WeaponDatabase.WeaponID.SlimeSpear);
                break;
            
            case MobType.Ghost:
                strength = 1;
                agility = 3;
                stamina = 1;
                maxHp = 6;
                damage = 3;
                droppedWeapon = WeaponDatabase.GetWeapon(WeaponDatabase.WeaponID.EctoplasmicBlade);
                break;

            case MobType.Golem:
                strength = 3;
                agility = 1;
                stamina = 3;
                maxHp = 10;
                damage = 1;
                droppedWeapon = WeaponDatabase.GetWeapon(WeaponDatabase.WeaponID.GolemFist);
                break;

            case MobType.Dragon:
                strength = 3;
                agility = 3;
                stamina = 3;
                maxHp = 20;
                damage = 4;
                droppedWeapon = WeaponDatabase.GetWeapon(WeaponDatabase.WeaponID.DragonClaw);
                break;

            default:
                strength = 1;
                agility = 1;
                stamina = 1;
                maxHp = 5;
                damage = 1;
                droppedWeapon = new Weapon("Странный предмет", WeaponType.Club, 1, 1);
                break;
        }
        
        currentHp = maxHp;
    }
}