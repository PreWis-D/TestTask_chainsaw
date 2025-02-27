using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private SplashScreenController _splashScreenController;

    private void Start()
    {
        LoadGame().Forget();
    }

    private async UniTaskVoid LoadGame()
    {
        _splashScreenController.ScreenActive = true;
        var index = SceneManager.GetActiveScene().buildIndex + 1;

        await LoadNextScene(index);

        var newScene = SceneManager.GetSceneByBuildIndex(index);
        SceneManager.SetActiveScene(newScene);
        _splashScreenController.ScreenActive = false;
    }

    private async UniTask LoadNextScene(int index)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);

        asyncLoad.allowSceneActivation = false;

        var currentFrame = 0;
        while (asyncLoad.progress < 0.9f)
        {
            currentFrame++;
            _splashScreenController.ChangeValueProgress(Mathf.Lerp(0f, 1f, currentFrame));

            await UniTask.NextFrame();
        }

        asyncLoad.allowSceneActivation = true;

        while (asyncLoad.isDone == false)
        {
            var value = Mathf.Lerp(0f, 1f, asyncLoad.progress);
            _splashScreenController.ChangeValueProgress(value);

            await UniTask.NextFrame();
        }
    }
}