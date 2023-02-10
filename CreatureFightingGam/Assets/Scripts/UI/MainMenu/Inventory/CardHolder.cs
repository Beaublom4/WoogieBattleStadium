using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardHolder : MonoBehaviour
{
    public Image iconImage;
    public TMP_Text nameText;
    public TMP_Text statsText;
    [Space]
    public GameObject typingPrefab;
    public Transform typingHolder;

    public void SetUp(WoogieSave woogieSave, WoogieScrObj woogieScrObj)
    {
        iconImage.sprite = woogieScrObj.icon;
        nameText.text = woogieScrObj.woogieName;
        statsText.text = "Stats";
        statsText.text += "<br>";
        statsText.text += "<br>Health: " + CalculationUtils.HpCalculation(woogieScrObj.baseStats.hp, woogieSave.individualStats.hp, woogieSave.EffortStats.hp, woogieSave.currentLevel);
        NatureScrObj nature = Resources.Load<NatureScrObj>("Natures/" + woogieSave.natureScrObjName);
        statsText.text += "<br>Attack: " + CalculationUtils.StatCaclulation(woogieScrObj.baseStats.att, woogieSave.individualStats.att, woogieSave.EffortStats.att, woogieSave.currentLevel, nature.attack);
        statsText.text += "<br>Defence: " + CalculationUtils.StatCaclulation(woogieScrObj.baseStats.def, woogieSave.individualStats.att, woogieSave.EffortStats.att, woogieSave.currentLevel, nature.defence);
        statsText.text += "<br>Sp. Att: " + CalculationUtils.StatCaclulation(woogieScrObj.baseStats.s_att, woogieSave.individualStats.att, woogieSave.EffortStats.att, woogieSave.currentLevel, nature.sp_att);
        statsText.text += "<br>Sp. Def: " + CalculationUtils.StatCaclulation(woogieScrObj.baseStats.s_def, woogieSave.individualStats.att, woogieSave.EffortStats.att, woogieSave.currentLevel, nature.sp_def);
        statsText.text += "<br>Speed: " + CalculationUtils.StatCaclulation(woogieScrObj.baseStats.spd, woogieSave.individualStats.att, woogieSave.EffortStats.att, woogieSave.currentLevel, nature.speed);

        foreach (TypingScrObj type in woogieScrObj.typing)
        {
            GameObject newType = Instantiate(typingPrefab, typingHolder);
            newType.GetComponentInChildren<TMP_Text>().text = type.typeName;
            newType.GetComponent<Image>().color = type.typeColor;
        }
    }
    public void SelectCard()
    {

    }
}
