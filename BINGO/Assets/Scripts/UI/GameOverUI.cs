using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text gameResult;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetText(string text)
    {
        gameResult.text = text;
    }

    public void OnRestart()
    {
        LoadingScreen.Singleton.LoadScene(LoadingScreen.SCENE_GAME);
    }

    public void MainMenu()
    {
        LoadingScreen.Singleton.LoadScene(LoadingScreen.SCENE_MENU);
    }

    public void OnQuit()
    {
        Application.ExternalEval("location.reload();");
    }
}
