using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    [SerializeField]
    //　ポーズした時に表示するUIのプレハブ
    private GameObject PauseUIPrefab;
    //　ポーズUIのインスタンス
    private GameObject pauseUIInstance;

    public bool isPause = false;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        PauseUIPrefab.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton7))
        {
            if (!isPause)
            {
                PauseUIPrefab.SetActive(true);
                Time.timeScale = 0f;
                isPause = true;
            }
            else
            {
                PauseUIPrefab.SetActive(false);
                Time.timeScale = 1f;
                isPause = false;
            }
        }
    }

    public bool GetPause()
    {
        return isPause;
    }
}

