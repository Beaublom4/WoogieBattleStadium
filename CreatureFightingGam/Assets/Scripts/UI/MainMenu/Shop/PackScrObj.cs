using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Woogies/Pack")]
public class PackScrObj : ScriptableObject
{
    public string packName;
    public string packType;
    public int itemsInPack;
    public int minLevel, maxLevel;
    public int shinyChance = 4069;
    [Space]
    public int packCoinCost;
    public int packMoneyCost;

    public WoogieScrObj[] woogies;
    public AttackScrObj[] attacks;
    //Items in packs
}
