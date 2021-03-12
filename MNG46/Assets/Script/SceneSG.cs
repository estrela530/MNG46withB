using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSG : MonoBehaviour
{
   
    public void OnClickButton()
    {
        if (Input.GetKeyDown("joystick button 0"))
        {
            SceneManager.LoadScene("Game");
        }

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    SceneManager.LoadScene("Game");
        //}
    }

    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Space))
        //{
        //    SceneManager.LoadScene("Game");
        //}
    }
}
