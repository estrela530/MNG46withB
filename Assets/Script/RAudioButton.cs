using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RAudioButton : MonoBehaviour
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
    public bool isPauseAudio = true;

    //音関連
    public AudioClip buttonSE;
    AudioSource audioSource;


    GameObject root;

    public bool pRABflag;


    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        //コンポーネントゲッツ！
        audioSource = GetComponent<AudioSource>();
        //PauseAudioUIPrefab.SetActive(false);
        pause = PauseUIPrefab.GetComponent<Pause>();

        isPauseAudio = true;

        root = transform.root.gameObject;
        pRABflag = root.GetComponent<pauseManager>().pMflag;

    }

    public void OnClick()
    {
        Debug.Log("押された！");

        //if (Input.GetKeyDown(KeyCode.JoystickButton0) || (Input.GetKeyDown(KeyCode.Space)))
        //{
        //if (!isPauseAudio)
        //{
        //    isPauseAudio = true;
        //    Time.timeScale = 0f;
        //    //audioSource.PlayOneShot(buttonSE);
        //    PauseUIPrefab.SetActive(false);
        //    PauseAudioUIPrefab.SetActive(true);
        //    Debug.Log("2222" + isPauseAudio);
        //}

        //0405
        //if (isPauseAudio)
        //{
        //    //audioSource.PlayOneShot(buttonSE);
        //    Debug.Log("bbbbbbbb");
        //    PauseUIPrefab.SetActive(true);
        //    PauseAudioUIPrefab.SetActive(false);
        //    isPauseAudio = false;
        //}

        pRABflag = root.GetComponent<pauseManager>().pMflag;

        //if (!pRABflag)
        //{
        //    //audioSource.PlayOneShot(buttonSE);
        //    Debug.Log("bbbbbbbb");
        //    PauseUIPrefab.SetActive(true);
        //    PauseAudioUIPrefab.SetActive(false);
        //}

        if (!pRABflag)
        {
            pRABflag = true;
            Time.timeScale = 0f;
            //audioSource.PlayOneShot(buttonSE);
            PauseUIPrefab.SetActive(false);
            PauseAudioUIPrefab.SetActive(true);
            Debug.Log("2222" + isPauseAudio);
        }
        else if (pRABflag)
        {
            //audioSource.PlayOneShot(buttonSE);
            Debug.Log("bbbbbbbb");
            PauseUIPrefab.SetActive(true);
            PauseAudioUIPrefab.SetActive(false);
            isPauseAudio = false;
        }

    }




    // Update is called once per frame
    void Update()
    {
        Debug.Log("見せなさい" + isPauseAudio);

        //if (Input.GetKeyDown(KeyCode.JoystickButton0) || (Input.GetKeyDown(KeyCode.Space)))
        //{
        //    if (!isPauseAudio)
        //    {

        //        PauseUIPrefab.SetActive(false);
        //        PauseAudioUIPrefab.SetActive(true);
        //        Time.timeScale = 0f;
        //        audioSource.PlayOneShot(buttonSE);
        //        isPauseAudio = true;
        //        Debug.Log("aaaaaaaaa");

        //    }
        //    else
        //    {
        //        PauseUIPrefab.SetActive(true);
        //        isPauseAudio = false;
        //        Time.timeScale = 1f;
        //        audioSource.PlayOneShot(buttonSE);
        //        Debug.Log("bbbbbbbb");
        //        PauseAudioUIPrefab.SetActive(false);
        //    }
        //}





        //if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
        //{
        //    Time.timeScale = 1f;
        //    //audioSource.PlayOneShot(buttonSE);
        //    isPauseAudio = false;
        //    PauseUIPrefab.SetActive(false);
        //    PauseAudioUIPrefab.SetActive(false);
        //}
    }
}
