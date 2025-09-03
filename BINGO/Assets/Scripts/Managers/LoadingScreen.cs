using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    public static string SCENE_LOGIN = "Login";
    public static string SCENE_MENU = "Menu";
    public static string SCENE_LOADING = "Loading";
    public static string SCENE_GAME = "Game";
    public static LoadingScreen Singleton;

    [SerializeField]
    private GameObject holder;

    private void Awake()
    {
        if(Singleton == null)
        {
            Singleton = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        holder.SetActive(false);
    }

    public void LoadScene(string sceneName)
    {
       StartCoroutine(LoadingCoroutinue(sceneName));    
    }

    private IEnumerator LoadingCoroutinue(string sceneName)
    {
        ShowHolder();

        // First load the loading scene
        yield return SceneManager.LoadSceneAsync(SCENE_LOADING);

        // Now load the target scene in background
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        asyncOp.allowSceneActivation = false;

        // Wait until it's almost ready
        while (asyncOp.progress < 0.9f)
        {
            // you can update progress bar here
            yield return null;
        }

        // Activate the loaded scene
        asyncOp.allowSceneActivation = true;

        // Wait until it finishes activation
        while (!asyncOp.isDone)
        {
            yield return null;
        }

        // Now unload the loading scene
        yield return SceneManager.UnloadSceneAsync(SCENE_LOADING);

        HideHolder();
    }

    private void HideHolder()
    {
        holder.SetActive(false);
    }

    private void ShowHolder()
    {
        holder.SetActive(true);
    }
}
