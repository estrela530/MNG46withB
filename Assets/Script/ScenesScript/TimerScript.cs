using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerScript : MonoBehaviour
{
    [SerializeField]
    public static int minute;
    [SerializeField]
    public static float seconds;

    public static int enemyCounter { get; set; }
    //public static int sendCounter;

    //　前のUpdateの時の秒数
    private float oldSeconds;
    //　タイマー表示用テキスト
    //private Text timerText; 
    [SerializeField]
    private TextMeshProUGUI timerText;
    int missionClearCount;

    public static bool allDeathMissionClearFlag;

    void Start()
    {
        minute = 0;
        seconds = 0f;
        enemyCounter = 0;
        //sendCounter = 0;
        oldSeconds = 0f;
        //timerText = GetComponentInChildren<Text>();
        //ミッション用の数値「全滅ミッション」
        missionClearCount = 7;

        allDeathMissionClearFlag = false;
    }

    void FixedUpdate()
    {
        seconds += Time.deltaTime;
        if (seconds >= 60f)
        {
            minute++;
            seconds = seconds - 60;
        }
        //　値が変わった時だけテキストUIを更新
        if ((int)seconds != (int)oldSeconds)
        {
            timerText.text = minute.ToString("00") + ":" + ((int)seconds).ToString("00");
        }
        oldSeconds = seconds;
        //Debug.Log(enemyCounter);
        if (enemyCounter >= missionClearCount)
        {
            allDeathMissionClearFlag = true;
        }

        //Debug.Log("seconds" + seconds);
    }

    public float GetClearSeconds()
    {
        return seconds;
    }

    public  bool GetAllDeathMissionClear()
    {
        return allDeathMissionClearFlag;
    }

}
