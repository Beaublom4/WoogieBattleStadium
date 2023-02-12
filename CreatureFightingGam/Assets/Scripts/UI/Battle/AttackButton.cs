using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AttackButton : MonoBehaviour
{
    public TMP_Text attackName;
    public TMP_Text attackUses;
    public TMP_Text attackDamage;
    public GameObject type;

    public AttackScrObj attackScrObj;
    public int attackNumber;

    public void SetUp(AttackScrObj _attackScrObj, int attacksLeft)
    {
        if (_attackScrObj != null)
        {
            attackScrObj = _attackScrObj;
            attackName.text = _attackScrObj.attackName;
            attackUses.text = attacksLeft + "/" + _attackScrObj.uses;
            attackDamage.text = "Dmg: " + _attackScrObj.damage;
            type.GetComponent<Image>().color = _attackScrObj.type.typeColor;
            type.GetComponentInChildren<TMP_Text>().text = _attackScrObj.type.typeName;
        }
        else
        {
            attackName.text = "Empty";
            attackUses.text = "";
            attackDamage.text = "";
            type.SetActive(false);
        }
    }
    public void SetUsesLeft(int uses)
    {
        string[] split = attackUses.text.Split("/");
        attackUses.text = uses + "/" + split[1];
    }
    public void Attack()
    {
        GameManager.Instance.Attack(attackScrObj, attackNumber);
    }
}
