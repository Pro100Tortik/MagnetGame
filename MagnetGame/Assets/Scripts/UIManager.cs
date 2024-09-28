using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup group;
    private ChangeLevelManager _changeLevelManager;
    private GameManager _gameManager;

    private void Start()
    {
        _changeLevelManager = ChangeLevelManager.Instance;
        _gameManager = GameManager.Instance;

        _gameManager.Paused += ToggleMenu;
        ToggleMenu();
    }

    private void OnDestroy() => _gameManager.Paused -= ToggleMenu;

    private void ToggleMenu() => group.gameObject.SetActive(_gameManager.IsPaused);

    public void Continue() => _gameManager.TogglePause();

    public void Restart()
    {
        group.interactable = false;
        _gameManager.TogglePause();
        _changeLevelManager.ChangeLevel(Utils.GetCurrentLevel().name);
    }

    public void GoToMainMenu()
    {
        group.interactable = false;
        _changeLevelManager.ChangeLevel("MainMenu");
    }
}
