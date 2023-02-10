using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Scripts.UI.MainMenu.Inventory
{
    public class WoogieSaveHolder : MonoBehaviour
    {
        public TMP_Text displayName;
        public Image displayImage;
        public TMP_Text levelDisplay;

        public WoogieSave woogieSave;

        public void SetUp(WoogieSave woogie)
        {
            woogieSave = woogie;
            WoogieScrObj woogieScrObj = Resources.Load<WoogieScrObj>("Woogies/" + woogie.woogieScrObjName);
            displayName.text = woogieScrObj.woogieName;
            displayImage.sprite = woogieScrObj.icon;
            levelDisplay.text = "Lv: " + woogieSave.currentLevel.ToString();
        }
        public void SelectData()
        {
            InventoryManager.Instance.SelectData(woogieSave);
        }
    }
}
