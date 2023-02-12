using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    public GameObject attackButtonsMenu;
    public GameObject teamButtonsMenu;
    private bool canOpenMenu = true;

    public Transform attackButtonsHolder;
    public GameObject attackButtonPrefab;

    public Transform teamButtonsHolder;
    public GameObject teamButtonPrefab;

    public GameObject messageObj;
    public TMP_Text messageText;
    [Tooltip("Lower number means faster text")]
    public float textSpeed;
    IEnumerator routine;
    public bool messageDisplaying;

    private void Awake()
    {
        Instance = this;
    }
    public void AttackButtonsToggle()
    {
        if (!canOpenMenu)
            return;
        teamButtonsMenu.SetActive(false);
        attackButtonsMenu.SetActive(!attackButtonsMenu.activeSelf);
    }
    public void TeamButtonsToggle()
    {
        if (!canOpenMenu)
            return;
        attackButtonsMenu.SetActive(false);
        teamButtonsMenu.SetActive(!teamButtonsMenu.activeSelf);
    }
    public void LockMenus()
    {
        canOpenMenu = false;
        attackButtonsMenu.SetActive(false);
        teamButtonsMenu.SetActive(false);
    }
    public void UnlockMenus()
    {
        canOpenMenu = true;
    }
    public void CloseAllMenus()
    {
        attackButtonsMenu.SetActive(false);
        teamButtonsMenu.SetActive(false);
    }
    public void SetAttackButtons(WoogieSave woogieSave, WoogieAttackUses attackUses)
    {
        foreach (Transform t in attackButtonsHolder)
            Destroy(t.gameObject);
        for (int i = 0; i < woogieSave.selectedAttacksScrObjNames.Length; i++)
        {
            GameObject newAttackButton = Instantiate(attackButtonPrefab, attackButtonsHolder);
            if (!string.IsNullOrEmpty(woogieSave.selectedAttacksScrObjNames[i]))
            {
                AttackScrObj attackScrObj = Resources.Load<AttackScrObj>("Attacks/" + woogieSave.selectedAttacksScrObjNames[i]);
                newAttackButton.GetComponent<AttackButton>().SetUp(attackScrObj, attackUses.attacksLeft[i]);
            }
            else
            {
                newAttackButton.GetComponent<AttackButton>().SetUp(null, attackUses.attacksLeft[i]);
            }
        }
    }
    public void SetTeamButtons(WoogieTeamMember[] woogieTeamMembers)
    {
        foreach (Transform t in teamButtonsHolder)
            Destroy(t.gameObject);
        for (int i = 0; i < woogieTeamMembers.Length; i++)
        {
            GameObject newTeamButton = Instantiate(teamButtonPrefab, teamButtonsHolder);
            if (!string.IsNullOrEmpty(woogieTeamMembers[i].woogieSave.woogieScrObjName))
            {
                newTeamButton.GetComponent<TeamButton>().SetUp(woogieTeamMembers[i], i);
            }
            else
            {
                newTeamButton.GetComponent<TeamButton>().SetUp(null, i);
            }
        }
    }
    public void MessagePopUp(string message)
    {
        if (routine != null)
            StopCoroutine(routine);
        routine = MessagePopUpRoutine(message);
        StartCoroutine(routine);
    }
    IEnumerator MessagePopUpRoutine(string message)
    {
        messageDisplaying = true;
        messageText.maxVisibleCharacters = 0;
        messageText.text = message;
        messageObj.SetActive(true);
        while (messageText.maxVisibleCharacters < message.Length)
        {
            messageText.maxVisibleCharacters++;
            yield return new WaitForSeconds(textSpeed);
        }
        yield return new WaitForSeconds(.5f);
        messageObj.SetActive(false);
        messageDisplaying = false;
    }
}
