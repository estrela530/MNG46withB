using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSG : MonoBehaviour
{
   
    public void OnClickStartButton()
    {
        if(Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            SceneManager.LoadScene("Game");
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("Game");
        }
    }
}
