using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.Identity.Client;
using System.Threading.Tasks;
using System;
using PlayFab;
using PlayFab.ClientModels;

public class Auth : MonoBehaviour
{

    public void Start()
    {
        // Note: Setting title Id here can be skipped if you have set the value in Editor Extensions already.
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            PlayFabSettings.staticSettings.TitleId = "144"; // Please change this value to your own titleId from PlayFab Game Manager
        }
        var request = new LoginWithGoogleAccountRequest {
            //ServerAuthCode
            TitleId = PlayFabSettings.staticSettings.TitleId,


          };
        PlayFabClientAPI.LoginWithGoogleAccount(request, OnLoginSuccess, OnLoginFailure);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Congratulations, you made your first successful API call!");
    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with your first API call.  :(");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }
}
