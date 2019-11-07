using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScript : MonoBehaviour
{
    public bool startedLoading = false;
    public float pauseDelay = 0;

    void Update()
    {
        // Press the space key to start coroutine
        if (!startedLoading)
        {
            //pauseDelay += Time.deltaTime;
            //if (pauseDelay > 0.2f)
            {
                startedLoading = true;
                // Use a coroutine to load the Scene in the background
                StartCoroutine(LoadYourAsyncScene());
            }
        }
    }

    IEnumerator LoadYourAsyncScene()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}