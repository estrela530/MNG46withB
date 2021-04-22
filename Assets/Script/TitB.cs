using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class TitB : MonoBehaviour
{
    public AudioClip buttonSE;
    //AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        //コンポーネントゲッツ！
        //audioSource = GetComponent<AudioSource>();
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
        if (Input.GetKeyDown(KeyCode.JoystickButton0) || (Input.GetKeyDown(KeyCode.Space)))
        {
            //audioSource.PlayOneShot(buttonSE);
            SceneManager.LoadScene("Title");
        }
    }
}
