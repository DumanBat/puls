using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public PlayerController playerPrefab;
    private PlayerController _playerInstance;
    private LevelGenerator _levelGenerator;

    private Coroutine _routineHandler;

    private void Awake()
    {
        _levelGenerator = GetComponent<LevelGenerator>();
    }

    public void Init()
    {
        GameManager.Instance.backgroundController.Init();
        GameManager.Instance.menuController.Init();
        _routineHandler = StartCoroutine(AnimateBackground());
    }

    public void StartGame()
    {
        if (_routineHandler != null)
            StopCoroutine(_routineHandler);
        _playerInstance = Instantiate(playerPrefab);
        _playerInstance.Init(new Vector3(-7.0f, 0.0f, 0.0f));
        _playerInstance.onPlayerDie += EndGame;

        CameraController.Instance.SetPosition(new Vector3(0.0f, 0.0f, -10.0f));
        CameraController.Instance.Init(_playerInstance.transform);
        _levelGenerator.Init();
    }

    public void EndGame()
    {
        GameManager.Instance.menuController.ShowPopUp(true, false);
    }

    public void ContinueGame()
    {
        var playerPos = new Vector2(_playerInstance.transform.position.x, 0.0f);
        _playerInstance.Init(playerPos);
    }

    public IEnumerator AnimateBackground()
    {
        var waitForFixedUpdate = new WaitForFixedUpdate();
        while (true)
        {
            CameraController.Instance.MoveCamera(0.01f);
            yield return waitForFixedUpdate;
        }
    }

    public void Unload()
    {
        _levelGenerator.Unload();
        Destroy(_playerInstance.gameObject);
    }
}
