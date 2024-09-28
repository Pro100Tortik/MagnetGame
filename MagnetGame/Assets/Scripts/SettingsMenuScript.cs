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

        private GameSettings Settings => _settings.GameSettings;
        private SettingsSaver _settings;

        private void Start()
        {
            _settings = SettingsSaver.Instance;

            InitializeVariables();
        }

        public void GoBack() => OnGoBack?.Invoke();

        public void SetMusicVolume(float volume)
        {
            audioMixer.SetFloat("Music", volume);
            Settings.MusicVolume = volume;
        }

        public void SetSFXVolume(float volume)
        {
            audioMixer.SetFloat("SFX", volume);
            Settings.SFXVolume = volume;
        }

        public void ToggleFullScreen(bool value)
        {
            Screen.fullScreen = fullScreenToggle.isOn;
            Settings.FullScreen = fullScreenToggle.isOn;
        }


        private void InitializeVariables()
        {
            if (musicVolumeSlider.transform.parent.gameObject.activeInHierarchy)
            {
                musicVolumeSlider.value = Settings.MusicVolume;
                SFXVolumeSlider.value = Settings.SFXVolume;
                fullScreenToggle.isOn = Settings.FullScreen;
            }
            
        }

        private void OnDisable() => _settings?.SaveSettings();
    }
}