using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkiptoScene : MonoBehaviour
{
    int skipCount;
    bool isPush;
    [SerializeField, Header("長押しで行きたいシーン名")]
    string nextScene;

    // Start is called before the first frame update
    void Start()
    {
        skipCount = 0;
        isPush = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey("joystick button 4") || Input.GetKey(KeyCode.Z))
        {
            PushButton();
        }
        else
        {
            UpButton();
            skipCount = 0;
        }

        if (isPush)
        {
            skipCount++;
            if (skipCount >= 120)
            {
                SceneManager.LoadScene(nextScene);
            }
        }
        else
        {
            if (skipCount >= 180)
            {
                SceneManager.LoadScene(nextScene);
            }
        }


    }

    public void PushButton()
    {
        isPush = true;
    }

    public void UpButton()
    {
        isPush = false;
    }

}
