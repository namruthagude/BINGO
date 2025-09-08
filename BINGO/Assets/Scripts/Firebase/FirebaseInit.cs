using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using System;
using Google;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Extensions;

public class FirebaseInit : MonoBehaviour
{
    public static FirebaseInit Instance;

    private Firebase.Auth.FirebaseAuth auth;
    public GoogleSignInConfiguration config;
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
        config = new GoogleSignInConfiguration
        {
            WebClientId = "652914484628-h8acnd85ja87s322vjrqpc6llksor2s4.apps.googleusercontent.com",
            RequestIdToken = true
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoginWithGoogle()
    {
        GoogleSignIn.Configuration = config;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestEmail = true;

        GoogleSignIn.DefaultInstance.SignIn().ContinueWithOnMainThread(OnGoogleSignIn);
    }

    private void OnGoogleSignIn(Task<GoogleSignInUser> task)
    {
        if(task.IsFaulted || task.IsCanceled)
        {
            Debug.LogError("Google signin failed" + task.Exception);
            return;
        }
        SigninToFirebase(task.Result.IdToken);
    }

    private async void SigninToFirebase(string idToken)
    {
        var credential = GoogleAuthProvider.GetCredential(idToken, null);
        var result = await auth.SignInWithCredentialAsync(credential);
        Debug.Log("Firebase User" + result.DisplayName);
    }
}
