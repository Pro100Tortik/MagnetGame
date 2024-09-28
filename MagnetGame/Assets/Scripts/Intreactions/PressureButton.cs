using UnityEngine;

public class PressureButton : MonoBehaviour
{
    [SerializeField] private AudioClip on;
    [SerializeField] private AudioClip off;
    [SerializeField] private AbstractInteraction[] interactables;
    [SerializeField] private float massToActivate = 1f;
    private float _currentMass;
    private bool _activated;
    private AudioSource source;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out Rigidbody2D rb))
        {
            _currentMass += rb.mass;
        }

        if (!_activated && _currentMass >= massToActivate)
        {
            for (int i = 0; i < interactables.Length; i++)
            {
                interactables[i]?.Interact();
                interactables[i]?.Open();
            }

            source.pitch = Random.Range(0.95f, 1.05f);
            source.PlayOneShot(on);
            _activated = true;
            Debug.Log("Activated");
        }
    }

    private void OnDrawGizmos()
    {
        if (interactables == null)
            return;

        Gizmos.color = Color.yellow;
        for (int i = 0; i < interactables.Length; i++)
        {
            Gizmos.DrawLine(transform.position, interactables[i].transform.position);
        }
    }

    private void OnValidate()
    {
        if (source == null)
        {
            if (!TryGetComponent(out source))
                source = gameObject.AddComponent<AudioSource>();
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
            for (int i = 0; i < interactables.Length; i++)
            {
                interactables[i]?.Close();
            }

            _activated = false;
            source.pitch = Random.Range(0.95f, 1.05f);
            source.PlayOneShot(off);
            Debug.Log("Deactivated");
        }
    }
}
