using UnityEngine;

[System.Serializable]
public class GameSettings
{
    [Header("Screen Settings")]
    public bool FullScreen = false;

    [Header("Gameplay Settings")]
    [Range(-80.0f, 10.0f)] public float MusicVolume = 1.0f;
    [Range(-80.0f, 10.0f)] public float SFXVolume = 1.0f;
}
