using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageMove : MonoBehaviour
{
    [SerializeField, Header("各クリアフラグ")]
    bool stage1Clear;
    [SerializeField]
    bool stage2Clear;
    [SerializeField]
    bool stage3Clear;

    int fadeCount;

    [SerializeField, Header("BGMスライダー")]
    public Slider bgmSlider;
    float BGMmemo;
    bool One;

    [SerializeField, Header("ステージ1のスタートポジション")]
    GameObject stage1StartPosition;
    [SerializeField, Header("ステージ1のゴールポジション")]
    GameObject stage1GoalPosition;
    [SerializeField, Header("ステージ2のスタートポジション")]
    GameObject stage2StartPosition;
    [SerializeField, Header("ステージ2のゴールポジション")]
    GameObject stage2GoalPosition;
    [SerializeField, Header("ステージ3のスタートポジション")]
    GameObject stage3StartPosition;
    [SerializeField, Header("ステージ3のゴールポジション")]
    GameObject stage3GoalPosition;

    [SerializeField, Header("シーン切り替え時エフェクトPrefab")]
    GameObject fadeManager;


    // Start is called before the first frame update
    void Start()
    {
        bool stage1Clear = false;
        bool stage2Clear = false;
        bool stage3Clear = false;
        One = true;

        fadeCount = 0;
        BGMmemo = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーのポジションがStage1のゴールにたどり着いたらステージクリアフラグ
        if (GameObject.FindGameObjectWithTag("Player").transform.position == stage1GoalPosition.transform.position)
        {
            stage1Clear = true;
        }

        //Aボタンを押したらシーン切り替え（デバッグ用）
        if (Input.GetKeyDown(KeyCode.N))
        {
            stage1Clear = true;
        }

        //プレイヤーのポジションをStage2のスタートポジションにする
        if (stage1Clear == true)
        {
            fadeCount++;
            if (fadeCount >= 360)
            {
                GameObject.FindGameObjectWithTag("Player").transform.position = stage2StartPosition.transform.position;
                stage1Clear = false;
                fadeCount = 0;
            }
            //fadeManager.SetActive(true);
        }

        if (stage2Clear == true)
        {
            GameObject.FindGameObjectWithTag("Player").transform.position = stage3StartPosition.transform.position;
            stage2Clear = false;
        }

        //シーン切り替えのflagがtrueになったらfadeManagerのsetActiveをtrueにする
        if (stage1Clear || stage2Clear || stage3Clear)
        {
            fadeManager.SetActive(true);
            //シーン切り替え時の音量調節処理
            //bgmSlider.GetComponent<Slider>().normalizedValue = bgmSlider.GetComponent<Slider>().normalizedValue / 2;

        }



    }
}
