using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class OptionsProfile
{
    public int musicVolume = 100;
    public int sfxVolume = 100;
    public string language = "EN";
    public string windowMode = "fullscreen";
}

public static class SaveManager
{
    [Serializable]
    private class SaveData
    {
        public OptionsProfile options = new OptionsProfile();
    }
    
    public static void setOptions(OptionsProfile options)
    {
        saveData.options = options;
        Save();
    }

    public static OptionsProfile getOptions()
    {
        return saveData.options;
    }
    
    static SaveData saveData = null;

    private static string getDataPath()
    {
        return $"{Application.persistentDataPath}/save.fun";
        
    }

    public static void Reset()
    {
        saveData = new SaveData();
        
        Save();
    }

    public static void Save()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(getDataPath(), FileMode.Create);
        
        formatter.Serialize(stream, saveData);
        stream.Close();
    }

    public static void Load()
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
                Debug.LogWarning("Incompatible save format, save was reset");
                stream.Close();
                Reset();
                saveData = new SaveData();
            }
            
            
        }
        else
        {
            Debug.LogWarning("Save file not found");
            saveData = new SaveData();
        }
    }
    
}
