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

    public void SetUp(WoogieScrObj woogie)
    {
        iconImage.sprite = woogie.icon;
        nameText.text = woogie.woogieName;
        statsText.text = "Stats";
        statsText.text += "<br>";
        statsText.text += "<br>Attack: " + woogie.stats.att;
        statsText.text += "<br>Defence: " + woogie.stats.def;
        statsText.text += "<br>Sp. Att: " + woogie.stats.s_att;
        statsText.text += "<br>Sp. Def: " + woogie.stats.s_def;
        statsText.text += "<br>Speed: " + woogie.stats.spd;

        foreach(TypingScrObj type in woogie.typing)
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
