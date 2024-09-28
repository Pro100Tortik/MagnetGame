using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance => _instance;
    private static AudioManager _instance;
    [SerializeField] private AudioClip[] music;
    private AudioSource _audioSource;
    private AudioSource _musicSource;
    private int _currentMusicID = 0;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        _musicSource = GetComponent<AudioSource>();

        PlayNextTrack();

        _audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip)
    {
        _audioSource.pitch = Random.Range(0.95f, 1.05f);
        _audioSource.PlayOneShot(clip);
    }

    void PlayNextTrack()
    {
        if (music.Length == 0) return;

        _musicSource.clip = music[_currentMusicID]; // Устанавливаем текущий трек
        _musicSource.Play(); // Воспроизводим трек
        _currentMusicID = (_currentMusicID + 1) % music.Length; // Увеличиваем индекс, зацикливая его
        StartCoroutine(WaitForTrackToEnd()); // Запускаем корутину для ожидания конца трека
    }

    private IEnumerator WaitForTrackToEnd()
    {
        yield return new WaitForSeconds(_musicSource.clip.length); // Ждем, пока трек закончится
        PlayNextTrack(); // Проигрываем следующий трек
    }
}
