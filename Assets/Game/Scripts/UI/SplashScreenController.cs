using UnityEngine;

public class SplashScreenController : MonoBehaviour
{
    [SerializeField] private ProgressBar _progressBar;
    
    public bool ScreenActive
    {
        set => gameObject.SetActive(value);
    }
    
    public bool ProgressBarActive
    {
        set => _progressBar.gameObject.SetActive(value);
    }

    public void ChangeValueProgress(float value)
    {
        _progressBar.ChangeValue(value);
    }
}
