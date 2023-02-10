using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Woogies/Attack")]
[System.Serializable]
public class AttackScrObj : ScriptableObject
{
    public string attackName;
    public string attackEffect;
    public GameObject vfx;
    public TypingScrObj type;
    public int uses;
    public int damage;
    public int accuracy;
    public AttackCategory category;
    [Header("Effects")]
    public bool priority;
    public bool frozen;
    public bool flamed;
    public bool corrupt;
    public bool block;
    public bool stun;
    public bool sleep;
}
