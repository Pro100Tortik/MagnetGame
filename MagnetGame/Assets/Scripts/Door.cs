using UnityEngine;

public class Door : AbstractInteraction
{
    [SerializeField] private AudioClip open;
    [SerializeField] private AudioClip close;
    [SerializeField] private float openHeight = 5f;
    private AudioSource source;
    private Vector2 _openedPos;
    private Vector2 _closedPos;

    private void Awake()
    {
        source = gameObject.AddComponent<AudioSource>();
        _openedPos = transform.localPosition + transform.up * openHeight;
        _closedPos = transform.localPosition;
    }

    public override void Open()
    {
        if (transform == null)
            return;

        transform.localPosition = _openedPos;
        source.pitch = Random.Range(0.95f, 1.05f);
        source.PlayOneShot(open);
    }

    public override void Close()
    {
        if (transform == null)
            return;

        transform.localPosition = _closedPos;
        source.pitch = Random.Range(0.95f, 1.05f);
        source.PlayOneShot(close);
    }

    public override void Interact() { }
}
