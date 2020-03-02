using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.Identity.Client;
using System.Threading.Tasks;
using System;
using GooglePlayGames.BasicApi;
using GooglePlayGames;

public class Auth : MonoBehaviour
{
    [SerializeField]
    public string GOOGLE_CLIENTID;
    
    public string idToken;

    public void Start()
    {
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
         //.RequestServerAuthCode(true)
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
        Social.localUser.Authenticate((bool success) => {
            if (success)
            {
                Debug.Log(Social.localUser.userName + " logged in");
                this.idToken = ((PlayGamesLocalUser)Social.localUser).GetIdToken();
                Debug.Log("ID TOKEN " + idToken);

            }
            else
            {
                Debug.LogError("Login Failed");
            }
        });
    }

    
}
