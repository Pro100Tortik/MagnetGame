using System.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class ChangeLevelManager : MonoBehaviour
{
    public static ChangeLevelManager Instance => _instance;
    private static ChangeLevelManager _instance;

    [SerializeField] private CanvasGroup group;
    [SerializeField] private float fadeSpeed = 2f;
    private CancellationTokenSource _source;
    private bool _changingLevel = false;
    private GameManager _gameManager;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        _source = new CancellationTokenSource();
        FadeIn(_source);
    }

    private void Start() => _gameManager = GameManager.Instance;

    public async void ChangeLevel(string levelName)
    {
        if (_changingLevel)
            return;

        AsyncOperation oper = Utils.ChangeLevel(levelName);
        _source?.Cancel();
        _source = new CancellationTokenSource();
        _changingLevel = true;
        FadeOut(_source);

        oper.allowSceneActivation = false;

        while (oper.progress < 0.7f)
        {
            await Task.Delay((int)(Time.fixedDeltaTime * 1000));
        }

        await Task.Delay(2000);
        oper.allowSceneActivation = true;
        await Task.Delay(500);
        _changingLevel = false;

        if (_gameManager.IsPaused)
            _gameManager.TogglePause();

        if (Utils.GetCurrentLevel().name == "MainMenu")
            Utils.EnableCursor();

        FadeIn(_source);
    }

    private async void FadeIn(CancellationTokenSource token)
    {
        float timer = 0f;

        while (group.alpha > 0)
        {
            if (token.IsCancellationRequested)
                break;

            timer += Time.fixedDeltaTime;
            group.alpha -= Time.fixedDeltaTime * fadeSpeed;
            await Task.Delay((int)(Time.fixedDeltaTime * 1000));
        }

        if (token.IsCancellationRequested)
            return;

        group.alpha = 0f;
    }

    private async void FadeOut(CancellationTokenSource token)
    {
        float timer = 0f;

        while (group.alpha < 1f)
        {
            if (token.IsCancellationRequested)
                break;

            timer += Time.fixedDeltaTime;
            group.alpha += Time.fixedDeltaTime * fadeSpeed;
            await Task.Delay((int)(Time.fixedDeltaTime * 1000));
        }

        if (token.IsCancellationRequested)
            return;

        group.alpha = 1f;
    }

    private void OnDestroy() => _source?.Cancel();
}
