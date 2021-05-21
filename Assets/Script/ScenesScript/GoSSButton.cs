using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GoSSButton : MonoBehaviour
{
    public AudioClip buttonSE;
    Pause pause;
    //AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        //コンポーネントゲッツ！
        //audioSource = GetComponent<AudioSource>();
        pause = GetComponent<Pause>();
    }

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

    }

    public void OnClickStartButton()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton0) || (Input.GetKeyDown(KeyCode.Return)))
        {
            //audioSource.PlayOneShot(buttonSE);
            SceneManager.LoadScene("StageSelect");
            //Pause.isPause = false;
            Time.timeScale = 1f;
        }
    }
}
