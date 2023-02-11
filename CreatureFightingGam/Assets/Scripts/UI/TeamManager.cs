using Scripts.UI.MainMenu;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TeamManager : MonoBehaviour
{
    public static TeamManager Instance;

    public WoogieSave[] selectedWoogies = new WoogieSave[4];
    public GameObject[] slotsButtons;
    public Image[] playScreenTeam;
    public int selectedSlot = 0;

    public GameObject coverObj, hightlightObj;
    public Sprite emptySlot;

    private WoogieSave switchWoogie;
    private float highlightTimer;
    private bool changeBySlotButton;

    public Transform attackHolder;
    public GameObject attackPrefab;

    [Header("Display")]
    public RawImage model;
    public TMP_Text level;
    public TMP_Text infoText;
    public Transform typingHolder;
    public GameObject typingPrefab;
    public TMP_Text nature;
    public TMP_Text statsText;
    public TMP_Text ability;
    public TMP_Text abilityDiscription;

    private void Awake()
    {
        Instance = this;
    }
    private void OnEnable()
    {
        coverObj.SetActive(false);
        selectedSlot = 0;
        ShowDisplayFromSlot(selectedSlot);
        UpdateSlots();
    }
    private void Update()
    {
        if(switchWoogie != null)
        {
            if(highlightTimer > 0)
            {
                highlightTimer -= Time.deltaTime;
            }
            else
            {
                highlightTimer = .1f;
                hightlightObj.SetActive(!hightlightObj.activeSelf);
            }
        }
    }
    public void SwitchWoogie(WoogieSave woogie)
    {
        coverObj.SetActive(true);
        hightlightObj.SetActive(true);
        highlightTimer = .1f;
        switchWoogie = woogie;

        //Check if there is an empty slot
        for (int i = 0; i < selectedWoogies.Length; i++)
        {
            if (string.IsNullOrEmpty(selectedWoogies[i].woogieScrObjName))
            {
                Fill_ReplaceSlot(i, woogie);
                selectedSlot = i;
                ShowDisplayFromSlot(selectedSlot);
                return;
            }
        }
        changeBySlotButton = true;
    }
    public void TeamSlotButton(int i)
    {
        //Change slot by pressing slot button
        if (changeBySlotButton)
        {
            Fill_ReplaceSlot(i, switchWoogie);
            changeBySlotButton = false;
            return;
        }
        //Display slot button clicked
        if (string.IsNullOrEmpty(selectedWoogies[i].woogieScrObjName))
            return;
        selectedSlot = i;
        ShowDisplayFromSlot(selectedSlot);
    }
    public void Fill_ReplaceSlot(int slot, WoogieSave woogie)
    {
        switchWoogie = null;
        hightlightObj.SetActive(false);
        selectedWoogies[slot] = woogie;
        UpdateSlots();
        coverObj.SetActive(false);
        ProfileManager.Instance.SaveAccount();
    }
    public void EmptySlot()
    {
        WoogieSave save = new();
        Fill_ReplaceSlot(selectedSlot, save);

        for (int i = 0; i < selectedWoogies.Length - 1; i++)
        {
            if (string.IsNullOrEmpty(selectedWoogies[i].woogieScrObjName))
            {
                if(!string.IsNullOrEmpty(selectedWoogies[i + 1].woogieScrObjName))
                {
                    Fill_ReplaceSlot(i, selectedWoogies[i + 1]);
                    Fill_ReplaceSlot(i + 1, new WoogieSave());
                }
            }
        }

        selectedSlot = 0;
        ShowDisplayFromSlot(selectedSlot);
    }
    public void UpdateSlots()
    {
        for (int i = 0; i < slotsButtons.Length; i++)
        {
            if (!string.IsNullOrEmpty(selectedWoogies[i].woogieScrObjName))
            {
                WoogieScrObj woogieScrObj = Resources.Load<WoogieScrObj>("Woogies/" + selectedWoogies[i].woogieScrObjName);
                foreach (Transform t in slotsButtons[i].transform)
                    if (t.GetComponent<Image>())
                        t.GetComponent<Image>().sprite = woogieScrObj.icon;
                playScreenTeam[i].sprite = woogieScrObj.icon;
                slotsButtons[i].GetComponentInChildren<TMP_Text>().text = woogieScrObj.woogieName;
            }
            else
            {
                foreach (Transform t in slotsButtons[i].transform)
                    if(t.GetComponent<Image>())
                        t.GetComponent<Image>().sprite = emptySlot;
                playScreenTeam[i].sprite = emptySlot;
                slotsButtons[i].GetComponentInChildren<TMP_Text>().text = "None";
            }
        }
    }
    public void ShowDisplayFromSlot(int slot)
    {
        if (string.IsNullOrEmpty(selectedWoogies[slot].woogieScrObjName))
        {
            coverObj.SetActive(true);
            return;
        }

        WoogieSave woogieSave = selectedWoogies[slot];
        WoogieScrObj woogieScrObj = Resources.Load<WoogieScrObj>("Woogies/" + woogieSave.woogieScrObjName);

        //Display info
        infoText.text = woogieScrObj.woogieName;
        //Display level
        level.text = "Lv: " + woogieSave.currentLevel.ToString();
        //Display Type
        foreach (Transform t in typingHolder)
            Destroy(t.gameObject);
        foreach (TypingScrObj type in woogieScrObj.typing)
        {
            GameObject newType = Instantiate(typingPrefab, typingHolder);
            newType.GetComponentInChildren<TMP_Text>().text = type.typeName;
            newType.GetComponent<Image>().color = type.typeColor;
        }
        //Display nature
        NatureScrObj natureScrObj = Resources.Load<NatureScrObj>("Natures/" + woogieSave.natureScrObjName);
        nature.text = natureScrObj.natureName;
        //Display stats from selected woogie
        statsText.text = "<br>Health: " + CalculationUtils.HpCalculation(woogieScrObj.baseStats.hp, woogieSave.individualStats.hp, woogieSave.EffortStats.hp, woogieSave.currentLevel);
        statsText.text += "<br>Attack: " + CalculationUtils.StatCaclulation(woogieScrObj.baseStats.att, woogieSave.individualStats.att, woogieSave.EffortStats.att, woogieSave.currentLevel, natureScrObj.attack);
        statsText.text += "<br>Defence: " + CalculationUtils.StatCaclulation(woogieScrObj.baseStats.def, woogieSave.individualStats.att, woogieSave.EffortStats.att, woogieSave.currentLevel, natureScrObj.defence);
        statsText.text += "<br>Sp. Att: " + CalculationUtils.StatCaclulation(woogieScrObj.baseStats.s_att, woogieSave.individualStats.att, woogieSave.EffortStats.att, woogieSave.currentLevel, natureScrObj.sp_att);
        statsText.text += "<br>Sp. Def: " + CalculationUtils.StatCaclulation(woogieScrObj.baseStats.s_def, woogieSave.individualStats.att, woogieSave.EffortStats.att, woogieSave.currentLevel, natureScrObj.sp_def);
        statsText.text += "<br>Speed: " + CalculationUtils.StatCaclulation(woogieScrObj.baseStats.spd, woogieSave.individualStats.att, woogieSave.EffortStats.att, woogieSave.currentLevel, natureScrObj.speed);
        //Display Ability
        ability.text = "Ability: " + woogieSave.ability;
        abilityDiscription.text = "Ability discription";
        //Display Attacks
        foreach (Transform t in attackHolder)
            Destroy(t.gameObject);
        foreach(string attack in woogieSave.selectedAttacksScrObjNames)
        {
            GameObject newAttack = Instantiate(attackPrefab, attackHolder);
            if (!string.IsNullOrEmpty(attack))
            {
                AttackScrObj attackScrObj = Resources.Load<AttackScrObj>("Attacks/" + attack);
                newAttack.GetComponent<AttackHolder>().SetUp(attackScrObj);
            }
            else
            {
                newAttack.GetComponent<AttackHolder>().SetUp(null);
            }
        }
    }
}
