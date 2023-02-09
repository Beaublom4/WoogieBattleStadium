using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PackHolder : MonoBehaviour
{
    public PackScrObj pack;

    public TMP_Text packTitle;
    public TMP_Text coinsText;
    public TMP_Text moneyText;

    private PacksManager manager;

    public void SetUp(PackScrObj _pack, PacksManager _manager)
    {
        packTitle.text = _pack.packName;
        coinsText.text = _pack.packCoinCost.ToString() + "coins";
        moneyText.text = _pack.packMoneyCost.ToString() + "money";
        manager = _manager;
    }
    public void BuyPack()
    {
        manager.BuyPack(pack);
    }
}
