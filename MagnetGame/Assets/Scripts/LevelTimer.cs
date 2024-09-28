using UnityEngine;

public class LevelTimer : MonoBehaviour
{
    [SerializeField] private bool startTimer = true;

    public float CurrentLevelTime => _timer;
    private float _timer;

    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameManager.Instance;

        _gameManager.LevelComplete += StopTimer;
    }

    private void Update()
    {
        if (startTimer)
            _timer += Time.deltaTime;
    }

    public void StartTimer() => startTimer = true;

    public void ResetTimer()
    {
        startTimer = false;
        _timer = 0;
    }

    public void StopTimer() => startTimer = false;

    private void OnDestroy()
    {
        _gameManager.LevelComplete -= StopTimer;
    }
}
