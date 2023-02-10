using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.MainMenu.Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager Instance;

        public TMP_Text toggleStats_AttacksText;
        public GameObject statsObj, attacksObj;
        [Space]
        public InventoryHolderType[] menus;
        public GameObject[] sideBars;
        private int currentMenu;
        public Color normalButtonColor, selectedButtonColor;
        public Transform menuButtonHolder;

        [Header("Woogies")]
        public Transform woogieHolder;
        public GameObject woogieDataHolderPrefab;
        public WoogieSave selectedWoogie;
        [Header("stats")]
        public Transform typingHolder;
        public GameObject typingPrefab;
        public TMP_Text nature;
        public TMP_Text statsText;
        public TMP_Text ability, abilityDiscription;

        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            SwitchMenu("woogie");
            SelectMenuButton(menuButtonHolder.GetChild(0));
        }
        public void OnEnable()
        {
            selectedWoogie = null;
            foreach (GameObject s in sideBars)
                s.SetActive(false);
            foreach(Transform t in woogieHolder)
            {
                Destroy(t.gameObject);
            }
            LoadWoogies();
        }
        public void ToggleStats_Attacks()
        {
            if (statsObj.activeSelf)
            {
                statsObj.SetActive(false);
                attacksObj.SetActive(true);
                toggleStats_AttacksText.text = "Stats";
            }
            else
            {
                statsObj.SetActive(true);
                attacksObj.SetActive(false);
                toggleStats_AttacksText.text = "Attack";
            }
        }
        public void SwitchWoogie()
        {
            
        }
        public void SwitchMenu(string menuType)
        {
            for (int i = 0; i < menus.Length; i++)
            {
                if (menus[i].holderType == menuType)
                {
                    currentMenu = i;
                    menus[i].gameObject.SetActive(true);
                    sideBars[i].SetActive(true);
                }
                else
                {
                    menus[i].gameObject.SetActive(false);
                    sideBars[i].SetActive(false);
                }
            }
        }
        public void SelectMenuButton(Transform button)
        {
            foreach (Transform t in menuButtonHolder)
            {
                if (t == button)
                    t.GetComponent<Image>().color = selectedButtonColor;
                else t.GetComponent<Image>().color = normalButtonColor;
            }
        }

        public void SelectData(WoogieSave woogie)
        {
            sideBars[currentMenu].SetActive(true);
            selectedWoogie = woogie;
            WoogieScrObj woogieScrObj = Resources.Load<WoogieScrObj>("Woogies/" + woogie.woogieScrObjName);

            //Display Type
            foreach (Transform t in typingHolder)
                Destroy(t.gameObject);
            foreach(TypingScrObj type in woogieScrObj.typing)
            {
                GameObject newType = Instantiate(typingPrefab, typingHolder);
                newType.GetComponentInChildren<TMP_Text>().text = type.typeName;
                newType.GetComponent<Image>().color = type.typeColor;
            }
            //Display nature
            NatureScrObj natureScrObj = Resources.Load<NatureScrObj>("Natures/" + woogie.natureScrObjName);
            nature.text = natureScrObj.natureName;
            //Display stats from selected woogie
            statsText.text = "<br>Health: " + CalculationUtils.HpCalculation(woogieScrObj.baseStats.hp, woogie.individualStats.hp, woogie.EffortStats.hp, woogie.currentLevel);
            statsText.text += "<br>Attack: " + CalculationUtils.StatCaclulation(woogieScrObj.baseStats.att, woogie.individualStats.att, woogie.EffortStats.att, woogie.currentLevel, natureScrObj.attack);
            statsText.text += "<br>Defence: " + CalculationUtils.StatCaclulation(woogieScrObj.baseStats.def, woogie.individualStats.att, woogie.EffortStats.att, woogie.currentLevel, natureScrObj.defence);
            statsText.text += "<br>Sp. Att: " + CalculationUtils.StatCaclulation(woogieScrObj.baseStats.s_att, woogie.individualStats.att, woogie.EffortStats.att, woogie.currentLevel, natureScrObj.sp_att);
            statsText.text += "<br>Sp. Def: " + CalculationUtils.StatCaclulation(woogieScrObj.baseStats.s_def, woogie.individualStats.att, woogie.EffortStats.att, woogie.currentLevel, natureScrObj.sp_def);
            statsText.text += "<br>Speed: " + CalculationUtils.StatCaclulation(woogieScrObj.baseStats.spd, woogie.individualStats.att, woogie.EffortStats.att, woogie.currentLevel, natureScrObj.speed);
            //Display Ability
            ability.text = "Ability: " + woogie.abilitie;
            abilityDiscription.text = "Ability discription";
        }

        public void SaveWoogie(WoogieSave woogie)
        {
            string json = JsonUtility.ToJson(woogie, true);
            SavingUtils.WriteToFile(woogie.woogieScrObjName + "_" + woogie.secretId + ".json", json, "/Woogies");
        }
        public void LoadWoogies()
        {
            foreach (string s in SavingUtils.GetAllDataFileNames())
            {
                WoogieSave woogie = new();
                string json = SavingUtils.ReadFromFile(s, "/Woogies");
                JsonUtility.FromJsonOverwrite(json, woogie);
                GameObject newDataHolder = Instantiate(woogieDataHolderPrefab, woogieHolder);
                newDataHolder.GetComponent<WoogieSaveHolder>().SetUp(woogie);
            }
        }
    }
}
