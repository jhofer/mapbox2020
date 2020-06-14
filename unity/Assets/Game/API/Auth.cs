using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.Identity.Client;
using System.Threading.Tasks;
using System;
using GooglePlayGames.BasicApi;
using GooglePlayGames;

public class Auth : BaseSingleton<Auth>
{

    [SerializeField]
    public string idToken;
    private TaskCompletionSource<string> promise = new TaskCompletionSource<string>();

    public void Start()
    {
      
        if (Application.isEditor)
        {
            Debug.Log("In editor Mode: Use idToken in the editor: \n" + idToken);
            AuthenctationSucceed();
        }
        else
        {
            Debug.Log("Real mode use PlayGamesClientConfiguration");

            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
             // enables saving game progress.
             // .EnableSavedGames()
             // registers a callback to handle game invitations received while the game is not running.
             //  .WithInvitationDelegate(< callback method >)
             // registers a callback for turn based match notifications received while the
             // game is not running.
             // .WithMatchDelegate(< callback method >)
             // requests the email address of the player be available.
             // Will bring up a prompt for consent.
             .RequestEmail()
             // requests a server auth code be generated so it can be passed to an
             //  associated back end server application and exchanged for an OAuth token.
             .RequestServerAuthCode(false)
             // requests an ID token be generated.  This OAuth token can be used to
             //  identify the player to other services such as Firebase.
             .RequestIdToken()
             .Build();

            PlayGamesPlatform.InitializeInstance(config);
            // recommended for debugging:
            PlayGamesPlatform.DebugLogEnabled = true;
            // Activate the Google Play Games platform
            PlayGamesPlatform.Activate();



            //PlayGamesClientConfiguration.Builder.RequestIdToken();
            PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptAlways, (success) =>
            {
                Debug.Log("status: " + success);
                if (SignInStatus.Success.Equals(success))
                {
                    AuthenctationSucceed();
                }

                else
                {
                    StartCoroutine(WaitForAuthenticationCoroutine());
                }
            });
        }
    }

    private void AuthenctationSucceed()
    {
       
        Debug.Log(Social.localUser.userName + " logged in");
        this.IsLoggedIn = true;
        if (!promise.TrySetResult(ReadToken())) {
            throw new Exception("Could not Resolve promise");
        };
    }

    private const float AuthenticationWaitTimeSeconds = 10;

    public bool IsLoggedIn { get; private set; }

    private IEnumerator WaitForAuthenticationCoroutine()
    {
        var startTime = Time.realtimeSinceStartup;

        while (!Social.localUser.authenticated)
        {
            if (Time.realtimeSinceStartup - startTime > AuthenticationWaitTimeSeconds)
            {
                // X seconds have passed and we are still not authenticated, time to give up.
                break;
            }

            yield return null;
        }

        if (Social.localUser.authenticated)
        {
            AuthenctationSucceed();
        }
        else
        {
            AuthenticationFailed();

        }
    }

    private void AuthenticationFailed()
    {
        Debug.LogError("Login Failed");
    }

    public Task<string> GetToken()
    {
       
        return promise.Task;
    }

    private string ReadToken()
    {

        if (Application.isEditor)
        {
            return this.idToken;
        }
        else
        {
            return ((PlayGamesLocalUser)Social.localUser).GetIdToken();
        }


    }
}
