using UnityEngine;

public class Door : AbstractInteraction
{
    [SerializeField] private AudioClip open;
    [SerializeField] private AudioClip close;
    [SerializeField] private float openHeight = 5f;
    private AudioSource source;

    private void Awake() => source = gameObject.AddComponent<AudioSource>();

    public override void Open()
    {
        transform.localPosition += new Vector3(0, openHeight, 0);
        source.pitch = Random.Range(0.95f, 1.05f);
        source.PlayOneShot(open);
    }

    public override void Close()
    {
        transform.localPosition -= new Vector3(0, openHeight, 0);
        source.pitch = Random.Range(0.95f, 1.05f);
        source.PlayOneShot(close);
    }

    public override void Interact() { }
}
