using UnityEngine;

public class PressureButton : MonoBehaviour
{
    [SerializeField] private AudioClip on;
    [SerializeField] private AudioClip off;
    [SerializeField] private AbstractInteraction interactable;
    [SerializeField] private float massToActivate = 1f;
    private float _currentMass;
    private bool _activated;
    private AudioSource source;

    private void Awake() => source = gameObject.AddComponent<AudioSource>();

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out Rigidbody2D rb))
        {
            _currentMass += rb.mass;
        }

        if (!_activated && _currentMass >= massToActivate)
        {
            interactable?.Interact();
            interactable?.Open();
            source.pitch = Random.Range(0.95f, 1.05f);
            source.PlayOneShot(on);
            _activated = true;
            Debug.Log("Activated");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out Rigidbody2D rb))
        {
            _currentMass -= rb.mass;
        }

        if (_activated && _currentMass < massToActivate)
        {
            interactable?.Close();
            _activated = false;
            source.pitch = Random.Range(0.95f, 1.05f);
            source.PlayOneShot(off);
            Debug.Log("Deactivated");
        }
    }
}
