using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResB : MonoBehaviour
{
    public void OnClickStartButton()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton0) || (Input.GetKeyDown(KeyCode.Space)))
        {
            SceneManager.LoadScene("Game");
        }
    }
}
