﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionManagerScript : MonoBehaviour
{
    [SerializeField, Header("ミッション1星")]
    GameObject Mission1;
    [SerializeField, Header("ミッション2星")]
    GameObject Mission2;
    [SerializeField, Header("ミッション3星")]
    GameObject Mission3;

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
        mission1ClearFlag = Player.isDeadFlag;
        mission3ClearFlag = TimerScript.allDeathMissionClearFlag;
        //もしステージごとにリザルトをさすのであれば変更が必要
        clearSeconds = TimerScript.seconds;
    }

    // Update is called once per frame
    void Update()
    {
        //clearSeconds = timerScript.GetClearSeconds();
        fadeCount++;

        if (fadeCount >= 120 && !mission1ClearFlag)
        {
            Mission1.GetComponent<Image>().color = Color.yellow;
        }
        if (fadeCount >= 300 && clearSeconds <= 60)
        {
            Mission2.GetComponent<Image>().color = Color.yellow;
        }
        if (fadeCount >= 480 && mission3ClearFlag)
        {
            Mission3.GetComponent<Image>().color = Color.yellow;
        }       
    }
}
