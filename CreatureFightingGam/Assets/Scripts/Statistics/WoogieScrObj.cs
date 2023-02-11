using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Woogies/Woogie")]
public class WoogieScrObj : ScriptableObject
{
    public string woogieName;
    public Sprite icon;
    public TypingScrObj[] typing;
    public NatureScrObj[] possibleNatures;
    public Ability[] possibleAbilities;
    public Stats baseStats;
    public LevelCurves levelCurve;
    public int evolveLevel;
    public WoogieScrObj evolution;
    public AttackUnlock[] attackUnlocks;
}
[System.Serializable]
public class WoogieSave
{
    public string secretId;
    public string woogieScrObjName;
    public Stats individualStats;
    public Stats EffortStats;
    public string natureScrObjName;
    public Ability ability;
    public string[] selectedAttacksScrObjNames;
    public int currentLevel;
    public int currentXp;
    public bool shiny;
}
