using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;

public class ProfileManager : MonoBehaviour
{
    public static ProfileManager Instance;

    public string profileName;
    public TMP_Text profileNameText;
    public int coins;
    public TMP_Text coinsText;
    public int money;
    public TMP_Text moneyText;
    public int level;
    public TMP_Text levelText;
    public int xp;
    public Slider xpSlider;
    [Space]
    public WoogieScrObj[] starters;
    public GameObject[] starterButtons;
    [Space]
    public GameObject menuCreatePanel;
    public GameObject nameCreatePanel;
    public TMP_InputField nameInput;
    [Space]
    public GameObject starterSelectPanel;
    public TMP_Text typingDisplay;

    private WoogieScrObj selectedWoogie;

    private void Awake()
    {
        Instance = this;

        if (File.Exists(SavingUtils.GetFilePath("Profile.json", "/Account")))
        {
            menuCreatePanel.SetActive(false);
            LoadAccount();
        }
        else
        {
            menuCreatePanel.SetActive(true);
            nameCreatePanel.SetActive(true);
            starterSelectPanel.SetActive(false);
        }
    }
    public void ContinueToStarterSelect()
    {
        if (string.IsNullOrEmpty(nameInput.text) || nameInput.text == "")
            return;
        nameCreatePanel.SetActive(false);
        typingDisplay.text = "";
        for (int i = 0; i < starterButtons.Length; i++)
        {
            starterButtons[i].GetComponentInChildren<Image>().sprite = starters[i].icon;
            starterButtons[i].GetComponentInChildren<TMP_Text>().text = starters[i].woogieName;
        }
        starterSelectPanel.SetActive(true);
    }
    public void SelectStarter(WoogieScrObj woogieScrObj)
    {
        selectedWoogie = woogieScrObj;
        typingDisplay.text = $"{woogieScrObj.woogieName} the {woogieScrObj.typing[0].typeName} type woogie";
    }
    public void CreateAccount()
    {
        if (selectedWoogie == null)
            return;

        //Creating account
        Account account = new();
        account.accountName = nameInput.text;
        account.accountLevel = 1;
        account.team = new WoogieSave[4];

        //Saving account
        string json = JsonUtility.ToJson(account);
        SavingUtils.WriteToFile("Profile.json", json, "/Account");

        //Creating starter woogie
        WoogieSave woogie = WoogieUtils.CreateNewWoogie(selectedWoogie, 5, 5, 0);
        SavingUtils.SaveWoogie(woogie);

        LoadAccount();
        menuCreatePanel.SetActive(false);
    }
    public void LoadAccount()
    {
        string json = SavingUtils.ReadFromFile("Profile.json", "/Account");
        Debug.Log(json);
        Account account = JsonUtility.FromJson<Account>(json);
        Debug.Log(account.accountName);
        profileName = account.accountName;
        profileNameText.text = profileName;
        level = account.accountLevel;
        levelText.text = level.ToString();
        xp = account.accountXp;
        xpSlider.value = xp;
        coins = account.accountCoins;
        coinsText.text = coins.ToString();
        money = account.accountMoney;
        moneyText.text = money.ToString();

        if (TeamManager.Instance == null) TeamManager.Instance = FindObjectOfType<TeamManager>(true);
        for (int i = 0; i < TeamManager.Instance.selectedWoogies.Length; i++)
        {
            TeamManager.Instance.selectedWoogies[i] = account.team[i];
        }
        TeamManager.Instance.ShowDisplayFromSlot(0);
        TeamManager.Instance.UpdateSlots();
    }
    public void SaveAccount()
    {
        Account account = new();
        account.accountName = profileName;
        account.accountLevel = level;
        account.accountXp = xp;
        account.accountCoins = coins;
        account.accountMoney = money;
        account.team = TeamManager.Instance.selectedWoogies;

        string json = JsonUtility.ToJson(account, true);
        SavingUtils.WriteToFile("Profile.json", json, "/Account");
    }
    public void AddCoins(int _coins)
    {
        coins += _coins;
        coinsText.text = coins.ToString();
        SaveAccount();
    }
    public void RemoveCoins(int _coins)
    {
        coins -= _coins;
        coinsText.text = coins.ToString();
        SaveAccount();
    }
    public void AddMoney(int _money)
    {
        money += _money;
        moneyText.text = money.ToString();
        SaveAccount();
    }
    public void RemoveMoney(int _money)
    {
        money -= _money;
        moneyText.text = money.ToString();
        SaveAccount();
    }
}
