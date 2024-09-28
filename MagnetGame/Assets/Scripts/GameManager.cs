using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public event Action Paused;
    public event Action LevelComplete;

    public static GameManager Instance => _instance;
    private static GameManager _instance;

    public bool IsPaused { get; private set; }

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        if (Utils.GetCurrentLevel().name != "MainMenu")
        {
            Utils.DisableCursor();
        }
        IsPaused = Cursor.lockState != CursorLockMode.Locked;
    }

    public void Restart() => Utils.RestartLevel();

    public void TogglePause()
    {
        Utils.ToggleCursor();
        IsPaused = Cursor.lockState != CursorLockMode.Locked;
        Paused?.Invoke();
    }

    public void LevelCompleted()
    {
        TogglePause();
        LevelComplete?.Invoke();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }
}
