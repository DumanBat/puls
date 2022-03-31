using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static readonly string PLATFORM = "Platform";

    private PlayerView _playerView;
    private Rigidbody2D _rb;
    private CircleCollider2D _collider;
    private LayerMask _platformMask;

    private float _speed = 5.0f;
    private float _jumpForce = 500.0f;
    private float _gravitySwitchCooldown = 2.0f;
    private float _lastGravitySwitchTime = -99.0f;

    private void Awake()
    {
        _playerView = GetComponent<PlayerView>();
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponentInChildren<CircleCollider2D>();
        _platformMask = LayerMask.GetMask(PLATFORM);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!IsGrounded())
                SwitchGravity();
            else
                Jump();
        }
    }

    private void FixedUpdate()
    {
        _rb.position += Vector2.right * _speed * Time.deltaTime;
    }

    private void SwitchGravity()
    {
        if (_lastGravitySwitchTime + _gravitySwitchCooldown > Time.time)
            return;

        StartCoroutine(_playerView.FillCooldownProgressBar(_gravitySwitchCooldown));
        _playerView.FlipProgressBar();

        _lastGravitySwitchTime = Time.time;
        _rb.gravityScale *= -1;
    }

    private bool IsFloor() => transform.position.y < 0;

    private void Jump()
    {
        if (!IsGrounded())
            return;

        var gravityDirection = IsFloor() ? 1 : -1;
        _rb.AddForce(new Vector2(0.0f, _jumpForce * gravityDirection));
    }

    private bool IsGrounded()
    {
        var heightOffset = 0.1f;
        var gravityDirection = IsFloor()
            ? Vector2.down
            : Vector2.up;
        RaycastHit2D raycastHit = Physics2D.BoxCast(_collider.bounds.center, _collider.bounds.size, 0f, gravityDirection, heightOffset, _platformMask);

        return raycastHit.collider != null;
    }

    public void Unload()
    {
        _lastGravitySwitchTime = -99.0f;
    }
}
