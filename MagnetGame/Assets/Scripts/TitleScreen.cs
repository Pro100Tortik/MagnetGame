using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    public void StartGame(string levelName) => ChangeLevelManager.Instance.ChangeLevel(levelName);

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
