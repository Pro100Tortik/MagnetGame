using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance => _instance;
    private static AudioManager _instance;
    [SerializeField] private AudioClip[] music;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource audioSource;
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

        PlayNextTrack();
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.PlayOneShot(clip);
    }

    void PlayNextTrack()
    {
        if (music.Length == 0) return;

        musicSource.clip = music[_currentMusicID]; // ������������� ������� ����
        musicSource.Play(); // ������������� ����
        _currentMusicID = (_currentMusicID + 1) % music.Length; // ����������� ������, ���������� ���
        StartCoroutine(WaitForTrackToEnd()); // ��������� �������� ��� �������� ����� �����
    }

    private IEnumerator WaitForTrackToEnd()
    {
        yield return new WaitForSeconds(musicSource.clip.length); // ����, ���� ���� ����������
        PlayNextTrack(); // ����������� ��������� ����
    }
}
