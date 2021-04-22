using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Escape : MonoBehaviour
{
    public AudioClip buttonSE;
    //AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        //コンポーネントゲッツ！
        //audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //audioSource.PlayOneShot(buttonSE);
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
      UnityEngine.Application.Quit();

#endif        
        }
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
    }

    public void OnClick()
    {

        if (Input.GetKeyDown(KeyCode.JoystickButton0) || (Input.GetKeyDown(KeyCode.Space)))
        {
            //audioSource.PlayOneShot(buttonSE);
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
      UnityEngine.Application.Quit();
#endif        
        }
    }
}