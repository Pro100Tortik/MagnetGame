using UnityEngine;

public class GameManager : MonoBehaviour
{
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Utils.ToggleCursor();
            IsPaused = Cursor.lockState != CursorLockMode.Locked;
        }

        if (Input.GetKeyDown(KeyCode.R))
            Restart();
    }
}
