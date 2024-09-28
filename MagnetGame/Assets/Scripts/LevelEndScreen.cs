using TMPro;
using UnityEngine;

public class LevelEndScreen : MonoBehaviour
{
    [SerializeField] private CanvasGroup group;
    [SerializeField] private LevelTimer timer;
    [SerializeField] private TMP_Text completionText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text collectablesText;
    [SerializeField] private TMP_Text gradeText;
    private GameManager _gameManager;
    private Collectables[] _collectables;
    private int _collected;

    private void Awake()
    {
        _collectables = FindObjectsOfType<Collectables>();
        for (int i = 0; i < _collectables.Length; i++)
        {
            _collectables[i].Collect += Collect;
        }

        group.gameObject.SetActive(false);
    }

    private void Collect() => _collected++;

    public void NextLevel() => ChangeLevelManager.Instance.ChangeLevel(FindObjectOfType<LevelExit>().NextLevel);

    private void Start()
    {
        _gameManager = GameManager.Instance;

        _gameManager.LevelComplete += ShowResult;
    }

    private void OnDestroy()
    {
        _gameManager.LevelComplete -= ShowResult;

        for (int i = 0; i < _collectables.Length; i++)
        {
            _collectables[i].Collect -= Collect;
        }
    }

    private void ShowResult()
    {
        group.gameObject.SetActive(true);

        completionText.text = "Completed";
        timerText.text = timer.CurrentLevelTime.ToString("0.0");
        collectablesText.text = $"{_collected}/{_collectables.Length}";
        gradeText.text = $"grade: {GetGrade()}";
    }

    private string GetGrade()
    {
        if (_collected >= _collectables.Length && timer.CurrentLevelTime < 90f)
            return "S";

        if (_collected < _collectables.Length && timer.CurrentLevelTime < 90f)
            return "A";

        if (timer.CurrentLevelTime > 120f)
            return "C";
        else if (timer.CurrentLevelTime > 90f)
            return "B";

        return "F";
    }
}
