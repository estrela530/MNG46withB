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

    [SerializeField, Header("ボタン1・2キャンバス")]
    GameObject s12Canvas;
    [SerializeField, Header("ボタン3・4キャンバス")]
    GameObject s34Canvas;
    [SerializeField, Header("ボタン5・6キャンバス")]
    GameObject s56Canvas;

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

    Vector3 nejirinStage1Pos = new Vector3(-40, -23, 80);
    Vector3 nejirinStage2Pos = new Vector3(5, -9, 80);

    public bool isMissionImageFlag;
    int moveCount;


    // Start is called before the first frame update
    void Start()
    {
        nejirin.transform.position = nejirinStage1Pos;
        isMissionImageFlag = true;
        button1NowFlag = true;
        button2NowFlag = false;
        button3NowFlag = false;
        button4NowFlag = false;
        button5NowFlag = false;
        button6NowFlag = false;
        s2Image.SetActive(false);
        s1Image.SetActive(true);
        s12Canvas.SetActive(true);
        s34Canvas.SetActive(false);
        s56Canvas.SetActive(false);

        moveCount = 0;
        sceneSG = sceneSGObj.GetComponent<SceneSG>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //moveCount++;
        //L Stick
        float lsh = Input.GetAxisRaw("Horizontal");
        isSceneMoveFlag = sceneSG.GetIsMoveFlag();
        Debug.Log("呼ばれた！");
        //1->2
        if (Input.GetKeyUp(KeyCode.RightArrow) || lsh > 0 && !isSceneMoveFlag && button1NowFlag)
        {
            Debug.Log("1で右押された");
            button1NowFlag = false;
            button2NowFlag = true;
            s1Image.SetActive(false);
            s2Image.SetActive(true);
            nejirin.transform.position = nejirinStage2Pos;
            isMissionImageFlag = true;
        }
        //2->1
        else if (Input.GetKeyUp(KeyCode.LeftArrow) || lsh < 0 && !isSceneMoveFlag && button2NowFlag)
        {
            Debug.Log("2で左押された");
            button2NowFlag = false;
            button1NowFlag = true;
            s2Image.SetActive(false);
            s1Image.SetActive(true);
            nejirin.transform.position = nejirinStage1Pos;
            isMissionImageFlag = true;
        }
        //2->3
        else if (Input.GetKeyUp(KeyCode.RightArrow) || lsh > 0 && !isSceneMoveFlag && button2NowFlag)
        {
            Debug.Log("2で右押された");
            button2NowFlag = false;
            button3NowFlag = true;
            s2Image.SetActive(false);
            s3Image.SetActive(true);
            s12Canvas.SetActive(false);
            s34Canvas.SetActive(true);
            nejirin.transform.position = nejirinStage2Pos;
            isMissionImageFlag = true;
        }
        //3->2
        else if (Input.GetKeyUp(KeyCode.LeftArrow) || lsh < 0 && !isSceneMoveFlag && button3NowFlag)
        {
            Debug.Log("3で左押された");
            button3NowFlag = false;
            button2NowFlag = true;
            s3Image.SetActive(false);
            s2Image.SetActive(true);
            s34Canvas.SetActive(false);
            s12Canvas.SetActive(true);
            nejirin.transform.position = nejirinStage1Pos;
            isMissionImageFlag = true;
        }
        //3->4
        else if (Input.GetKeyUp(KeyCode.RightArrow) || lsh > 0 && !isSceneMoveFlag && button3NowFlag)
        {
            Debug.Log("3で右押された");
            button3NowFlag = false;
            button4NowFlag = true;
            s3Image.SetActive(false);
            s4Image.SetActive(true);
            nejirin.transform.position = nejirinStage2Pos;
            isMissionImageFlag = true;
        }
        //4->3
        else if (Input.GetKeyUp(KeyCode.LeftArrow) || lsh < 0 && !isSceneMoveFlag && button4NowFlag)
        {
            Debug.Log("4で左押された");
            button4NowFlag = false;
            button3NowFlag = true;
            s4Image.SetActive(false);
            s3Image.SetActive(true);
            nejirin.transform.position = nejirinStage1Pos;
            isMissionImageFlag = true;
        }
        //4->5
        else if (Input.GetKeyUp(KeyCode.RightArrow) || lsh > 0 && !isSceneMoveFlag && button4NowFlag)
        {
            Debug.Log("4で右押された");
            button4NowFlag = false;
            button5NowFlag = true;
            s4Image.SetActive(false);
            s5Image.SetActive(true);
            s34Canvas.SetActive(false);
            s56Canvas.SetActive(true);
            nejirin.transform.position = nejirinStage2Pos;
            isMissionImageFlag = true;
        }
        //5->4
        else if (Input.GetKeyUp(KeyCode.LeftArrow) || lsh < 0 && !isSceneMoveFlag && button5NowFlag)
        {
            Debug.Log("5で左押された");
            button5NowFlag = false;
            button4NowFlag = true;
            s5Image.SetActive(false);
            s4Image.SetActive(true);
            s56Canvas.SetActive(false);
            s34Canvas.SetActive(true);
            nejirin.transform.position = nejirinStage1Pos;
            isMissionImageFlag = true;
        }
        //5->6
        else if (Input.GetKeyUp(KeyCode.RightArrow) || lsh > 0 && !isSceneMoveFlag && button5NowFlag)
        {
            Debug.Log("5で右押された");
            button5NowFlag = false;
            button6NowFlag = true;
            s5Image.SetActive(false);
            s6Image.SetActive(true);
            nejirin.transform.position = nejirinStage2Pos;
            isMissionImageFlag = true;
        }
        //6->5
        else if (Input.GetKeyUp(KeyCode.LeftArrow) || lsh < 0 && !isSceneMoveFlag && button6NowFlag)
        {
            Debug.Log("6で左押された");
            button6NowFlag = false;
            button5NowFlag = true;
            s6Image.SetActive(false);
            s5Image.SetActive(true);
            nejirin.transform.position = nejirinStage1Pos;
            isMissionImageFlag = true;
        }
        //クリックしたらButton1へ
        else if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
        {
            button1NowFlag = true;
            s12Canvas.SetActive(true);
            s34Canvas.SetActive(false);
            s56Canvas.SetActive(false);
            s34Canvas.SetActive(false);
            s56Canvas.SetActive(false);
            s12Canvas.SetActive(true);
            nejirin.transform.position = nejirinStage1Pos;
            button1NowFlag = false;
            isMissionImageFlag = true;
        }

    }
}
