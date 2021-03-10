using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSG : MonoBehaviour
{
   
    public void OnClickStartButton()
    {
        SceneManager.LoadScene("Game");
    }

    void Update()
    {

        SceneChange();
    }

    //シーン切り替え用
    void SceneChange()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        if (Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            SceneManager.LoadScene("Game");
        }


    }
}
