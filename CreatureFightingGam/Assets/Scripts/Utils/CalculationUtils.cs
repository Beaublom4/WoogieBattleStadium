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
    /// <summary>
    /// 
    /// </summary>
    /// <param name="level"></param>
    /// <param name="power"></param>
    /// <param name="attack"></param>
    /// <param name="defence"></param>
    /// <param name="stab"> 1 = not same woogie typing as attack type, 1.5 = same woogie typing as attack type </param>
    /// <param name="type"> 0 = not effective, .5 = not very effective, 1 = normal effective, 2 = 1 time super effective, 4 = 2 times super effecitve </param> 
    /// <param name="burn "> .5 = if attacker is burned, 1 = if attacker isnt burned </param>
    /// <param name="other"></param>
    /// <returns></returns>
    public static DamageCalculaton DamageCalcualtion(int level, int power, int attack, int defence, float stab, float type, float burn, float other)
    {
        DamageCalculaton dmgCalc = new();

        float dmg = 2 * level;
        dmg /= 5;
        dmg += 2;
        dmg *= power;
        int AD = attack / defence;
        dmg *= AD;
        dmg /= 50;
        dmg += 2;
        float crit = 1;
        if (Random.Range(0f, 100f) <= 6.25f)
        {
            dmgCalc.crit = true;
            crit = 1.5f;
        }
        dmg *= crit;
        float random = Random.Range(.85f, 1f);
        dmgCalc.roll = random;
        dmg *= random;
        dmg *= stab;
        dmg *= type;
        if (type >= 1 && type < 2)
            dmgCalc.effectiveness = DamageCalculaton.Effectiveness.defaultEffective;
        else if (type >= 2)
            dmgCalc.effectiveness = DamageCalculaton.Effectiveness.superEffective;
        else if (type == 0)
            dmgCalc.effectiveness = DamageCalculaton.Effectiveness.notVeryEffective;
        else if (type < 1)
            dmgCalc.effectiveness = DamageCalculaton.Effectiveness.notEffective;
        dmg *= burn;
        dmg *= other;

        dmgCalc.damage = Mathf.FloorToInt(dmg);
        return dmgCalc;
    }
    public struct DamageCalculaton
    {
        public int damage;
        public bool crit;
        public Effectiveness effectiveness;
        public float roll;

        public enum Effectiveness
        {
            notEffective,
            notVeryEffective,
            defaultEffective,
            superEffective
        }
    }
    public static float Stab(WoogieScrObj woogieScrObj, AttackScrObj attackScrObj)
    {
        float stab = 1;
        foreach (TypingScrObj type in woogieScrObj.typing)
            if (type == attackScrObj.type)
                stab = 1.5f;

        return stab;
    }
    public static float Type(TypingScrObj attackType, TypingScrObj[] otherTypes)
    {
        float type = 1;
        foreach(TypingScrObj otherType in otherTypes)
        {
            bool set = false;
            bool setNotEffective = false;
            foreach(TypingScrObj superEffective in attackType.superEffective)
            {
                if(superEffective == otherType)
                {
                    Debug.Log("Its super effective");
                    type *= 2;
                    set = true;
                    break;
                }
            }
            if (set) continue;
            foreach(TypingScrObj notVeryEffective in attackType.notVeryEffective)
            {
                if(notVeryEffective == otherType)
                {
                    type /= 2f;
                    set = true;
                    break;
                }
            }
            if (set) continue;
            foreach(TypingScrObj notEffective in attackType.notEffective)
            {
                if(notEffective == otherType)
                {
                    type = 0;
                    setNotEffective = true;
                    break;
                }
            }
            if (setNotEffective) break;
        }
        Debug.Log("Type: " + type);

        return type;
    }
}
