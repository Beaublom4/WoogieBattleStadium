using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.UI.Battle
{
    public class MenuManager : MonoBehaviour
    {
        public GameObject attackButtonsHolder;

        public void AttackButtonsToggle()
        {
            attackButtonsHolder.SetActive(!attackButtonsHolder.activeSelf);
        }
    }
}
