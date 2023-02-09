using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Woogies/Woogie")]
public class WoogieScrObj : ScriptableObject
{
    public int number;
    public string woogieName;
    public Sprite icon;
    public TypingScrObj[] typing;
    public NatureScrObj[] natures;
    public Ability ability;
    public Stats stats;
    public LevelCurves levelCurve;
    public int evolveLevel;
    public AttackUnlock[] attackUnlocks;
}
[System.Serializable]
public class WoogieSave
{
    public WoogieSave(int _number, string _name, Sprite _icon, TypingScrObj[] _typing, NatureScrObj[] _natures, Ability _ability, Stats _stats, LevelCurves _curve, int _evolveLevel, AttackUnlock[] _attackUnlocks)
    {
        number = _number;
        woogieName = _name;
        icon = _icon;
        typing = _typing;
        natures = _natures;
        ability = _ability;
        stats = _stats;
        levelCurve = _curve;
        evolveLevel = _evolveLevel;
        attackUnlocks = _attackUnlocks;
    }

    public int number;
    public string woogieName;
    public Sprite icon;
    public TypingScrObj[] typing;
    public NatureScrObj[] natures;
    public Ability ability;
    public Stats stats;
    public LevelCurves levelCurve;
    public int evolveLevel;
    public AttackUnlock[] attackUnlocks;
}
