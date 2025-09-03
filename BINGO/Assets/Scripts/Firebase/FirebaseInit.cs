using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using System;

public class FirebaseInit : MonoBehaviour
{
    public static FirebaseInit Instance;

    private Firebase.Auth.FirebaseAuth auth;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                Debug.Log("Firebase is ready!");
                auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
            }
            else
            {
                Debug.LogError("Could not resolve Firebase dependencies: " + dependencyStatus);
            }
        });
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
