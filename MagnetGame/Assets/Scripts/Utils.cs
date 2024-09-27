using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static AsyncOperation RestartLevel() => ChangeLevel(GetCurrentLevel().buildIndex);

    public static AsyncOperation ChangeLevel(string level) => SceneManager.LoadSceneAsync(level);

    public static AsyncOperation ChangeLevel(int index) => SceneManager.LoadSceneAsync(index);

    public static Scene GetCurrentLevel() => SceneManager.GetActiveScene();

    public static Vector3 GetSpread(float xDeg, float yDeg) =>
    new Vector3(Random.Range(-xDeg, xDeg),
        Random.Range(-yDeg, yDeg),
        Random.Range(-xDeg, xDeg)) * Mathf.Deg2Rad;

    public static void EnableCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public static void DisableCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public static void ToggleCursor()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            EnableCursor();
        }
        else
        {
            DisableCursor();
        }
    }

    public static T GetRandomElement<T>(this IList<T> list) => list[Random.Range(0, list.Count)];
    public static T GetRandomElement<T>(this T[] array) => array[Random.Range(0, array.Length)];
}
