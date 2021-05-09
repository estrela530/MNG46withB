using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSG : MonoBehaviour
{
    int fadeCount;
    int fadeMax;
    bool isScene1ChangeFlag;
    bool isScene2ChangeFlag;

    [SerializeField, Header("フェードPrefab")]
    GameObject fadeManager;

    // Start is called before the first frame update
    void Start()
    {
        fadeCount = 0;
        fadeMax = 90;
        isScene1ChangeFlag = false;
        isScene2ChangeFlag = false;
    }


    public void OnClickStartStage1Button()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton0) || (Input.GetKeyDown(KeyCode.Space)))
        {
            isScene1ChangeFlag = true;
        }
    }

    public void OnClickStartStage2Button()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton0) || (Input.GetKeyDown(KeyCode.Space)))
        {
            isScene2ChangeFlag = true;
        }
    }


    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SceneManager.LoadScene("Game");
        }

        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    SceneManager.LoadScene("KubotaPlayer");
        //}

        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    SceneManager.LoadScene("Enemy Scene");
        //}

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
        {
            SceneManager.LoadScene("Game2");
        }

        if (isScene1ChangeFlag)
        {
            fadeManager.SetActive(true);
            fadeCount++;
            if (fadeCount >= fadeMax)
            {
                SceneManager.LoadScene("Game2");
            }
        }
        else if (isScene2ChangeFlag)
        {
            fadeManager.SetActive(true);
            fadeCount++;
            if (fadeCount >= fadeMax)
            {
                SceneManager.LoadScene("Game3");
            }

        }

    }
}
