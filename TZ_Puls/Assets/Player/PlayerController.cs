using System;
using System.Collections;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    private static readonly string PLATFORM = "Platform";
    private static readonly string OBSTACLE = "Obstacle";
    private static readonly string DEATH = "Death";
    private static readonly int IS_JUMPING = Animator.StringToHash("IsJumping");
    private static readonly int STICK = Animator.StringToHash("Stick");
    private static readonly int DIE = Animator.StringToHash("Die");
    private static readonly int SPAWN = Animator.StringToHash("Spawn");

    private PlayerView _playerView;
    private Rigidbody2D _rb;
    private Animator _animator;
    private CircleCollider2D _collider;
    private LayerMask _platformMask;

    private bool _isActive;
    private float _speed = 5.0f;
    private float _jumpForce = 500.0f;
    private float _gravityScale = 2.0f;
    private float _gravitySwitchCooldown = 1.5f;
    private float _lastGravitySwitchTime = -99.0f;

    public Action onPlayerDie;

    private void Awake()
    {
        _playerView = GetComponent<PlayerView>();
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
        _collider = GetComponentInChildren<CircleCollider2D>();
        _platformMask = LayerMask.GetMask(PLATFORM);
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.menuController.IsPaused())
            return;
        if (!_isActive)
            return;

        _rb.position += Vector2.right * _speed * Time.deltaTime;

        if (_rb.position.y < -6.0f || _rb.position.y > 6.0f)
            StartCoroutine(Die());
    }

    public void Init(Vector3 position)
    {
        _rb.gravityScale = _gravityScale;
        _rb.transform.position = position;
        _animator.SetTrigger(SPAWN);
        _playerView.root.gameObject.SetActive(true);
        _isActive = true;
    }

    public void FreezePosition(bool val)
    {
        if (!val)
            _rb.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
        else
            _rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
    }

    public void SetActiveAnimation(bool val) => _animator.enabled = val;
    public Vector3 GetPosition() => _rb.position;
    private bool IsFloor() => transform.position.y < 0;
    private bool IsGrounded()
    {
        var heightOffset = 0.1f;
        var gravityDirection = IsFloor()
            ? Vector2.down
            : Vector2.up;
        RaycastHit2D raycastHit = Physics2D.BoxCast(_collider.bounds.center, _collider.bounds.size, 0f, gravityDirection, heightOffset, _platformMask);

        return raycastHit.collider != null;
    }

    public void Jump()
    {
        if (!IsGrounded())
            SwitchGravity();
        else
            StartCoroutine(JumpRoutine());
    }

    private void SwitchGravity()
    {
        if (_lastGravitySwitchTime + _gravitySwitchCooldown > Time.time)
            return;

        StartCoroutine(_playerView.FillCooldownProgressBar(_gravitySwitchCooldown));
        _playerView.FlipProgressBar();

        _lastGravitySwitchTime = Time.time;
        _rb.gravityScale *= -1;
        _animator.SetTrigger(STICK);
    }    

    private WaitForSeconds _waitForJumpStart = new WaitForSeconds(0.2f);
    private IEnumerator JumpRoutine()
    {
        if (!IsGrounded())
            yield break;

        var gravityDirection = IsFloor() ? 1 : -1;
        _rb.AddForce(new Vector2(0.0f, _jumpForce * gravityDirection));

        _animator.SetBool(IS_JUMPING, true);
        yield return _waitForJumpStart;
        yield return new WaitUntil(() => IsGrounded());

        _animator.SetBool(IS_JUMPING, false);
    }

    private IEnumerator Die()
    {
        _isActive = false;

        _playerView.root.gameObject.SetActive(false);
        _rb.gravityScale = 0.0f;
        _rb.velocity = Vector2.zero;
        _animator.SetTrigger(DIE);
        yield return new WaitUntil(() => !_animator.GetCurrentAnimatorStateInfo(0).IsName(DEATH));
        onPlayerDie?.Invoke();
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag(OBSTACLE))
            StartCoroutine(Die());
    }
}
