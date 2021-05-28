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

    [SerializeField, Header("メインカメラ")]
    GameObject mainCamera;


    SceneSG sceneSG;
    bool isSceneMoveFlag;

    public int state = 1;

    Vector3 nejirinStage1Pos = new Vector3(-47, -25, 70);
    Vector3 nejirinStage2Pos = new Vector3(30, -25, 70);
    Vector3 nejirinStage3Pos = new Vector3(200, -25, 70);
    Vector3 nejirinStage4Pos = new Vector3(283, -3, 70);
    Vector3 nejirinStage5Pos = new Vector3(418, 2, 70);
    Vector3 nejirinStage6Pos = new Vector3(518, 25, 70);

    Vector3 Camera1Pos = new Vector3(0, 0, 0);
    Vector3 Camera2Pos = new Vector3(231, 0, 0);
    Vector3 Camera3Pos = new Vector3(462, 0, 0);

    GameObject StageSelectManager;
    SelectButton selectButton;

    public bool isMissionImageFlag;
    int moveCount;

    float lsh = 0;

    float preStick = 0;//Horizontalトリガーの保存用キー
    float nowStick = 0;//Horizontalトリガーの取得用キー


    // Start is called before the first frame update
    void Start()
    {
        nejirin.transform.position = nejirinStage1Pos;
        isMissionImageFlag = true;
        s2Image.SetActive(false);
        s1Image.SetActive(true);
        //s12Canvas.SetActive(true);
        //s34Canvas.SetActive(false);
        //s56Canvas.SetActive(false);
        moveCount = 0;
        mainCamera.transform.position = Camera1Pos;
        sceneSG = sceneSGObj.GetComponent<SceneSG>();
        //親を取得
        StageSelectManager = transform.root.gameObject;
        selectButton = StageSelectManager.GetComponent<SelectButton>();
    }

    // Update is called once per frame
    void Update()
    {
        //入力状態を取得
        nowStick = Input.GetAxisRaw("Horizontal");


        //moveCount++;
        //L Stick
        lsh = Input.GetAxisRaw("Horizontal");
        isSceneMoveFlag = sceneSG.GetIsMoveFlag();
        Debug.Log("呼ばれた！");
        //1->2

        //関数を読んでいます。
        Test();

        //現在のキーの状態をコピーする
        preStick = nowStick;

    }

    void Test()
    {
        if (isSceneMoveFlag) return;

        switch (state)
        {
            case 1:
                if (Input.GetKeyDown(KeyCode.RightArrow) || GetStick_Right())
                {
                    s1Image.SetActive(false);
                    s2Image.SetActive(true);
                    nejirin.transform.position = nejirinStage2Pos;
                    selectButton.buttons[1].Select();
                    isMissionImageFlag = true;

                    state = 2;
                }

                break;
            case 2:
                //2->1
                if (Input.GetKeyDown(KeyCode.LeftArrow) || GetStick_Left())
                {
                    s2Image.SetActive(false);
                    s1Image.SetActive(true);
                    nejirin.transform.position = nejirinStage1Pos;
                    selectButton.buttons[0].Select();
                    isMissionImageFlag = true;

                    state = 1;
                }
                //2->3
                if (Input.GetKeyDown(KeyCode.RightArrow) || GetStick_Right())
                {
                    s2Image.SetActive(false);
                    s3Image.SetActive(true);
                    //s12Canvas.SetActive(false);
                    //s34Canvas.SetActive(true);
                    mainCamera.transform.position = Camera2Pos;
                    nejirin.transform.position = nejirinStage3Pos;
                    selectButton.buttons[2].Select();
                    isMissionImageFlag = true;

                    state = 3;
                }
                break;
            case 3:
                //3->2
                if (Input.GetKeyDown(KeyCode.LeftArrow) || GetStick_Left())
                {
                    s3Image.SetActive(false);
                    s2Image.SetActive(true);
                    //s34Canvas.SetActive(false);
                    //s12Canvas.SetActive(true);
                    mainCamera.transform.position = Camera1Pos;
                    nejirin.transform.position = nejirinStage2Pos;
                    selectButton.buttons[1].Select();
                    isMissionImageFlag = true;

                    state = 2;
                }
                //3->4
                else if (Input.GetKeyDown(KeyCode.RightArrow) || GetStick_Right())
                {
                    s3Image.SetActive(false);
                    s4Image.SetActive(true);
                    nejirin.transform.position = nejirinStage4Pos;
                    selectButton.buttons[3].Select();
                    isMissionImageFlag = true;

                    state = 4;
                }
                break;
            case 4:
                //4->3
                if (Input.GetKeyDown(KeyCode.LeftArrow) || GetStick_Left())
                {
                    s4Image.SetActive(false);
                    s3Image.SetActive(true);
                    nejirin.transform.position = nejirinStage3Pos;
                    selectButton.buttons[2].Select();
                    isMissionImageFlag = true;

                    state = 3;
                }
                //4->5
                else if (Input.GetKeyDown(KeyCode.RightArrow) || GetStick_Right())
                {
                    s4Image.SetActive(false);
                    s5Image.SetActive(true);
                    //s34Canvas.SetActive(false);
                    //s56Canvas.SetActive(true);
                    mainCamera.transform.position = Camera3Pos;
                    nejirin.transform.position = nejirinStage5Pos;
                    selectButton.buttons[4].Select();
                    isMissionImageFlag = true;

                    state = 5;
                }
                break;
            case 5:
                //5->4
                if (Input.GetKeyDown(KeyCode.LeftArrow) || GetStick_Left())
                {
                    s5Image.SetActive(false);
                    s4Image.SetActive(true);
                    //s56Canvas.SetActive(false);
                    //s34Canvas.SetActive(true);
                    mainCamera.transform.position = Camera2Pos;
                    nejirin.transform.position = nejirinStage4Pos;
                    selectButton.buttons[3].Select();
                    isMissionImageFlag = true;

                    state = 4;
                }
                //5->6
                else if (Input.GetKeyDown(KeyCode.RightArrow) || GetStick_Right())
                {
                    s5Image.SetActive(false);
                    s6Image.SetActive(true);
                    nejirin.transform.position = nejirinStage6Pos;
                    selectButton.buttons[5].Select();
                    isMissionImageFlag = true;

                    state = 6;
                }
                break;
            case 6:
                //6->5
                if (Input.GetKeyDown(KeyCode.LeftArrow) || GetStick_Left())
                {
                    s6Image.SetActive(false);
                    s5Image.SetActive(true);
                    nejirin.transform.position = nejirinStage5Pos;
                    selectButton.buttons[4].Select();
                    isMissionImageFlag = true;

                    state = 5;
                }
                //クリックしたらButton1へ
                else if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
                {
                    //s12Canvas.SetActive(true);
                    //s34Canvas.SetActive(false);
                    //s56Canvas.SetActive(false);
                    //s34Canvas.SetActive(false);
                    //s56Canvas.SetActive(false);
                    //s12Canvas.SetActive(true);
                    mainCamera.transform.position = Camera1Pos;
                    nejirin.transform.position = nejirinStage1Pos;
                    selectButton.buttons[0].Select();
                    isMissionImageFlag = true;

                    state = 1;
                }
                break;

            default:
                break;
        }
    }

    private bool GetStick_Right()
    {
        //前が押されていない
        if (preStick == 0.0f)
        {
            //左が押されている
            if (nowStick > 0)
            {
                return true;
            }
        }
        return false;
    }

    private bool GetStick_Left()
    {
        //前が押されていない
        if (preStick == 0.0f)
        {
            //右が押されている
            if (nowStick < 0)
            {
                return true;
            }
        }
        return false;
    }
}
