using UnityEngine;

public class PressureButton : MonoBehaviour
{
    [SerializeField] private AudioClip on;
    [SerializeField] private AudioClip off;
    [SerializeField] private AbstractInteraction[] interactables;
    [SerializeField] private float massToActivate = 1f;
    private float _currentMass;
    private bool _activated;

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
                interactables[i].Open();
            }

            AudioManager.Instance.PlaySound(on);
            _activated = true;
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

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out Rigidbody2D rb))
        {
            _currentMass -= rb.mass;
        }

        if (_activated && _currentMass < massToActivate)
        {
            for (int j = 0; j < interactables.Length; j++)
            {
                interactables[j].Close();
            }

            _activated = false;
            AudioManager.Instance.PlaySound(off);
        }
    }
}
