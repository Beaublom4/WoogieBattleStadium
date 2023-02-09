using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.MainMenu
{
    public class MainMenuManager : MonoBehaviour
    {
        public static MainMenuManager Instance;

        public MenuType[] menus;
        public Color normalButtonColor, selectedButtonColor;
        public Transform menuButtonHolder;

        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            SwitchMenu("play");
            SelectMenuButton(menuButtonHolder.GetChild(0));
        }
        public void SwitchMenu(string menuName)
        {
            foreach(MenuType mt in menus)
            {
                if (mt.menuName == menuName)
                    mt.gameObject.SetActive(true);
                else mt.gameObject.SetActive(false);
            }
        }
        public void SelectMenuButton(Transform button)
        {
            foreach(Transform t in menuButtonHolder)
            {
                if (t == button)
                    t.GetComponent<Image>().color = selectedButtonColor;
                else t.GetComponent<Image>().color = normalButtonColor;
            }
        }
    }
}
