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
    [Tooltip("-1 = never, 0 = default(4069), 1 = always, > 1 = chance")]
    public int shinyChance = 0;
    [Space]
    public int packCoinCost;
    public int packMoneyCost;

    public WoogieRarityDrops[] drops;
    public AttackScrObj[] attacks;
    //Items in packs
}
[System.Serializable]
public struct WoogieRarityDrops
{
    public CardRarity rarity;
    public float dropChance;
    public WoogieScrObj[] woogieScrObjs;
}
