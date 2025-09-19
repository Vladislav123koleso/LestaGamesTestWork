using System.Collections.Generic;

public static class WeaponDatabase
{
    public enum WeaponID
    {
        // Стартовое оружие
        PlayerSword,
        PlayerClub,
        PlayerDagger,

        // Оружие монстров
        GoblinDagger,
        SkeletonAxe,
        SlimeSpear,
        EctoplasmicBlade,
        GolemFist,
        DragonClaw,

        // Легендарное
        LegendarySword
    }

    private static readonly Dictionary<WeaponID, Weapon> weapons = new Dictionary<WeaponID, Weapon>
    {
        // --- Оружие с фиксированным уроном ---
        { WeaponID.PlayerSword, new Weapon("Меч", Classes.WeaponType.Sword, 3, 3) },
        { WeaponID.PlayerClub, new Weapon("Дубина", Classes.WeaponType.Club, 3, 3) },
        { WeaponID.PlayerDagger, new Weapon("Кинжал", Classes.WeaponType.Dagger, 2, 2) },
        { WeaponID.SkeletonAxe, new Weapon("Топор", Classes.WeaponType.Axe, 4, 4) },
        { WeaponID.SlimeSpear, new Weapon("Копье", Classes.WeaponType.Spear, 3, 3) },
        { WeaponID.LegendarySword, new Weapon("Легендарный меч", Classes.WeaponType.Sword, 10, 10) },
        
        // --- Оружие монстров ---
        { WeaponID.GoblinDagger, new Weapon("Ржавый кинжал", Classes.WeaponType.Dagger, 2, 2) },
        { WeaponID.EctoplasmicBlade, new Weapon("Эктоплазменный клинок", Classes.WeaponType.Sword, 3, 3) },
        { WeaponID.GolemFist, new Weapon("Каменный кулак", Classes.WeaponType.Club, 5, 5) },
        { WeaponID.DragonClaw, new Weapon("Коготь дракона", Classes.WeaponType.Dagger, 6, 6) },
    };

    public static Weapon GetWeapon(WeaponID id)
    {
        return weapons[id];
    }
}