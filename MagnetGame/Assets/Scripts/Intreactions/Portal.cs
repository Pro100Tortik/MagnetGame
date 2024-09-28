using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private Transform destination;

    private void OnDrawGizmos()
    {
        if (destination == null)
            return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, destination.position);
        Gizmos.DrawWireCube(destination.position, Vector3.one);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Rigidbody2D rb))
        {
            rb.position = destination.position;
        }
    }
}
