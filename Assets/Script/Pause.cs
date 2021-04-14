﻿using System.Collections;
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

    SelectButton selectButton;

    //音関連
    public AudioClip buttonSE;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        PauseUIPrefab.SetActive(false);
        //コンポーネントゲッツ！
        audioSource = GetComponent<AudioSource>();
        selectButton = GetComponent<SelectButton>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton7) || Input.GetKeyDown(KeyCode.P))
        {
            if (!isPause)
            {
                PauseUIPrefab.SetActive(true);
                Time.timeScale = 0f;
                //audioSource.PlayOneShot(buttonSE);
                isPause = true;
                selectButton.Wasshoi();
            }
            else
            {
                PauseUIPrefab.SetActive(false);
                Time.timeScale = 1f;
                //audioSource.PlayOneShot(buttonSE);
                isPause = false;
            }
        }

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
        {
            PauseUIPrefab.SetActive(false);
            Time.timeScale = 1f;
            //audioSource.PlayOneShot(buttonSE);
            isPause = false;
        }
    }

    public bool GetPause()
    {
        return isPause;
    }

    public void OnClick()
    {
        //if (Input.GetKeyDown(KeyCode.JoystickButton0) || (Input.GetKeyDown(KeyCode.Space)))
        //{
        if (!isPause)
        {
            PauseUIPrefab.SetActive(true);
            Time.timeScale = 0f;
            //audioSource.PlayOneShot(buttonSE);
            isPause = true;
        }
        else
        {
            Time.timeScale = 1f;
            //udioSource.PlayOneShot(buttonSE);
            isPause = false;
            PauseUIPrefab.SetActive(false);
        }
        //}

    }
}

