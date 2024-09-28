using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System.IO;

[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField] private List<TKey> keys = new List<TKey>();
    [SerializeField] private List<TValue> values = new List<TValue>();

    public void OnAfterDeserialize()
    {
        this.Clear();

        for (int i = 0; i < keys.Count; i++)
        {
            this.Add(keys[i], values[i]);
        }
    }

    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();

        foreach (KeyValuePair<TKey, TValue> kvp in this)
        {
            keys.Add(kvp.Key);
            values.Add(kvp.Value);
        }
    }
}

[System.Serializable]
public class LevelData
{
    public SerializableDictionary<string, bool> Levels = new SerializableDictionary<string, bool>();
}

public class GameSaver : MonoBehaviour
{
    public static GameSaver Instance => _instance;
    private static GameSaver _instance;

    public LevelData levels;

    [SerializeField] private string configDirectory = "Saves";
    [SerializeField] private string fileName = "save.sav";

    /// <summary> Get the folder where the game .exe file is </summary>
    private string _directoryPath
    {
        get
        {
            string path = @$"{Directory.GetParent(Application.dataPath)}/{configDirectory}";
#if UNITY_EDITOR
            path = @$"{Application.dataPath}/{configDirectory}";
#endif
            return path;
        }
    }

    private string _path => @$"{_directoryPath}/{fileName}";

    public void OpenConfigDirectory() => Process.Start("explorer.exe", _directoryPath);

    public void OpenConfigFile() => Process.Start("notepad.exe", _path);

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        LoadSettings();
        UnlockNewLevel("tutorial3");
    }

    // Creates new file with default values
    private void CreateNewGameSettings()
    {
        levels = new LevelData();
        SaveSettings();
    }

    public bool IsLevelUnlocked(string levelName)
    {
        levels.Levels.TryGetValue(levelName, out var unlocked);
        return unlocked;
    }

    public void UnlockNewLevel(string levelName)
    {
        if (!levels.Levels.TryGetValue(levelName, out var unlocked))
        {
            levels.Levels.Add(levelName, true);
        }
    }

    [ContextMenu("Save Settings")]
    public void SaveSettings()
    {
        if (!Directory.Exists(_directoryPath))
            Directory.CreateDirectory(_directoryPath);

        string json = JsonUtility.ToJson(levels, true);
        using StreamWriter write = new StreamWriter(_path);
        write.Write(json);
        write.Close();
    }

    [ContextMenu("Load Settings")]
    public void LoadSettings()
    {
        if (!Directory.Exists(_directoryPath))
            Directory.CreateDirectory(_directoryPath);

        if (!File.Exists(_path))
        {
            CreateNewGameSettings();
            return;
        }

        using StreamReader reader = new StreamReader(_path);
        string json = reader.ReadToEnd();
        reader.Close();

        levels = JsonUtility.FromJson<LevelData>(json);
    }

    [ContextMenu("New Settings")]
    public void ResetSettings() => CreateNewGameSettings();

    private void OnApplicationQuit() => SaveSettings();
}
