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
        _offset = new Vector3(target.position.x + 8.0f, 0.0f, -10f);
        transform.position = _offset;
    }

    private void Update()
    {
        if (target == null) return;

        var targetPosition = new Vector3(target.position.x, 0.0f, -10f);
        transform.position = targetPosition + _offset;
    }
}
