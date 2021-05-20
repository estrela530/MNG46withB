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

    public bool isMoveFlag;

    [SerializeField, Header("フェードPrefab")]
    GameObject fadeManager;

    // Start is called before the first frame update
    void Start()
    {
        fadeCount = 0;
        fadeMax = 90;
        isScene1ChangeFlag = false;
        isScene2ChangeFlag = false;
        isMoveFlag = false;

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
        //Debug.Log(isScene1ChangeFlag);
        //Debug.Log(isScene2ChangeFlag);
        Debug.Log(isMoveFlag);

        //if ((Input.GetKeyDown(KeyCode.Space)))
        //{
        //    isScene1ChangeFlag = true;
        //}

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SceneManager.LoadScene("Game");
        }

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
        {
            SceneManager.LoadScene("Game2");
        }

        if (isScene1ChangeFlag)
        {
            isMoveFlag = true;
            fadeManager.SetActive(true);
            fadeCount++;
            if (fadeCount >= fadeMax)
            {
                SceneManager.LoadScene("Game2");
            }
        }
        else if (isScene2ChangeFlag)
        {
            isMoveFlag = true;
            fadeManager.SetActive(true);
            fadeCount++;
            if (fadeCount >= fadeMax)
            {
                SceneManager.LoadScene("Game3");
            }

        }

    }
}
