using System.Collections.Generic;

public static class WeaponDatabase
{
    public enum WeaponID
    {
        // ��������� ������
        PlayerSword,
        PlayerClub,
        PlayerDagger,

        // ������ ��������
        GoblinDagger,
        SkeletonAxe,
        SlimeSpear,
        EctoplasmicBlade,
        GolemFist,
        DragonClaw,

        // �����������
        LegendarySword
    }

    private static readonly Dictionary<WeaponID, Weapon> weapons = new Dictionary<WeaponID, Weapon>
    {
        // --- ������ � ������������� ������ ---
        { WeaponID.PlayerSword, new Weapon("���", Classes.WeaponType.Sword, 3, 3) },
        { WeaponID.PlayerClub, new Weapon("������", Classes.WeaponType.Club, 3, 3) },
        { WeaponID.PlayerDagger, new Weapon("������", Classes.WeaponType.Dagger, 2, 2) },
        { WeaponID.SkeletonAxe, new Weapon("�����", Classes.WeaponType.Axe, 4, 4) },
        { WeaponID.SlimeSpear, new Weapon("�����", Classes.WeaponType.Spear, 3, 3) },
        { WeaponID.LegendarySword, new Weapon("����������� ���", Classes.WeaponType.Sword, 10, 10) },
        
        // --- ������ �������� ---
        { WeaponID.GoblinDagger, new Weapon("������ ������", Classes.WeaponType.Dagger, 2, 2) },
        { WeaponID.EctoplasmicBlade, new Weapon("�������������� ������", Classes.WeaponType.Sword, 3, 3) },
        { WeaponID.GolemFist, new Weapon("�������� �����", Classes.WeaponType.Club, 5, 5) },
        { WeaponID.DragonClaw, new Weapon("������ �������", Classes.WeaponType.Dagger, 6, 6) },
    };

    public static Weapon GetWeapon(WeaponID id)
    {
        return weapons[id];
    }
}