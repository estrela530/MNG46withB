using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetumeiButton : MonoBehaviour
{
    [SerializeField]
    //　ポーズした時に表示するUIのプレハブ
    private GameObject PauseUIPrefab;
    //　ポーズUIのインスタンス
    private GameObject pauseUIInstance;

    [SerializeField]
    //　Setumeiボタンをクリックした時に表示するUIのプレハブ
    private GameObject SetumeiUIPrefab;

    Pause pause;
    public static bool isSetumei = false;

    //音関連
    public AudioClip buttonSE;
    AudioSource audioSource;

    public GameObject root;

    public bool pABflag;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        pause = PauseUIPrefab.GetComponent<Pause>();
        root = transform.root.gameObject;
    }

    public void OnClick()
    {

        if (Input.GetKeyDown(KeyCode.JoystickButton0) || (Input.GetKeyDown(KeyCode.Return)))
        {
            if (isSetumei == false)
            {
                isSetumei = true;
                Time.timeScale = 0f;
                PauseUIPrefab.SetActive(false);
                SetumeiUIPrefab.SetActive(true);
            }
            else if (isSetumei == true)
            {
                PauseUIPrefab.SetActive(false);
                SetumeiUIPrefab.SetActive(false);
                isSetumei = false;
            }

         
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("isSetumeiは" + isSetumei);
        if (Input.GetKeyDown(KeyCode.JoystickButton7) || Input.GetKeyDown(KeyCode.P))
        {
            Time.timeScale = 1f;
            isSetumei = false;
            PauseUIPrefab.SetActive(false);
            SetumeiUIPrefab.SetActive(false);
        }

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
        {
            Time.timeScale = 1f;
            isSetumei = false;
            PauseUIPrefab.SetActive(false);
            SetumeiUIPrefab.SetActive(false);

        }
    }
}
