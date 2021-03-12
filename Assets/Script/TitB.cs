using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitB : MonoBehaviour
{
    public void OnClickStartButton()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton0) || (Input.GetKeyDown(KeyCode.Space)))
        {
            SceneManager.LoadScene("Title");
        }
    }
}
