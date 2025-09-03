using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginManager : MonoBehaviour
{
    public static LoginManager Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void PlayAsGuest()
    {
        if (PlayerPrefs.HasKey(PlayerPrefsStrings.USERNAME))
        {
            // Retrive Data from PlayerPrefs
            RuntimeDBManager.instance.UserName = PlayerPrefs.GetString(PlayerPrefsStrings.USERNAME);
        }
        else
        {
            string userName = GenerateUserName();
            PlayerPrefs.SetString(PlayerPrefsStrings.USERNAME, userName);
            //Instatiate Data
            RuntimeDBManager.instance.UserName = PlayerPrefs.GetString(PlayerPrefsStrings.USERNAME);

        }

        //Load Next Scene
        LoadingScreen.Singleton.LoadScene(LoadingScreen.SCENE_MENU);
    }
    public string  GenerateUserName()
    {
        string name = "Guest" + Random.Range(10000, 99999);
        return name;    
    }
}
