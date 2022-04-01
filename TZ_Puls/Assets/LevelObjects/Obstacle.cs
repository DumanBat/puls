using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private static readonly string PLATFORM = "Platform";

    private string _obstacleName;
    private BoxCollider2D _collider;
    private LayerMask _platformMask;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
        _platformMask = LayerMask.GetMask(PLATFORM);
    }

    public string GetObstacleName() => _obstacleName;
    public void SetObstaclename(string val) => _obstacleName = val;
    public bool IsGrounded(Vector2 direction)
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(_collider.bounds.center, _collider.bounds.size, 0f, direction, 0.05f, _platformMask);

        return raycastHit.collider != null;
    }
}
