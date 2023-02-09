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
            WoogieScrObj woogie = currentPack.woogies[Random.Range(0, currentPack.woogies.Length)];
            GameObject newCard = Instantiate(cardPrefab, cardHolder);
            newCard.GetComponent<CardHolder>().SetUp(woogie);

            Scripts.UI.MainMenu.Inventory.InventoryManager.Instance.SaveNewWoogie(woogie);
        }
    }
}
