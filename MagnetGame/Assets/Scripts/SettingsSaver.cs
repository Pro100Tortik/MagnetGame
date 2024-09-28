using System.Diagnostics;
using UnityEngine;
using System.IO;

public class SettingsSaver : MonoBehaviour
{
    public static SettingsSaver Instance => _instance;
    private static SettingsSaver _instance;

    public GameSettings GameSettings;

    [SerializeField] private string configDirectory = "Config";
    [SerializeField] private string fileName = "config.cfg";

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
    }

    // Creates new file with default values
    private void CreateNewGameSettings()
    {
        GameSettings = new GameSettings();
        SaveSettings();
    }

    public void ApplyScreenSettings() => Screen.fullScreen = GameSettings.FullScreen;

    [ContextMenu("Save Settings")]
    public void SaveSettings()
    {
        if (!Directory.Exists(_directoryPath))
            Directory.CreateDirectory(_directoryPath);

        string json = JsonUtility.ToJson(GameSettings, true);
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

        GameSettings = JsonUtility.FromJson<GameSettings>(json);
    }

    [ContextMenu("New Settings")]
    public void ResetSettings() => CreateNewGameSettings();

    private void OnApplicationQuit() => SaveSettings();
}
