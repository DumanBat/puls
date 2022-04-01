using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    private Transform _target;
    private Vector3 _offset;

    private void FixedUpdate()
    {
        if (_target == null) return;

        var targetPosOffset = new Vector3(_target.position.x, 0.0f, -10f);
        transform.position = targetPosOffset + _offset;
    }

    public void Init(Transform followTarget)
    {
        _target = followTarget;
        var targetPosOffest = new Vector3(_target.position.x, 0.0f, -10f);
        _offset = transform.position - targetPosOffest;
    }

    public void SetPosition(Vector3 pos) => transform.position = pos;
    public void MoveCamera(float amount) => transform.position += new Vector3(amount, 0.0f, 0.0f);
}
