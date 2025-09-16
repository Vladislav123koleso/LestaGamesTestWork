using static Classes;

public struct Weapon
{
    public string name;
    public WeaponType type;
    public int minDamage;
    public int maxDamage;

    public Weapon(string name, WeaponType type, int minDamage, int maxDamage)
    {
        this.name = name;
        this.type = type;
        this.minDamage = minDamage;
        this.maxDamage = maxDamage;
    }

    public int GetDamage()
    {
        return UnityEngine.Random.Range(minDamage, maxDamage + 1);
    }
}