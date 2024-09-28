using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private enum PlayerMode
    { 
        None,
        Player1,
        Player2
    }

    [Header("Player Controller")]
    [SerializeField] private AudioClip[] deathSounds;
    [SerializeField] private ParticleSystem deathEffect;
    [SerializeField] private ParticleSystem groundParticles;
    [SerializeField] private PlayerMode playerMode;
    [SerializeField] private Attractor attractor;
    [SerializeField] private SpriteRenderer bodyRenderer;
    [SerializeField] private SpriteRenderer headRenderer;
    [SerializeField] private Animator headAnimator;
    [SerializeField] private Animator bodyAnimator;

    [SerializeField] private string walkStringBody;
    [SerializeField] private string walkStringHead;
    [SerializeField] private string idleStringBody;
    [SerializeField] private string idleStringHead;

    [Header("Controller Settings")]
    [SerializeField] private float acceleration = 20f;
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float jumpForce = 3f;
    [SerializeField] private float airAcceleration = 5f;
    [SerializeField] private float airControl = 0.6f;
    [SerializeField] private float friction = 6f;
    [SerializeField] private float maxSlopeAngle = 35f;
    [SerializeField] private float coyoteTime = 0.25f;

    private Rigidbody2D _rigidbody;
    private float _horizontalInput;
    private bool _isJumping;
    private Vector2 _velocity;
    private Vector2 _groundNormal;
    private bool _isGrounded;
    private float _coyoteTimer;
    private bool _right;

    private GameManager _gameManager;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        _rigidbody.freezeRotation = true;
    }

    private void Start() => _gameManager = GameManager.Instance;

    public void DIE()
    {
        playerMode = PlayerMode.None;
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        AudioManager.Instance.PlaySound(deathSounds.GetRandomElement());
        Destroy(gameObject);
    }

    private void GetInputs()
    {
        switch (playerMode)
        {
            case PlayerMode.Player1:
                if (!_gameManager.IsPaused)
                {
                    _horizontalInput = Input.GetKey(KeyCode.A) ? -1f : Input.GetKey(KeyCode.D) ? 1f : 0f;
                    _isJumping = Input.GetKey(KeyCode.W);
                }
                else
                {
                    _horizontalInput = 0;
                    _isJumping = false;
                    return;
                }

                if (Input.GetKeyDown(KeyCode.S))
                    attractor.StartAttracting();
                else if(Input.GetKeyUp(KeyCode.S))
                    attractor.StopAttracting();
                break;

            case PlayerMode.Player2:
                if (!_gameManager.IsPaused)
                {
                    _horizontalInput = Input.GetKey(KeyCode.LeftArrow) ? -1f : Input.GetKey(KeyCode.RightArrow) ? 1f : 0f;
                    _isJumping = Input.GetKey(KeyCode.UpArrow);
                }
                else
                {
                    _horizontalInput = 0;
                    _isJumping = false;
                    return;
                }

                if (Input.GetKeyDown(KeyCode.DownArrow))
                    attractor.StartAttracting();
                else if (Input.GetKeyUp(KeyCode.DownArrow))
                    attractor.StopAttracting();
                break;

            case PlayerMode.None:
                break;
        }
    }

    private void Update() => GetInputs();

    private void FixedUpdate()
    {
        _velocity = _rigidbody.velocity;

        ApplyFriction();

        _rigidbody.gravityScale = _isGrounded ? 0f : _rigidbody.velocity.y < -1f ? 2f : 1f;

        Move();

        if (_isGrounded && Mathf.Abs(_velocity.x) > 0.5f)
            groundParticles.Play();

        if (_horizontalInput != 0)
        {
            _right = _horizontalInput > 0;
            bodyAnimator.CrossFade(walkStringBody, 0f);
            headAnimator.CrossFade(walkStringHead, 0f);
        }
        else if (Mathf.Abs(_velocity.x) < 0.5f)
        {
            bodyAnimator.CrossFade(idleStringBody, 0f);
            headAnimator.CrossFade(idleStringHead, 0f);
        }

        headRenderer.flipX = _right;
        bodyRenderer.flipX = _right;

        Jump();

        _rigidbody.velocity = _velocity;

        if (_isGrounded)
            _coyoteTimer = coyoteTime;

        _isGrounded = false;

        DoCoyoteTime();
    }

    private void Move()
    {
        if (_isGrounded)
        {
            Vector3 projectedVel = Vector3.ProjectOnPlane(new Vector3(_horizontalInput, 0, 0), new Vector3(_groundNormal.x, _groundNormal.y));
            Vector2 modifiedInput = (Vector2)projectedVel;
            _velocity += modifiedInput * movementSpeed * acceleration * Time.deltaTime;
        }
        else
        {
            _velocity.x += _horizontalInput * movementSpeed * airAcceleration * airControl * Time.deltaTime;
        }
    }

    private void Jump() => _velocity.y = (_isGrounded || _coyoteTimer > 0) && _isJumping ? jumpForce : _velocity.y;

    private void ApplyFriction() => _rigidbody.drag = _isGrounded ? friction : 0f;

    private void DoCoyoteTime()
    {
        if (!_isGrounded)
        {
            if (_coyoteTimer > 0)
                _coyoteTimer -= Time.deltaTime;
            else
                _coyoteTimer = 0;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        foreach (ContactPoint2D contactPoint in collision.contacts)
        {
            if (contactPoint.normal.y >= maxSlopeAngle * Mathf.Deg2Rad)
            {
                _groundNormal = contactPoint.normal;
                _isGrounded = true;
                return;
            }
        }
    }
}
