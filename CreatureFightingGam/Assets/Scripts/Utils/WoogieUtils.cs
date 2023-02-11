using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WoogieUtils
{
    public const int defaultShinyChance = 4069;
    private const string glyphs = "abcdefghijklmnopqrstovwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ123456789";

    /// <summary>
    /// Creating a new randomized woogie from <paramref name="woogieScrObj"/>
    /// </summary>
    /// <param name="woogieScrObj"></param>
    /// <param name="minLevel"></param>
    /// <param name="maxLevel"></param>
    /// <param name="-1 = never, 0 = default(4069), 1 = always, > 1 = chance"></param>
    /// <returns></returns>
    public static WoogieSave CreateNewWoogie(WoogieScrObj woogieScrObj, int minLevel, int maxLevel, float shinyChance)
    {
        WoogieSave woogie = new();
        //Getting a secret ID
        string newId = "";
        for (int s = 0; s < 9; s++)
        {
            newId += glyphs[Random.Range(0, glyphs.Length)];
        }
        woogie.secretId = newId;
        woogie.woogieScrObjName = woogieScrObj.name;
        woogie.individualStats = RandomStats(0, 31);
        woogie.natureScrObjName = woogieScrObj.possibleNatures[Random.Range(0, woogieScrObj.possibleNatures.Length)].name;
        woogie.ability = woogieScrObj.possibleAbilities[Random.Range(0, woogieScrObj.possibleAbilities.Length)];
        //Getting the base attacks
        string[] currentAttacksScrObjs = new string[4];
        for (int a = 0; a < woogieScrObj.attackUnlocks.Length && a < 4; a++)
        {
            if (woogieScrObj.attackUnlocks[a].levelUnlocked == 0)
            {
                currentAttacksScrObjs[a] = woogieScrObj.attackUnlocks[a].possibleAttack[Random.Range(0, woogieScrObj.attackUnlocks[a].possibleAttack.Length)].name;
            }
            else
                break;
        }
        woogie.selectedAttacksScrObjNames = currentAttacksScrObjs;
        woogie.currentLevel = Random.Range(minLevel, maxLevel + 1);
        if (shinyChance == 1)
            woogie.shiny = true;
        else if (shinyChance == -1)
            woogie.shiny = false;
        else
        {
            if (shinyChance == 0) shinyChance = defaultShinyChance;
            woogie.shiny = Random.Range(0, shinyChance) == 0;
        }

        return woogie;
    }
    public static Stats RandomStats(int minStat = 0, int maxStat = 0)
    {
        Stats randomStats = new();
        maxStat++;
        randomStats.hp = Random.Range(minStat, maxStat);
        randomStats.att = Random.Range(minStat, maxStat);
        randomStats.def = Random.Range(minStat, maxStat);
        randomStats.s_att = Random.Range(minStat, maxStat);
        randomStats.s_def = Random.Range(minStat, maxStat);
        randomStats.spd = Random.Range(minStat, maxStat);

        return randomStats;
    }
}
