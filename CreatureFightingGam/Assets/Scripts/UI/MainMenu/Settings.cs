using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    public void DeleteAccount()
    {
        File.Delete(SavingUtils.GetFilePath("/Woogies", ""));
        File.Delete(SavingUtils.GetFilePath("/Account", ""));
        ReloadMenu();
    }
    public void ReloadMenu()
    {
        SceneManager.LoadScene(0);
    }
}
