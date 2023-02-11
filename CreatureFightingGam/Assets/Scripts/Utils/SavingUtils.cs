using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SavingUtils
{
    public static void WriteToFile(string fileName, string json, string directory)
    {
        string path = GetFilePath(fileName, directory);
        FileStream fileStream = new(path, FileMode.Create);

        using StreamWriter writer = new(fileStream);
        writer.Write(json);
    }
    public static string GetFilePath(string fileName, string directory)
    {
        string path = Application.persistentDataPath;
        path = Directory.CreateDirectory(path + directory).ToString();
        return path + "/" + fileName;
    }
    public static string ReadFromFile(string fileName, string directory)
    {
        string path = GetFilePath(fileName, directory);
        if (File.Exists(path))
        {
            using StreamReader reader = new(path);
            string json = reader.ReadToEnd();
            return json;
        }
        else
            Debug.LogWarning("File not found");

        return "";
    }
    public static string[] GetAllDataFileNames()
    {
        string[] files = Directory.GetFiles(Application.persistentDataPath + "/Woogies");
        for (int i = 0; i < files.Length; i++)
        {
            files[i] = Path.GetFileName(files[i]);
        }
        return files;
    }

    public static void SaveWoogie(WoogieSave woogie)
    {
        string json = JsonUtility.ToJson(woogie, true);
        WriteToFile(woogie.woogieScrObjName + "_" + woogie.secretId + ".json", json, "/Woogies");
    }
}
