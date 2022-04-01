using UnityEngine;

public class MenuController : MonoBehaviour
{
    private MenuView _menuView;
    private bool _isPaused;
    public bool IsPaused() => _isPaused;

    private void Awake()
    {
        _menuView = GetComponent<MenuView>();
        _menuView.start.onClick.AddListener(() =>
        {
            _menuView.start.gameObject.SetActive(false);
            _menuView.pause.gameObject.SetActive(true);
            _menuView.jump.gameObject.SetActive(true);
            GameManager.Instance.levelManager.StartGame();
        });
        _menuView.jump.onClick.AddListener(() => PlayerController.Instance.Jump());
        _menuView.pause.onClick.AddListener(() =>
        {
            PlayerController.Instance.FreezePosition(true);
            PlayerController.Instance.SetActiveAnimation(false);
            _isPaused = true;
            ShowPopUp(true, true);
        });
        _menuView.about.onClick.AddListener(() => 
        {
            ShowAbout(true);
        });

        _menuView.resume.onClick.AddListener(() =>
        {
            PlayerController.Instance.FreezePosition(false);
            PlayerController.Instance.SetActiveAnimation(true);
            _isPaused = false;
            ShowPopUp(false, true);
        });
        _menuView.playAgain.onClick.AddListener(() =>
        {
            GameManager.Instance.levelManager.Unload();
            _isPaused = false;
            ShowPopUp(false, false);
            GameManager.Instance.levelManager.StartGame();
        });
        _menuView.continueGame.onClick.AddListener(() =>
        {
            ShowPopUp(false, false);
            GameManager.Instance.levelManager.ContinueGame();
        });
        _menuView.exit.onClick.AddListener(() => Application.Quit());
        _menuView.closeAbout.onClick.AddListener(() =>
        {
            ShowAbout(false);
        });
    }

    public void Init()
    {
        _menuView.about.gameObject.SetActive(true);
        _menuView.start.gameObject.SetActive(true);
    }

    public void ShowPopUp(bool val, bool isPause)
    {
        _menuView.ShowPopUp(val, isPause);
    }

    public void ShowAbout(bool val)
    {
        _menuView.ShowAbout(val);
    }
}
