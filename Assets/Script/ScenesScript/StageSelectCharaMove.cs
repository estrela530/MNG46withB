using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectCharaMove : MonoBehaviour
{

    [SerializeField, Header("ねじりん")]
    GameObject nejirin;
    [SerializeField, Header("ステージ1")]
    GameObject s1Image;
    [SerializeField, Header("ステージ2")]
    GameObject s2Image;
    [SerializeField, Header("ステージ3")]
    GameObject s3Image;
    [SerializeField, Header("ステージ4")]
    GameObject s4Image;
    [SerializeField, Header("ステージ5")]
    GameObject s5Image;
    [SerializeField, Header("ステージ6")]
    GameObject s6Image;

    [SerializeField, Header("ボタン1")]
    GameObject s1Button;
    [SerializeField, Header("ボタン2")]
    GameObject s2Button;
    [SerializeField, Header("ボタン3")]
    GameObject s3Button;
    [SerializeField, Header("ボタン4")]
    GameObject s4Button;
    [SerializeField, Header("ボタン5")]
    GameObject s5Button;
    [SerializeField, Header("ボタン6")]
    GameObject s6Button;


    [SerializeField, Header("シーン遷移")]
    GameObject sceneSGObj;

    SceneSG sceneSG;
    bool isSceneMoveFlag;

    public bool button1NowFlag;
    public bool button2NowFlag = false;
    public bool button3NowFlag = false;
    public bool button4NowFlag = false;
    public bool button5NowFlag = false;
    public bool button6NowFlag = false;

    Vector3 nejirinnStage1Pos = new Vector3(-40, -23, 80);
    Vector3 nejirinnStage2Pos = new Vector3(5, -9, 80);

    public bool isMissionImageFlag;

    // Start is called before the first frame update
    void Start()
    {
        nejirin.transform.position = nejirinnStage1Pos;
        isMissionImageFlag = true;
        button1NowFlag = true;
        button2NowFlag = false;
        button3NowFlag = false;
        button4NowFlag = false;
        button5NowFlag = false;
        button6NowFlag = false;
        s2Image.SetActive(false);
        s1Image.SetActive(true);
        s1Button.SetActive(true);
        s2Button.SetActive(true);
        s3Button.SetActive(false);
        s4Button.SetActive(false);
        s5Button.SetActive(false);
        s6Button.SetActive(false);

        sceneSG = sceneSGObj.GetComponent<SceneSG>();
    }

    // Update is called once per frame
    void Update()
    {
        //L Stick
        float lsh = Input.GetAxisRaw("Horizontal");

        isSceneMoveFlag = sceneSG.GetIsMoveFlag();

        //1->2
        if (Input.GetKeyDown(KeyCode.RightArrow) || lsh > 0 && !isSceneMoveFlag && button1NowFlag)
        {
            Debug.Log("右押された");
            button1NowFlag = false;
            button2NowFlag = true;
            s1Image.SetActive(false);
            s2Image.SetActive(true);
            nejirin.transform.position = nejirinnStage2Pos;
            isMissionImageFlag = true;
        }
        //2->1
        if (Input.GetKeyDown(KeyCode.LeftArrow) || lsh < 0 && !isSceneMoveFlag && button2NowFlag)
        {
            Debug.Log("左押された");
            button2NowFlag = false;
            button1NowFlag = true;
            s2Image.SetActive(false);
            s1Image.SetActive(true);
            nejirin.transform.position = nejirinnStage1Pos;
            isMissionImageFlag = true;
        }
        //2->3
        if (Input.GetKeyDown(KeyCode.RightArrow) || lsh > 0 && !isSceneMoveFlag && button2NowFlag)
        {
            Debug.Log("右押された");
            button2NowFlag = false;
            button3NowFlag = true;
            s2Image.SetActive(false);
            s3Image.SetActive(true);
            nejirin.transform.position = nejirinnStage2Pos;
            isMissionImageFlag = true;
        }
        //3->2
        if (Input.GetKeyDown(KeyCode.LeftArrow) || lsh < 0 && !isSceneMoveFlag && button3NowFlag)
        {
            Debug.Log("左押された");
            button3NowFlag = false;
            button2NowFlag = true;
            s3Image.SetActive(false);
            s2Image.SetActive(true);
            nejirin.transform.position = nejirinnStage1Pos;
            isMissionImageFlag = true;
        }
        //3->4
        if (Input.GetKeyDown(KeyCode.RightArrow) || lsh > 0 && !isSceneMoveFlag && button3NowFlag)
        {
            Debug.Log("右押された");
            button3NowFlag = false;
            button4NowFlag = true;
            s3Image.SetActive(false);
            s4Image.SetActive(true);
            nejirin.transform.position = nejirinnStage2Pos;
            isMissionImageFlag = true;
        }
        //4->3
        if (Input.GetKeyDown(KeyCode.LeftArrow) || lsh < 0 && !isSceneMoveFlag && button4NowFlag)
        {
            Debug.Log("左押された");
            button4NowFlag = false;
            button3NowFlag = true;
            s4Image.SetActive(false);
            s3Image.SetActive(true);
            nejirin.transform.position = nejirinnStage1Pos;
            isMissionImageFlag = true;
        }
        //4->5
        if (Input.GetKeyDown(KeyCode.RightArrow) || lsh > 0 && !isSceneMoveFlag && button4NowFlag)
        {
            Debug.Log("右押された");
            button4NowFlag = false;
            button5NowFlag = true;
            s4Image.SetActive(false);
            s5Image.SetActive(true);
            nejirin.transform.position = nejirinnStage2Pos;
            isMissionImageFlag = true;
        }
        //5->4
        if (Input.GetKeyDown(KeyCode.LeftArrow) || lsh < 0 && !isSceneMoveFlag && button5NowFlag)
        {
            Debug.Log("左押された");
            button5NowFlag = false;
            button4NowFlag = true;
            s5Image.SetActive(false);
            s4Image.SetActive(true);
            nejirin.transform.position = nejirinnStage1Pos;
            isMissionImageFlag = true;
        }
        //5->6
        if (Input.GetKeyDown(KeyCode.RightArrow) || lsh > 0 && !isSceneMoveFlag && button5NowFlag)
        {
            Debug.Log("右押された");
            button5NowFlag = false;
            button6NowFlag = true;
            s5Image.SetActive(false);
            s6Image.SetActive(true);
            nejirin.transform.position = nejirinnStage2Pos;
            isMissionImageFlag = true;
        }
        //6->5
        if (Input.GetKeyDown(KeyCode.LeftArrow) || lsh < 0 && !isSceneMoveFlag && button6NowFlag)
        {
            Debug.Log("左押された");
            button6NowFlag = false;
            button5NowFlag = true;
            s6Image.SetActive(false);
            s5Image.SetActive(true);
            nejirin.transform.position = nejirinnStage1Pos;
            isMissionImageFlag = true;
        }
        //クリックしたらButton1へ
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
        {
            button1NowFlag = true;
            s2Image.SetActive(false);
            s3Image.SetActive(false);
            s4Image.SetActive(false);
            s5Image.SetActive(false);
            s6Image.SetActive(false);
            s1Image.SetActive(true);
            nejirin.transform.position = nejirinnStage1Pos;
            button1NowFlag = false;
            isMissionImageFlag = true;
        }
    }
}
