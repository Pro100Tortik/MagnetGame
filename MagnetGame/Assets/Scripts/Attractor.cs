using System.Threading.Tasks;
using UnityEngine;

public class Attractor : MonoBehaviour
{
    [SerializeField] private float attractionForce = 10f;
    [SerializeField] private float maxAttractionRange = 4f;
    [SerializeField] private float timeToGetMaxRange = 1.3f;
    [SerializeField] private AnimationCurve attractionCurve;
    [SerializeField] private LayerMask attractablesMask;
    private float _currentAttractionRange;
    private Rigidbody2D _mineRB;

    private void Awake() => _mineRB = GetComponent<Rigidbody2D>();

    public void StartAttracting() => DoMagic(_currentAttractionRange, maxAttractionRange, timeToGetMaxRange);

    public void StopAttracting() => DoMagic(_currentAttractionRange, 0, 0.4f);

    private void FixedUpdate()
    {
        if (_currentAttractionRange < 0.3f)
            return;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _currentAttractionRange, attractablesMask);
        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent(out Rigidbody2D rb))
            {
                if (rb == _mineRB)
                    continue;

                if (Vector2.Distance(rb.position, transform.position) < 1.0f)
                    continue;

                Vector2 direction = (transform.position - collider.transform.position).normalized;
                rb.AddForce(direction * attractionForce);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _currentAttractionRange);
    }

    private async void DoMagic(float startValue, float endValue, float time)
    {
        float timer = 0;

        while (timer < time)
        {
            float value = attractionCurve.Evaluate(timer / time);
            float range = Mathf.Lerp(startValue, endValue, value);
            _currentAttractionRange = range;
            timer += Time.deltaTime;
            await Task.Yield();
        }

        _currentAttractionRange = endValue;
    }
}
