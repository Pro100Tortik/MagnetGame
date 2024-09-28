using UnityEngine;
using UnityEngine.UI;

public class LevelSelectButton : MonoBehaviour
{
    public string LevelName => levelName;
    [SerializeField] private string levelName;
    private Button _button;

    private void Start()
    {
        _button = GetComponent<Button>();
        _button.interactable = GameSaver.Instance.IsLevelUnlocked(levelName);
    }

    public void LoadLevel() => ChangeLevelManager.Instance.ChangeLevel(levelName);
}
