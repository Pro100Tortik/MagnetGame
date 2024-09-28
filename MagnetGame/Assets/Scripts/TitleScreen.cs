using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] private List<Button> levelButtons;
    private GameSaver _gameSaver;

    private void Start()
    {
        _gameSaver = GameSaver.Instance;
    }

    public void StartGame(string levelName) => ChangeLevelManager.Instance.ChangeLevel(levelName);

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
