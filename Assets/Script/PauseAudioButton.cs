using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseAudioButton : MonoBehaviour
{
    [SerializeField]
    //　ポーズした時に表示するUIのプレハブ
    private GameObject PauseUIPrefab;
    //　ポーズUIのインスタンス
    private GameObject pauseUIInstance;

    [SerializeField]
    //　PauseAudioボタンをクリックした時に表示するUIのプレハブ
    private GameObject PauseAudioUIPrefab;

    Pause pause;
    public bool isPauseAudio = false;

    //音関連
    public AudioClip buttonSE;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        //コンポーネントゲッツ！
        audioSource = GetComponent<AudioSource>();
        PauseAudioUIPrefab.SetActive(false);
        pause = PauseUIPrefab.GetComponent<Pause>();
    }

    public void OnClick()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton0) || (Input.GetKeyDown(KeyCode.Space)))
        {
            if (!isPauseAudio)
            {
                PauseUIPrefab.SetActive(false);
                PauseAudioUIPrefab.SetActive(true);
                Time.timeScale = 0f;
                audioSource.PlayOneShot(buttonSE);              
            }
            else
            {
                PauseUIPrefab.SetActive(true);
                PauseAudioUIPrefab.SetActive(false);
                Time.timeScale = 1f;
                audioSource.PlayOneShot(buttonSE);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
