using System.Collections;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public Transform root;

    [SerializeField]
    private ProgressBar _progressBar;

    public IEnumerator FillCooldownProgressBar(float duration)
    {
        _progressBar.gameObject.SetActive(true);
        _progressBar.SetValue(0);
        float time = 0.0f;
        while (time < duration)
        {
            _progressBar.IncrementValue(time / duration);
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        _progressBar.gameObject.SetActive(false);
    }

    public void FlipProgressBar()
    {
        var localPos = _progressBar.transform.localPosition;
        _progressBar.transform.localPosition = new Vector3(localPos.x, localPos.y * (-1), localPos.z);
    }
}
