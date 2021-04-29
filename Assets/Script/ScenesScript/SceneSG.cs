using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSG : MonoBehaviour
{
    int fadeCount;
    int fadeMax;
    bool isSceneChangeFlag;

    [SerializeField, Header("フェードPrefab")]
    GameObject fadeManager;

    // Start is called before the first frame update
    void Start()
    {
        fadeCount = 0;
        fadeMax = 360;
        isSceneChangeFlag = false;
    }


    public void OnClickStartButton()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton0) || (Input.GetKeyDown(KeyCode.Space)))
        {
            isSceneChangeFlag = true;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SceneManager.LoadScene("estrelaStage");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SceneManager.LoadScene("KubotaPlayer");
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SceneManager.LoadScene("Enemy Scene");
        }

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
        {
            SceneManager.LoadScene("Game");
        }

        if (isSceneChangeFlag)
        {
            fadeManager.SetActive(true);
            fadeCount++;
            if (fadeCount >= fadeMax)
            {
                SceneManager.LoadScene("Game");
            }
        }

    }
}
