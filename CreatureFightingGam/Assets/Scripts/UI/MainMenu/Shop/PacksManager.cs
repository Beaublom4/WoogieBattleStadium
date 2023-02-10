using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PacksManager : MonoBehaviour
{
    public class PackTabInfo
    {
        public string tabName;
        public List<PackScrObj> packs = new();
    }
    public PackScrObj[] packs;
    public List<PackTypes> types;
    private PackScrObj currentPack;

    public TMP_Text packNameText;
    public TMP_Text coinsAmountText;
    public TMP_Text moneyAmountText;
    [Space]
    public Transform packTabs;
    public Transform packTypesHolder;
    [Space]
    public GameObject packTabButton;
    public GameObject packsHolder;
    public GameObject packPrefab;
    [Space]
    public Transform cardHolder;
    public GameObject cardPrefab;

    private const string glyphs = "abcdefghijklmnopqrstovwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ123456789";

    private void Awake()
    {
        List<PackTabInfo> tabInfos = new();
        foreach(PackScrObj pack in packs)
        {
            if (tabInfos.Count > 0)
            {
                foreach (PackTabInfo info in tabInfos)
                {
                    if (info.tabName == pack.packType)
                    {
                        info.packs.Add(pack);
                        continue;
                    }
                }
            }
            PackTabInfo newTab = new();
            newTab.tabName = pack.packType;
            newTab.packs.Add(pack);
            tabInfos.Add(newTab);
        }

        foreach(PackTabInfo tabs in tabInfos)
        {
            GameObject newButton = Instantiate(packTabButton, packTabs);
            newButton.GetComponentInChildren<TMP_Text>().text = tabs.tabName;
            newButton.GetComponent<Button>().onClick.AddListener(delegate { SwitchPackTypes(tabs.tabName); });

            GameObject newHolder = Instantiate(packsHolder, packTypesHolder);
            newHolder.GetComponent<PackTypes>().packTypeName = tabs.tabName;
            types.Add(newHolder.GetComponent<PackTypes>());
            foreach(PackScrObj pack in tabs.packs)
            {
                Instantiate(packPrefab, newHolder.transform).GetComponent<PackHolder>().SetUp(pack, this);
            }
        }
    }
    public void SwitchPackTypes(string name)
    {
        foreach (PackTypes pt in types)
        {
            if (pt.packTypeName == name)
            {
                pt.gameObject.SetActive(true);
            }
            else
            {
                pt.gameObject.SetActive(false);
            }
        }
    }
    public void BuyPack(PackScrObj pack)
    {
        currentPack = pack;
        packNameText.text = pack.packName;
        coinsAmountText.text = $"Buy with {pack.packCoinCost} coins";
        moneyAmountText.text = $"Buy with {pack.packMoneyCost} money";
        Scripts.UI.MainMenu.MainMenuManager.Instance.SwitchMenu("packConfirm");
    }
    public void ConfirmWithCoins()
    {
        if (ProfileManager.Instance.coins >= currentPack.packCoinCost)
        {
            ProfileManager.Instance.RemoveCoins(currentPack.packCoinCost);
            GetItems();
        }
    }
    public void ConfirmWithMoney()
    {
        if(ProfileManager.Instance.money >= currentPack.packMoneyCost)
        {
            ProfileManager.Instance.RemoveMoney(currentPack.packMoneyCost);
            GetItems();
        }
    }
    public void GetItems()
    {
        Scripts.UI.MainMenu.MainMenuManager.Instance.SwitchMenu("pack");

        foreach(Transform t in cardHolder)
        {
            Destroy(t.gameObject);
        }
        for (int i = 0; i < currentPack.itemsInPack; i++)
        {
            Random.InitState((int)System.DateTime.Now.Ticks);
            //Getting random possible woogie from pack
            WoogieScrObj woogieScrObj = currentPack.woogies[Random.Range(0, currentPack.woogies.Length)];
            //Settings stats
            WoogieSave woogie = new();
            //Getting a secret ID
            string newId = "";
            for (int s = 0; s < 9; s++)
            {
                newId += glyphs[Random.Range(0, glyphs.Length)];
            }
            woogie.secretId = newId;
            woogie.woogieScrObjName = woogieScrObj.name;
            woogie.individualStats = RandomStats(0, 31);
            woogie.natureScrObjName = woogieScrObj.possibleNatures[Random.Range(0, woogieScrObj.possibleNatures.Length)].name;
            woogie.abilitie = woogieScrObj.possibleAbilities[Random.Range(0, woogieScrObj.possibleAbilities.Length)];
            //Getting the base attacks
            string[] currentAttacksScrObjs = new string[4];
            for (int a = 0; a < woogieScrObj.attackUnlocks.Length && a < 4; a++)
            {
                if (woogieScrObj.attackUnlocks[a].levelUnlocked == 0)
                {
                    currentAttacksScrObjs[a] = woogieScrObj.attackUnlocks[a].possibleAttack[Random.Range(0, woogieScrObj.attackUnlocks[a].possibleAttack.Length)].name;
                }
                else
                    break;
            }
            woogie.selectedAttacksScrObjNames = currentAttacksScrObjs;
            woogie.currentLevel = Random.Range(currentPack.minLevel, currentPack.maxLevel + 1);
            woogie.shiny = Random.Range(0, currentPack.shinyChance) == 0;

            GameObject newCard = Instantiate(cardPrefab, cardHolder);
            newCard.GetComponent<CardHolder>().SetUp(woogie, woogieScrObj);

            Scripts.UI.MainMenu.Inventory.InventoryManager.Instance.SaveWoogie(woogie);
        }
    }
    public Stats RandomStats(int minStat = 0, int maxStat = 0)
    {
        Stats randomStats = new();
        maxStat++;
        randomStats.hp = Random.Range(minStat, maxStat);
        randomStats.att = Random.Range(minStat, maxStat);
        randomStats.def = Random.Range(minStat, maxStat);
        randomStats.s_att = Random.Range(minStat, maxStat);
        randomStats.s_def = Random.Range(minStat, maxStat);
        randomStats.spd = Random.Range(minStat, maxStat);

        return randomStats;
    }

}
