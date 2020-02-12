using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginScript : MonoBehaviour
{

    public void Login()
    {
        Debug.Log("Login clicked");
        SceneManager.LoadScene("GameWorld", LoadSceneMode.Single);
        Debug.Log("scene loaded");
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
