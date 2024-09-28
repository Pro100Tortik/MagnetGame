using UnityEngine;

public class LevelExit : MonoBehaviour
{
    [SerializeField] private AudioClip winSound;
    [SerializeField] private string nextLevel = "MainMenu";
    private int _playersInExitArea;
    private GameManager _gameManager;
    private ChangeLevelManager _changeLevelManager;

    private void Start()
    {
        _gameManager = GameManager.Instance;
        _changeLevelManager = ChangeLevelManager.Instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Plus") || collision.CompareTag("Minus"))
        {
            _playersInExitArea++;
        }

        if (_playersInExitArea >= 2)
        {
            _changeLevelManager.ChangeLevel(nextLevel);
            AudioManager.Instance.PlaySound(winSound);
            Debug.Log("Level Completed");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Plus") || collision.CompareTag("Minus"))
        {
            if (_playersInExitArea > 0)
                _playersInExitArea--;
        }
    }
}
