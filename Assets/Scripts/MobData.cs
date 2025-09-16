using UnityEngine;

[CreateAssetMenu(fileName = "New MobData", menuName = "Mob Data", order = 51)]
public class MobData : ScriptableObject
{
    [SerializeField] private string mobName;
    [SerializeField] private int health;
    [SerializeField] private int weaponDamage;
    [SerializeField] private int strength;
    [SerializeField] private int dexterity;
    [SerializeField] private int endurance;
    [SerializeField] private Classes.WeaponType reward;
    [SerializeField] private Classes.MobType mobType;

    public string MobName => mobName;
    public int Health => health;
    public int WeaponDamage => weaponDamage;
    public int Strength => strength;
    public int Dexterity => dexterity;
    public int Endurance => endurance;
    public Classes.WeaponType Reward => reward;
    public Classes.MobType MobType => mobType;
}
