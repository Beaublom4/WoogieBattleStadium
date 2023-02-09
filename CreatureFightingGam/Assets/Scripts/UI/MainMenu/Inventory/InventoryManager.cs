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
        public Color normalButtonColor, selectedButtonColor;
        public Transform menuButtonHolder;

        [Header("Woogies")]
        public Transform woogieHolder;
        public GameObject woogieDataHolderPrefab;
        private const string glyphs = "abcdefghijklmnopqrstovwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ123456789";
        public WoogieData selectedData;
        [Header("stats")]
        public TMP_Text nature;
        public TMP_Text stats;
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
                    menus[i].gameObject.SetActive(true);
                    sideBars[i].gameObject.SetActive(true);
                }
                else
                {
                    menus[i].gameObject.SetActive(false);
                    sideBars[i].gameObject.SetActive(false);
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

        public void SelectData(WoogieData data)
        {
            selectedData = data;

            nature.text = selectedData.nature.natureName;
            stats.text = $"{selectedData.health} <br> {selectedData.attack} <br> {selectedData.defence} <br> {selectedData.sp_attack} <br> {selectedData.sp_defence} <br> {selectedData.speed}";
            ability.text = "Ability: " + selectedData.woogie.ability;
            abilityDiscription.text = "Ability discription";
        }

        public void SaveNewWoogie(WoogieScrObj woogie)
        {
            WoogieData newData = new();
            string newId = "";
            for (int i = 0; i < 9; i++)
            {
                newId += glyphs[Random.Range(0, glyphs.Length)];
            }
            newData.secretId = newId;
            WoogieScrObj newWoogie = Resources.Load("Woogies/" + woogie.name) as WoogieScrObj;
            newData.woogie = new WoogieSave(newWoogie.number, newWoogie.woogieName, newWoogie.icon, newWoogie.typing, newWoogie.natures, newWoogie.ability, newWoogie.stats, newWoogie.levelCurve, newWoogie.evolveLevel, newWoogie.attackUnlocks);
            AttackScrObj[] moves = new AttackScrObj[4];
            for (int i = 0; i < woogie.attackUnlocks.Length && i < 4; i++)
            {
                if (woogie.attackUnlocks[i].levelUnlocked == 0)
                {
                    moves[i] = woogie.attackUnlocks[i].attack;
                }
                else
                    break;
            }
            newData.woogieAttacks = moves;
            newData.xp = 0;
            foreach(NatureScrObj nature in woogie.natures)
            {
                newData.nature = nature;
            }
            newData.health = 10;
            newData.attack = 9;
            newData.defence = 8;
            newData.sp_attack = 7;
            newData.sp_defence = 6;
            newData.speed = 5;
            SaveWoogie(newData);
        }
        public void SaveWoogie(WoogieData data)
        {
            string json = JsonUtility.ToJson(data, true);
            SavingUtils.WriteToFile(data.woogie.woogieName + data.secretId + ".json", json, "/Woogies");
        }
        public void LoadWoogies()
        {
            foreach (string s in SavingUtils.GetAllDataFileNames())
            {
                WoogieData data = new();
                string json = SavingUtils.ReadFromFile(s, "/Woogies");
                JsonUtility.FromJsonOverwrite(json, data);
                GameObject newDataHolder = Instantiate(woogieDataHolderPrefab, woogieHolder);
                newDataHolder.GetComponent<WoogieDataHolder>().SetUp(data);
            }
        }
    }
}
