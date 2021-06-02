using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionManagerScript : MonoBehaviour
{
    [SerializeField, Header("ミッション1星")]
    GameObject Mission1;
    [SerializeField, Header("ミッション1星Clear")]
    GameObject Mission1C;
    [SerializeField, Header("ミッション2星")]
    GameObject Mission2;
    [SerializeField, Header("ミッション2星Clear")]
    GameObject Mission2C;
    [SerializeField, Header("ミッション3星")]
    GameObject Mission3;
    [SerializeField, Header("ミッション3星Clear")]
    GameObject Mission3C;

    GameObject m1;
    GameObject m2;
    GameObject m3;

    float fadeCount;
    
    TimerScript timerScript;
    float clearSeconds;
    bool mission1ClearFlag;
    bool mission3ClearFlag;
    //int missionClearCount;

    // Start is called before the first frame update
    void Start()
    {
        fadeCount = 0;
        clearSeconds = 0;
        //ミッション用の数値「全滅ミッション」
        //missionClearCount = 10; 
        //プレイヤーが生きてるかFlag
        mission1ClearFlag = Player.isDeadFlag;
        //
        mission3ClearFlag = TimerScript.allDeathMissionClearFlag;
        //もしステージごとにリザルトをさすのであれば変更が必要
        clearSeconds = TimerScript.minute;

        Mission1C.SetActive(false);
        Mission2C.SetActive(false);
        Mission3C.SetActive(false);

        IsAlreadyStage1.isAlreadyStage1ClearFlag = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //clearSeconds = timerScript.GetClearSeconds();
        fadeCount++;

        if (fadeCount >= 60 && !mission1ClearFlag)
        {
            Mission1.SetActive(false);
            Mission1C.SetActive(true);
        }

        if (fadeCount >= 120)
        {
            //ステージ1なら
            if (SceneNumberData.numberData.referer == "Game1")
            {
                //2分以内だったらミッションクリア
                if (clearSeconds < 2)
                {
                    Mission2.SetActive(false);
                    Mission2C.SetActive(true);
                }

            }
            //ステージ2なら
            else if (SceneNumberData.numberData.referer == "Game2")
            {
                //4分以内だったらミッションクリア
                if (clearSeconds < 4)
                {
                    Mission2.SetActive(false);
                    Mission2C.SetActive(true);
                }
            }
            //ステージ3なら
            else if (SceneNumberData.numberData.referer == "Game3")
            {
                //4分以内だったらミッションクリア
                if (clearSeconds < 4)
                {
                    Mission2.SetActive(false);
                    Mission2C.SetActive(true);
                }
            }
            //ステージ4なら
            else if (SceneNumberData.numberData.referer == "Game4")
            {
                //7分以内だったらミッションクリア
                if (clearSeconds < 7)
                {
                    Mission2.SetActive(false);
                    Mission2C.SetActive(true);
                }
            }
            //ステージ5なら
            else if (SceneNumberData.numberData.referer == "Game5")
            {
                //4分以内だったらミッションクリア
                if (clearSeconds < 4)
                {
                    Mission2.SetActive(false);
                    Mission2C.SetActive(true);
                }
            }
            //ステージ6なら
            else if (SceneNumberData.numberData.referer == "Game6")
            {
                //6分以内だったらミッションクリア
                if (clearSeconds < 6)
                {
                    Mission2.SetActive(false);
                    Mission2C.SetActive(true);
                }
            }
        }
        if (fadeCount >= 180 && mission3ClearFlag)
        {
            Mission3.SetActive(false);
            Mission3C.SetActive(true);
        }

    }
}
