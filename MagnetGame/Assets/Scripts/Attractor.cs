using System.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class Attractor : MonoBehaviour
{
    [SerializeField] private bool isBlackHole = false;
    [SerializeField] private bool isMagnet = false;
    [SerializeField] private bool attractOnlyPlus = false;

    [SerializeField] private AudioSource attractorSource;
    [SerializeField] private AudioClip expand;
    [SerializeField] private AudioClip compress;
    [SerializeField] private float attractionForce = 10f;
    [SerializeField] private float maxAttractionRange = 4f;
    [SerializeField] private float timeToGetMaxRange = 1.3f;
    [SerializeField] private AnimationCurve attractionCurve;
    [SerializeField] private LayerMask attractablesMask;
    [SerializeField] private Transform radiusVisualizer;
    private float _currentAttractionRange;
    private Rigidbody2D _mineRB;
    private CancellationTokenSource _cancellationTokenSource;

    private void Awake()
    {
        _mineRB = GetComponent<Rigidbody2D>();
        radiusVisualizer.localScale = Vector2.zero;

        if (isBlackHole || isMagnet)
            StartAttracting();
    }

    public void StartAttracting()
    {
        _cancellationTokenSource?.Cancel();
        var token = _cancellationTokenSource = new CancellationTokenSource();
        attractorSource.pitch = Random.Range(0.95f, 1.05f);
        attractorSource.PlayOneShot(expand);
        DoMagic(token, _currentAttractionRange, maxAttractionRange, timeToGetMaxRange);
    }

    public void StopAttracting()
    {
        _cancellationTokenSource?.Cancel();
        var token = _cancellationTokenSource = new CancellationTokenSource();
        attractorSource.pitch = Random.Range(0.95f, 1.05f);
        attractorSource.PlayOneShot(compress);
        DoMagic(token, _currentAttractionRange, 0, 0.4f);
    }

    private void FixedUpdate()
    {
        if (_currentAttractionRange < 0.3f)
            return;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _currentAttractionRange, attractablesMask);
        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent(out Rigidbody2D rb))
            {
                if (_mineRB != null && rb == _mineRB)
                    continue;

                if (isMagnet)
                {
                    if (attractOnlyPlus && !rb.CompareTag("Plus"))
                        continue;
                    else if (!attractOnlyPlus && !rb.CompareTag("Minus"))
                        continue;
                }

                if (Vector2.Distance(rb.position, transform.position) < 2.0f)
                    continue;

                Vector2 direction = (transform.position - collider.transform.position).normalized;
                rb.AddForce(direction * attractionForce);
            }
        }
    }

    private async void DoMagic(CancellationTokenSource token, float startValue, float endValue, float time)
    {
        float timer = 0;

        while (timer < time)
        {
            if (token.IsCancellationRequested)
                break;

            float value = attractionCurve.Evaluate(timer / time);
            float range = Mathf.Lerp(startValue, endValue, value);
            _currentAttractionRange = range;
            timer += Time.fixedDeltaTime;

            radiusVisualizer.localScale = new Vector2(_currentAttractionRange * 2, _currentAttractionRange * 2);

            await Task.Delay((int)(Time.fixedDeltaTime * 1000));
        }

        if (token.IsCancellationRequested)
            return;

        _currentAttractionRange = endValue;
    }

    private void OnDestroy() => _cancellationTokenSource?.Cancel();
}
