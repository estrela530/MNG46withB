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
        mission1ClearFlag = Player.isDeadFlag;
        mission3ClearFlag = TimerScript.allDeathMissionClearFlag;
        //もしステージごとにリザルトをさすのであれば変更が必要
        clearSeconds = TimerScript.minute;

        Mission1C.SetActive(false);
        Mission2C.SetActive(false);
        Mission3C.SetActive(false);
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

        if (fadeCount >= 120 && clearSeconds < 1)
        {
            Mission2.SetActive(false);
            Mission2C.SetActive(true);
        }

        if (fadeCount >= 180 && mission3ClearFlag)
        {
            Mission3.SetActive(false);
            Mission3C.SetActive(true);
        }

    }
}
