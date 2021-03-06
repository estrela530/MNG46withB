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
    public static bool isPauseAudio = false;

    //音関連
    public AudioClip buttonSE;
    AudioSource audioSource;

    public GameObject root;

    public bool pABflag;

    // Start is called before the first frame update
    void Start()
    {
        //Time.timeScale = 1f;
        //コンポーネントゲッツ！
        audioSource = GetComponent<AudioSource>();
        //PauseAudioUIPrefab.SetActive(false);
        pause = PauseUIPrefab.GetComponent<Pause>();
        //isPauseAudio = false;
        root = transform.root.gameObject;
    }

    public void OnClick()
    {

        if (Input.GetKeyDown(KeyCode.JoystickButton0) || (Input.GetKeyDown(KeyCode.Return)))
        {
            if (isPauseAudio == false)
            {
                isPauseAudio = true;
                Time.timeScale = 0f;
                //audioSource.PlayOneShot(buttonSE);
                PauseUIPrefab.SetActive(false);
                PauseAudioUIPrefab.SetActive(true);
               // Debug.Log("2222" + isPauseAudio);
            }
            else if (isPauseAudio == true)
            {
                //audioSource.PlayOneShot(buttonSE);
                //Debug.Log("bbbbbbbb");
                PauseUIPrefab.SetActive(false);
                PauseAudioUIPrefab.SetActive(false);
                isPauseAudio = false;
            }

            //pABflag = root.GetComponent<pauseManager>().pMflag;

            //if (!pABflag)
            //{
            //    Time.timeScale = 0f;
            //    //audioSource.PlayOneShot(buttonSE);
            //    PauseUIPrefab.SetActive(false);
            //    PauseAudioUIPrefab.SetActive(true);
            //    Debug.Log("2222" + pABflag);
            //    pABflag = true;
            //}
            //else if (pABflag)
            //{
            //    //audioSource.PlayOneShot(buttonSE);
            //    Debug.Log("bbbbbbbb");
            //    PauseUIPrefab.SetActive(true);
            //    PauseAudioUIPrefab.SetActive(false);
            //    pABflag = false;
            //}
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("見せなさい" + isPauseAudio);

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
        if (Input.GetKeyDown(KeyCode.JoystickButton7) || Input.GetKeyDown(KeyCode.P))
        {
            Time.timeScale = 1f;
            //audioSource.PlayOneShot(buttonSE);
            isPauseAudio = false;
            PauseUIPrefab.SetActive(false);
            PauseAudioUIPrefab.SetActive(false);
        }

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
        {
            Time.timeScale = 1f;
            //audioSource.PlayOneShot(buttonSE);
            isPauseAudio = false;
            PauseUIPrefab.SetActive(false);
            PauseAudioUIPrefab.SetActive(false);

        }
    }
}
