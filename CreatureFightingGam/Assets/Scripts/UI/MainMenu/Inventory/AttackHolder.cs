using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AttackHolder : MonoBehaviour
{
    public TMP_Text attackName;
    public TMP_Text attackUses;
    public TMP_Text attackDamage;
    public GameObject type;
    public GameObject catagory;

    private AttackScrObj attackScrObj;

    public void SetUp(AttackScrObj _attackScrObj)
    {
        if (_attackScrObj != null)
        {
            attackScrObj = _attackScrObj;
            attackName.text = attackScrObj.attackName;
            attackUses.text = "Uses: " + attackScrObj.uses;
            attackDamage.text = "Dmg: " + attackScrObj.damage;
            type.GetComponent<Image>().color = attackScrObj.type.typeColor;
            type.GetComponentInChildren<TMP_Text>().text = attackScrObj.type.typeName;
        }
        else
        {
            attackName.text = "Empty";
            attackUses.text = "";
            attackDamage.text = "";
            type.SetActive(false);
        }
    }
}
