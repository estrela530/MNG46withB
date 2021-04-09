using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearStageUI : MonoBehaviour
{
    public Text clearStageText;

    //StageMoveのスクリプト取得
    SceneGR sGR;
    [SerializeField]
    private GameObject sGRP;
    int sText = 0;

    //今は仮段階、　後にはこのstageNumberにクリアしたシーンから受け取るシーン番号を代入できるようにする
    int stageNumber = 1;


    // Start is called before the first frame update
    void Start()
    {
        clearStageText = GetComponent<Text>();
        sGR = sGRP.GetComponent<SceneGR>();
    }

    // Update is called once per frame
    void Update()
    {
        //なんでfalseなん？
        if (!sGR.changeOKFlag)
        {
            Debug.Log(sGR.changeOKFlag);
            clearStageText.text = "Stage" + stageNumber;
            //    clearStageText.text = "Stage";//+ ":" + stageNumber.ToString("1")
            //    //sGR.changeOKFlag = false;
        }
    }
}