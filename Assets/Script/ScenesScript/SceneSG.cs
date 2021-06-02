using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSG : MonoBehaviour
{
    int fadeCount;
    int fadeMax;
    public bool isScene1ChangeFlag;
    public bool isScene2ChangeFlag;
    public bool isScene3ChangeFlag;
    public bool isScene4ChangeFlag;
    public bool isScene5ChangeFlag;
    public bool isScene6ChangeFlag;

    bool thisIsAlreadyStage1ClearFlag = IsAlreadyStage1.isAlreadyStage1ClearFlag;

    public static bool isSceneMoveStoGFlag;

    [SerializeField, Header("フェードPrefab")]
    GameObject fadeManager;

    // Start is called before the first frame update
    void Start()
    {
        fadeCount = 0;
        fadeMax = 90;
        isScene1ChangeFlag = false;
        isScene2ChangeFlag = false;
        isScene3ChangeFlag = false;
        isScene4ChangeFlag = false;
        isScene5ChangeFlag = false;
        isScene6ChangeFlag = false;
        isSceneMoveStoGFlag = false;

    }


    public void OnClickStartStage1Button()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton0) || (Input.GetKeyDown(KeyCode.Return)))
        {
            isScene1ChangeFlag = true;
        }
    }

    public void OnClickStartStage2Button()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton0) || (Input.GetKeyDown(KeyCode.Return)) && thisIsAlreadyStage1ClearFlag)
        {
            isScene2ChangeFlag = true;
        }
    }

    public void OnClickStartStage3Button()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton0) || (Input.GetKeyDown(KeyCode.Return)) && thisIsAlreadyStage1ClearFlag)
        {
            isScene3ChangeFlag = true;
        }
    }

    public void OnClickStartStage4Button()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton0) || (Input.GetKeyDown(KeyCode.Return)) && thisIsAlreadyStage1ClearFlag)
        {
            isScene4ChangeFlag = true;
        }
    }
    public void OnClickStartStage5Button()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton0) || (Input.GetKeyDown(KeyCode.Return)) && thisIsAlreadyStage1ClearFlag)
        {
            isScene5ChangeFlag = true;
        }
    }

    public void OnClickStartStage6Button()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton0) || (Input.GetKeyDown(KeyCode.Return)) && thisIsAlreadyStage1ClearFlag)
        {
            isScene6ChangeFlag = true;
        }
    }


    void FixedUpdate()
    {
        if (isScene1ChangeFlag)
        {
            isSceneMoveStoGFlag = true;
            fadeManager.SetActive(true);
            fadeCount++;
            if (fadeCount >= fadeMax)
            {
                SceneManager.LoadScene("Game1");
            }
        }
        else if (isScene2ChangeFlag)
        {
            isSceneMoveStoGFlag = true;
            fadeManager.SetActive(true);
            fadeCount++;
            if (fadeCount >= fadeMax)
            {
                SceneManager.LoadScene("Game2");
            }
        }
        else if (isScene3ChangeFlag)
        {
            isSceneMoveStoGFlag = true;
            fadeManager.SetActive(true);
            fadeCount++;
            if (fadeCount >= fadeMax)
            {
                SceneManager.LoadScene("Game3");
            }
        }
        else if (isScene4ChangeFlag)
        {
            isSceneMoveStoGFlag = true;
            fadeManager.SetActive(true);
            fadeCount++;
            if (fadeCount >= fadeMax)
            {
                SceneManager.LoadScene("Game4");
            }
        }
        else if (isScene5ChangeFlag)
        {
            isSceneMoveStoGFlag = true;
            fadeManager.SetActive(true);
            fadeCount++;
            if (fadeCount >= fadeMax)
            {
                SceneManager.LoadScene("Game5");
            }
        }
        else if (isScene6ChangeFlag)
        {
            isSceneMoveStoGFlag = true;
            fadeManager.SetActive(true);
            fadeCount++;
            if (fadeCount >= fadeMax)
            {
                SceneManager.LoadScene("Game6");
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SceneManager.LoadScene("KubotaPlayer");
        }
    }

    public bool GetIsMoveFlag()
    {
        return isSceneMoveStoGFlag;
    }
}
