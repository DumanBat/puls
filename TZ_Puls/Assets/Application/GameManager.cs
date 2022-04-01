using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public BackgroundController backgroundController;
    public MenuController menuController;
    public LevelManager levelManager;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        levelManager.Init();
    }
}
