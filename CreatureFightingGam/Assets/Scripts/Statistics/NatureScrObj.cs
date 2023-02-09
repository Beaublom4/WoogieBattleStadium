using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Woogies/Nature")]
[System.Serializable]
public class NatureScrObj : ScriptableObject
{
    public string natureName;
    //stats
    public int health;
    public int attack;
    public int defence;
    public int sp_att;
    public int sp_def;
    public int speed;
}
