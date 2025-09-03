using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeDBManager : MonoBehaviour
{
    public static RuntimeDBManager instance;

    public event Action OnNoUserName;
    public bool IsMultiplayer;
    public int Coins;
    public int Tickets;
    public string UserName;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void Start()
    {
        if (!PlayerPrefs.HasKey(PlayerPrefsStrings.USERNAME))
        {
            OnNoUserName?.Invoke();
        }
    }

}
