using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class SaveData
{
    public string locale = "FR";
}

public static class SaveManager
{
    
    
    static SaveData saveData = null;
    
    

    public static void updateLocale(string locale)
    {
        saveData.locale = locale;
        save();
    }
    

    private static string getDataPath()
    {
        return $"{Application.persistentDataPath}/save.fun";
    }

    public static void reset()
    {
        saveData = new SaveData();
        save();
    }

    public static void save()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(getDataPath(), FileMode.Create);
        
        formatter.Serialize(stream, saveData);
        stream.Close();
    }

    public static void load()
    {
        if (File.Exists(getDataPath()))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(getDataPath(), FileMode.Open);
            try
            {
                saveData = formatter.Deserialize(stream) as SaveData;
                stream.Close();
                Debug.Log("Save loaded");
            }
            catch
            {
                Debug.LogWarning("Incompatible save format, resetting save");
                stream.Close();
                reset();
                saveData = new SaveData();
            }
            
            
        }
        else
        {
            Debug.LogWarning("Save file not found");
            saveData = new SaveData();
        }

        onLoad();
    }

    private static void onLoad()
    {
        LocalizationManager.setLocale(saveData.locale);
    }
    
}
