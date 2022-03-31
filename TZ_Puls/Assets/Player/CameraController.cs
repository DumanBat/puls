using UnityEngine;

public class CameraController : MonoBehaviour
{
    //TEMP
    public Transform target;

    private Vector3 _offset;
    
    //TEMP
    private void Start()
    {
        Init(target);
    }
    ///
    public void Init(Transform followTarget)
    {
        target = followTarget;
        var targetPosOffest = new Vector3(target.position.x, 0.0f, -10f);
        transform.position = targetPosOffest;
        _offset = transform.position - targetPosOffest;
    }

    private void FixedUpdate()
    {
        if (target == null) return;

        var targetPosOffset = new Vector3(target.position.x + 7.0f, 0.0f, -10f);
        transform.position = targetPosOffset + _offset;
    }
}
