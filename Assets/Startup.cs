using UnityEngine;
using UnityEngine.SceneManagement;

public class Startup : MonoBehaviour
{
    public string MarsSceneName;
    public string NoMarsSceneName;

    public AsyncOperation CurrentOperation;

    public void LoadScene(string sceneName)
    {
        if (CurrentOperation != null && !CurrentOperation.isDone)
        {
            return;
        }

        CurrentOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }

    public void UnloadScene(string sceneName)
    {
        if (CurrentOperation != null && !CurrentOperation.isDone)
        {
            return;
        }

        CurrentOperation = SceneManager.UnloadSceneAsync(sceneName);
    }

    public void LoadMarsScene()
    {
        LoadScene(MarsSceneName);
    }

    public void UnloadMarsScene()
    {
        UnloadScene(MarsSceneName);
    }

    public void LoadNoMarsScene()
    {
        LoadScene(NoMarsSceneName);
    }

    public void UnloadNoMarsScene()
    {
        UnloadScene(NoMarsSceneName);
    }
}
