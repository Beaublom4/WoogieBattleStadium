using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilities : MonoBehaviour
{
    public string abilityName;
    public string abilityDiscription;

    public static void PerformAbility(Ability abil)
    {
        switch (abil)
        {
            case Ability.intimidate:

                

                break;
        }
    }
}
public enum Ability
{
    intimidate
}
