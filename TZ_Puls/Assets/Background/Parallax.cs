using UnityEngine;

public class Parallax : MonoBehaviour
{
    private Camera _cam;
    private Background _background;

    [SerializeField]
    private float _parallaxValue;
    private float _length;
    private float _startingPosition;

    private Vector2 _lastCameraPosition;

    private void Awake()
    {
        _background = GetComponent<Background>();
    }

    public void Init()
    {
        _cam = Camera.main;
        _lastCameraPosition = _cam.transform.position;

        _startingPosition = transform.position.x;
        _length = _background.GetLength();

        if (transform.childCount > 0)
        {
            transform.GetChild(0).position = new Vector2(transform.position.x - _length, transform.position.y);
            transform.GetChild(1).position = new Vector2(transform.position.x + _length, transform.position.y);
        }
    }

    void Update()
    {
        if (Vector2.Distance(_lastCameraPosition, _cam.transform.position) != 0)
            MoveParallax();
    }

    private void MoveParallax()
    {
        _lastCameraPosition = _cam.transform.position;
        var dist = _lastCameraPosition.x * _parallaxValue;
        var temp = _lastCameraPosition.x * (1 - _parallaxValue);

        transform.position = new Vector2(_startingPosition + dist, transform.position.y);

        if (temp > _startingPosition + _length)
            _startingPosition += _length;
        else if (temp < _startingPosition - _length)
            _startingPosition -= _length;
    }
}
