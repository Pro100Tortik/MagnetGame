using UnityEngine;
using System;

public class Collectables : MonoBehaviour
{
    public event Action Collect;
    [SerializeField] private AudioClip sound;
    [SerializeField] private bool plus = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (plus ? collision.CompareTag("Plus") : collision.CompareTag("Minus"))
        {
            gameObject.SetActive(false);
            Collect?.Invoke();
            AudioManager.Instance.PlaySound(sound);
        }
    }
}
