using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TeamButton : MonoBehaviour
{
    public TMP_Text woogieName;
    public Slider healthSlider;
    public TMP_Text healthText;
    public Transform typeHolder;
    public GameObject typePrefab;

    private int slot;

    public void SetUp(WoogieTeamMember woogieTeamMember, int _slot)
    {
        if (woogieTeamMember != null)
        {
            woogieName.text = woogieTeamMember.woogieScrObj.woogieName;
            healthSlider.maxValue = woogieTeamMember.healthLeft;
            SetHealth(woogieTeamMember.healthLeft);
            foreach (Transform t in typeHolder)
                Destroy(t.gameObject);
            foreach (TypingScrObj typing in woogieTeamMember.woogieScrObj.typing)
            {
                GameObject newType = Instantiate(typePrefab, typeHolder);
                newType.GetComponent<Image>().color = typing.typeColor;
                newType.GetComponentInChildren<TMP_Text>().text = typing.typeName;
            }
            slot = _slot;
        }
        else
        {
            woogieName.text = "Empty";
            healthSlider.gameObject.SetActive(false);
            healthText.gameObject.SetActive(false);
            typeHolder.gameObject.SetActive(false);
        }
    }
    public void SetHealth(int health)
    {
        healthSlider.value = health;
        healthText.text = health + "/" + healthSlider.maxValue;
    }
    public void SelectWoogie()
    {
        GameManager.Instance.Switch(slot);
    }
}
