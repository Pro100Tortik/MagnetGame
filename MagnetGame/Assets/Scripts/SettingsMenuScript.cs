using UnityEngine.Events;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;

namespace ProjectMOMENTUM
{
    public class SettingsMenuScript : MonoBehaviour
    {
        [SerializeField] private AudioMixer audioMixer;

        [Header("Graphics")]
        [SerializeField] private Toggle fullScreenToggle;

        [Header("Audio")]
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider SFXVolumeSlider;

        public UnityEvent OnGoBack;

        private SettingsSaver _settings;

        private void OnEnable()
        {
            _settings = SettingsSaver.Instance;
            UpdateVariables();
        }

        public void GoBack() => OnGoBack?.Invoke();

        public void SetMusicVolume(float volume)
        {
            audioMixer.SetFloat("Music", volume);
            _settings.GameSettings.MusicVolume = volume;
        }

        public void SetSFXVolume(float volume)
        {
            audioMixer.SetFloat("SFX", volume);
            _settings.GameSettings.SFXVolume = volume;
        }

        public void ToggleFullScreen(bool value)
        {
            Screen.fullScreen = value;
            _settings.GameSettings.FullScreen = value;
        }


        private void UpdateVariables()
        {
            Debug.Log(_settings.GameSettings.MusicVolume);
            musicVolumeSlider.value = _settings.GameSettings.MusicVolume;
            SFXVolumeSlider.value = _settings.GameSettings.SFXVolume;
            fullScreenToggle.isOn = _settings.GameSettings.FullScreen;
        }

        private void OnDisable() => _settings?.SaveSettings();
    }
}