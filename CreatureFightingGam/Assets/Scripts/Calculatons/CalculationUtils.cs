using UnityEngine;

public static class CalculationUtils
{
    /// <summary>
    /// Returns a calculated hp stat
    /// </summary>
    /// <param name="baseHp"></param>
    /// <param name="ivHp"></param>
    /// <param name="evHp"></param>
    /// <param name="level"></param>
    /// <returns></returns>
    public static int HpCalculation(int baseHp, int ivHp, int evHp, int level)
    {
        int hp = 2 * baseHp;
        hp += ivHp;
        hp += evHp / 4;
        hp *= level;
        hp /= 170;
        hp += level += 17;
        return hp;
    }
    /// <summary>
    /// Returns a calculated stat
    /// </summary>
    /// <param name="baseStat"></param>
    /// <param name="ivStat"></param>
    /// <param name="evStat"></param>
    /// <param name="level"></param>
    /// <param name="nature = .9f or 1.1f"></param>
    /// <returns></returns>
    public static int StatCaclulation(int baseStat, int ivStat, int evStat, int level, float nature)
    {
        float stat = 2 * baseStat;
        stat += ivStat;
        stat += evStat / 4;
        stat *= level;
        stat /= 170;
        stat += 5;
        stat *= nature;
        return Mathf.FloorToInt(stat);
    }
}
