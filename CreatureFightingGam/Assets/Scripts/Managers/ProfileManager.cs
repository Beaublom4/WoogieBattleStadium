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

    public GameObject menuCreatePanel;
    public TMP_InputField nameInput;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        if (File.Exists(SavingUtils.GetFilePath("Profile.json", "/Account")))
        {
            LoadAccount();
        }
        else
        {
            menuCreatePanel.SetActive(true);
        }
    }
    public void CreateAccount()
    {
        Account account = new();
        account.accountName = nameInput.text;
        account.accountLevel = 1;

        string json = JsonUtility.ToJson(account);
        SavingUtils.WriteToFile("Profile.json", json, "/Account");

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
    }
    public void SaveAccount()
    {
        Account account = new();
        account.accountName = profileName;
        account.accountLevel = level;
        account.accountXp = xp;
        account.accountCoins = coins;
        account.accountMoney = money;

        string json = JsonUtility.ToJson(account);
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
