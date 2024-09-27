using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private enum PlayerMode
    { 
        Player1,
        Player2
    }

    [Header("Player Controller")]
    [SerializeField] private PlayerMode playerMode;
    [SerializeField] private Attractor attractor;

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

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        _rigidbody.freezeRotation = true;
    }

    private void GetInputs()
    {
        switch (playerMode)
        {
            case PlayerMode.Player1:
                _horizontalInput = Input.GetKey(KeyCode.A) ? -1f : Input.GetKey(KeyCode.D) ? 1f : 0f;
                _isJumping = Input.GetKey(KeyCode.W);

                if (Input.GetKeyDown(KeyCode.S))
                    attractor.StartAttracting();
                else if(Input.GetKeyUp(KeyCode.S))
                    attractor.StopAttracting();
                break;

            case PlayerMode.Player2:
                _horizontalInput = Input.GetKey(KeyCode.LeftArrow) ? -1f : Input.GetKey(KeyCode.RightArrow) ? 1f : 0f;
                _isJumping = Input.GetKey(KeyCode.UpArrow);

                if (Input.GetKeyDown(KeyCode.DownArrow))
                    attractor.StartAttracting();
                else if (Input.GetKeyUp(KeyCode.DownArrow))
                    attractor.StopAttracting();
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
