using UnityEngine;

public class LevelExit : MonoBehaviour
{
    public string NextLevel => nextLevel;
    [SerializeField] private string nextLevel = "MainMenu";
    [SerializeField] private AudioClip winSound;
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
            _gameManager.LevelCompleted();
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
