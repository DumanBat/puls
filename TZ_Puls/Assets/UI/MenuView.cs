using UnityEngine;
using UnityEngine.UI;

public class MenuView : MonoBehaviour
{
    public Button start;
    public Button jump;

    [Header("Header")]
    public Transform header;
    public Button pause;
    public Button about;

    [Header("PopUp")]
    public Transform popUpWindow;
    public Transform outfield;
    public Button resume;
    public Button playAgain;
    public Button continueGame;
    public Button exit;

    [Header("About")]
    public Transform aboutWindow;
    public Button closeAbout;

    public void ShowPopUp(bool val, bool isPause)
    {
        popUpWindow.gameObject.SetActive(val);
        outfield.gameObject.SetActive(val);
        header.gameObject.SetActive(!val);
        jump.gameObject.SetActive(!val);

        if (isPause)
        {
            resume.gameObject.SetActive(true);
            continueGame.gameObject.SetActive(false);
        }
        else
        {
            resume.gameObject.SetActive(false);
            continueGame.gameObject.SetActive(true);
        }
    }

    public void ShowAbout(bool val)
    {
        header.gameObject.SetActive(!val);
        aboutWindow.gameObject.SetActive(val);
        outfield.gameObject.SetActive(val);
    }
}
