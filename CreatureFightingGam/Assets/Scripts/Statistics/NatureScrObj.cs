using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Woogies/Nature")]
[System.Serializable]
public class NatureScrObj : ScriptableObject
{
    public string natureName;
    //stats
    public float attack = 1;
    public float defence = 1;
    public float sp_att = 1;
    public float sp_def = 1;
    public float speed = 1;
}
