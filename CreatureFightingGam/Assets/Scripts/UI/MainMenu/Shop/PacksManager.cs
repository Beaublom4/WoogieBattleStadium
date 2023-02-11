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
    private float waitTime = .2f;
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
        Scripts.UI.MainMenu.MainMenuManager.Instance.StartCoroutine(PackOpening());
    }
    IEnumerator PackOpening()
    {
        //Getting woogies from a pack
        List<DroppedWoogie> droppedWoogies = new();
        Random.InitState((int)System.DateTime.Now.Ticks);
        for (int i = 0; i < currentPack.itemsInPack; i++)
        {
            //Getting random rarity for next woogie
            float currentDropRarity = Random.Range(0f, 100f);

            //Getting an array form all the woogies in that rarity
            WoogieRarityDrops drops = new();
            for (int r = 0; r < currentPack.drops.Length; r++)
            {
                if(currentDropRarity <= currentPack.drops[r].dropChance)
                {
                    drops = currentPack.drops[r];
                }
                else
                {
                    break;
                }
            }

            //Getting a woogie from the array above
            WoogieScrObj woogieScrObj = drops.woogieScrObjs[Random.Range(0, drops.woogieScrObjs.Length)];
            //Creating random stats for that woogie
            WoogieSave woogieSave = WoogieUtils.CreateNewWoogie(woogieScrObj, currentPack.minLevel, currentPack.maxLevel, currentPack.shinyChance);

            //Adding woogie to a list with all the dropped woogies;
            DroppedWoogie newWoogie = new();
            newWoogie.woogieSave = woogieSave;
            newWoogie.woogieScrObj = woogieScrObj;
            newWoogie.rarity = drops.rarity;
            droppedWoogies.Add(newWoogie);
        }
        //sort the list above to rarity
        CardRarity[] rarities = Resources.LoadAll<CardRarity>("CardRarities");
        if (rarities.Length == 0)
            yield return null;
        List<DroppedWoogie> sortedDroppedWoogies = new();
        foreach (CardRarity raritie in rarities) 
        {
            foreach (DroppedWoogie dropWoogie in droppedWoogies)
            {
                if (dropWoogie.rarity == raritie)
                {
                    sortedDroppedWoogies.Add(dropWoogie);
                }
            }
        }
        //Display all woogies sorted in rarity
        for (int i = 0; i < sortedDroppedWoogies.Count; i++)
        {
            GameObject newCard = Instantiate(cardPrefab, cardHolder);
            newCard.GetComponent<CardHolder>().SetUp(sortedDroppedWoogies[i].woogieSave, sortedDroppedWoogies[i].woogieScrObj, sortedDroppedWoogies[i].rarity.rarityColor);

            SavingUtils.SaveWoogie(sortedDroppedWoogies[i].woogieSave);
            yield return new WaitForSeconds(waitTime);
        }
    }
}
public struct DroppedWoogie
{
    public WoogieSave woogieSave;
    public WoogieScrObj woogieScrObj;
    public CardRarity rarity;
}
