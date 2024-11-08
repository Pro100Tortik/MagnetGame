using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Door : AbstractInteraction
{
    [SerializeField] private AudioClip open;
    [SerializeField] private AudioClip close;
    [SerializeField] private float openHeight = 5f;
    private Vector2 _openedPos;
    private Vector2 _closedPos;
    private CancellationTokenSource _source;

    private void Awake()
    {
        _openedPos = transform.localPosition + transform.up * openHeight;
        _closedPos = transform.localPosition;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position + transform.up * openHeight, transform.localScale);
    }

    public override void Open()
    {
        _source?.Cancel();
        _source = new CancellationTokenSource();

        DoMagic(_source, _openedPos);

        AudioManager.Instance.PlaySound(open);
    }

    public override void Close()
    {
        _source?.Cancel();
        _source = new CancellationTokenSource();

        DoMagic(_source, _closedPos);

        AudioManager.Instance.PlaySound(close);
    }

    public override void Interact() { }

    private void OnDestroy() => _source?.Cancel();

    private async void DoMagic(CancellationTokenSource token, Vector3 endValue)
    {
        float timer = 0;

        while (timer < 1)
        {
            if (token.IsCancellationRequested)
                break;

            //float value = attractionCurve.Evaluate(timer / 1);
            Vector3 pos = Vector3.Lerp(transform.localPosition, endValue, timer);
            transform.localPosition = pos;
            timer += Time.fixedDeltaTime;

            await Task.Delay((int)(Time.fixedDeltaTime * 1000));
        }

        if (token.IsCancellationRequested)
            return;

        transform.localPosition = endValue;
    }
}
