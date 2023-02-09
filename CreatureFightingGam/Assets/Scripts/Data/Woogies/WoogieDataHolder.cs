using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Scripts.UI.MainMenu.Inventory
{
    public class WoogieDataHolder : MonoBehaviour
    {
        public TMP_Text displayName;
        public Image displayImage;

        public WoogieData woogieData;

        public void SetUp(WoogieData data)
        {
            woogieData = data;
            displayName.text = woogieData.woogie.woogieName;
            displayImage.sprite = woogieData.woogie.icon;
        }
        public void SelectData()
        {
            InventoryManager.Instance.SelectData(woogieData);
        }
    }
}
