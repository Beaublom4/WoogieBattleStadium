using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WoogieStats : MonoBehaviour
{
    public TMP_Text woogieNameText;
    public TMP_Text woogieLevelText;
    public Slider woogieHealthSlider;
    public TMP_Text woogieHealthText;
    public Slider woogieXpSlider;

    public float health;

    public void SetUp(WoogieSave woogieSave)
    {
        WoogieScrObj woogieScrObj = Resources.Load<WoogieScrObj>("Woogies/" + woogieSave.woogieScrObjName);

        woogieNameText.text = woogieScrObj.woogieName;
        woogieLevelText.text = "Lv: " + woogieSave.currentLevel.ToString();
        int maxHealth = CalculationUtils.HpCalculation(woogieScrObj.baseStats.hp, woogieSave.individualStats.hp, woogieSave.EffortStats.hp, woogieSave.currentLevel);
        health = maxHealth;
        woogieHealthSlider.maxValue = maxHealth;
        woogieHealthSlider.value = maxHealth;
        woogieHealthText.text = maxHealth + "/" + maxHealth;
        woogieXpSlider.maxValue = woogieScrObj.levelCurve.levelCurve.Evaluate(woogieSave.currentLevel);
        woogieXpSlider.value = woogieSave.currentXp;
    }
    public void SetHealth(float _health)
    {
        health = _health;
        woogieHealthSlider.value = health;
        woogieHealthText.text = Mathf.CeilToInt(health).ToString() + "/" + woogieHealthSlider.maxValue;
    }
}
