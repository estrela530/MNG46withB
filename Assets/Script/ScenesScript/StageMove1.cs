using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageMove1 : MonoBehaviour
{
    [SerializeField, Header("各クリアフラグ")]
    bool stage1Clear;
    [SerializeField]
    bool stage2Clear;
    [SerializeField]
    bool bossClear;

    //[SerializeField, Header("現在進行ステージフラグ")]
    [SerializeField]
    public bool stage1Now { get; set; } = true;
    [SerializeField]
    public bool stage2Now { get; set; }
    [SerializeField]
    public bool bossNow { get; set; }

    public bool nowFlag { get; set; }

    bool positionMove12 = false;
    bool positionMove2B = false;

    Player player;
    Transition transition;

    int fadeCount;
    int fadeMax;
    int koko;

    [SerializeField, Header("BGMスライダー")]
    public Slider bgmSlider;

    [SerializeField, Header("ステージ1のスタートポジション")]
    GameObject stage1StartPosition;
    [SerializeField, Header("ステージ2のスタートポジション")]
    GameObject stage2StartPosition;
    [SerializeField, Header("Bossステージのスタートポジション")]
    GameObject stageBossStartPosition;

    [SerializeField, Header("1-2シーン切り替え時エフェクトPrefab")]
    GameObject fadeManager;
    [SerializeField, Header("2-3シーン切り替え時エフェクトPrefab")]
    GameObject fadeManager2B;

    [SerializeField, Header("ステージ1Prefab")]
    GameObject stage1Prefab;
    [SerializeField, Header("ステージ2Prefab")]
    GameObject stage2Prefab;
    [SerializeField, Header("ステージBossPrefab")]
    GameObject stageBossPrefab;

    [SerializeField, Header("普通ステージBGM")]
    GameObject NormalBGM;
    [SerializeField, Header("ボスステージBGM")]
    GameObject BossBGM;


    // Start is called before the first frame update
    void Start()
    {
        stage1Clear = false;
        stage2Clear = false;
        bossClear = false;

        stage1Now = true;
        stage2Now = false;
        bossNow = false;

        positionMove12 = false;
        positionMove2B = false;

        stage1Prefab.SetActive(true);
        stage2Prefab.SetActive(false);
        stageBossPrefab.SetActive(false);

        //0412バグ前
        //koko = 240;
        //0412バグ後
        koko = 90;
        //バグ前360　バグ後60 0416 300
        //fadeMax = 360;
        fadeMax = 120;
        //fadeMax = 60;
        fadeCount = 0;

        nowFlag = false;

        player = GetComponent<Player>();
        transition = GetComponent<Transition>();

        NormalBGM.SetActive(true);
        BossBGM.SetActive(false);
    }

    //void Update()
    //{
    //    if (stage1Clear == true)
    //    {
    //        Time.timeScale = 0f;
    //    }
    //}


    // Update is called once per frame
    void FixedUpdate()
    {
        if (nowFlag)
        {
            //プレイヤーのポジションゴール(旗)にたどり着いた時に今がステージ1ならステージ1クリアフラグ
            if (stage1Now)
            {
                Stage1();
            }
            //プレイヤーのポジションゴール(旗)にたどり着いた時に今がステージ2ならステージ2クリアフラグ
            else if (stage2Now)
            {
                Stage2();
            }
            //プレイヤーのポジションゴール(旗)にたどり着いた時に今がボスステージならゲームクリアフラグ
            else if (bossNow)
            {
                StageBoss();
            }
        }

        //Nボタンを押したらシーン切り替え（デバッグ用）
        if (Input.GetKeyDown(KeyCode.N))
        {
            nowFlag = true;
        }
    }

    public void Stage1()
    {
        stage1Clear = true;

        //プレイヤーのポジションをStage2のスタートポジションにする
        if (stage1Clear == true)
        {
            //Time.timeScale = 0f;
            fadeCount++;
            if (fadeCount >= fadeMax && positionMove12 == false)
            {
                GameObject.FindGameObjectWithTag("Player").transform.position = stage2StartPosition.transform.position;
                stage2Prefab.SetActive(true);
                positionMove12 = true;
            }

            fadeManager.SetActive(true);
            //transition.BeginTransition();

            if (fadeCount > 0 && fadeCount <= koko)
            {
                bgmSlider.GetComponent<Slider>().normalizedValue = bgmSlider.GetComponent<Slider>().normalizedValue / 1.01f;
            }
            else if (fadeCount > koko && fadeCount <= koko * 2)
            {
                bgmSlider.GetComponent<Slider>().normalizedValue = bgmSlider.GetComponent<Slider>().normalizedValue * 1.01f;
            }
            else if (fadeCount > koko * 2)
            {
                stage1Clear = false;
                stage1Now = false;
                //0415Playerとのやつができたら削除（デバッグ用）
                nowFlag = false;
                stage2Now = true;
                fadeCount = 0;
                fadeManager.SetActive(false);
                stage1Prefab.SetActive(false);
                Time.timeScale = 1.0f;
            }

        }

    }

    public void Stage2()
    {
        stage2Clear = true;

        //プレイヤーのポジションをStage3のスタートポジションにする
        if (stage2Clear == true)
        {
            fadeCount++;
            if (fadeCount >= fadeMax && positionMove2B == false)
            {
                GameObject.FindGameObjectWithTag("Player").transform.position = stageBossStartPosition.transform.position;
                stageBossPrefab.SetActive(true);
                positionMove2B = true;
            }

            fadeManager2B.SetActive(true);

            if (fadeCount > 0 && fadeCount <= koko)
            {
                bgmSlider.GetComponent<Slider>().normalizedValue = bgmSlider.GetComponent<Slider>().normalizedValue / 1.01f;
            }
            else if (fadeCount > koko && fadeCount <= koko * 2)
            {
                bgmSlider.GetComponent<Slider>().normalizedValue = bgmSlider.GetComponent<Slider>().normalizedValue * 1.01f;
                NormalBGM.SetActive(false);
                BossBGM.SetActive(true);
            }
            else if (fadeCount > koko * 2)
            {
                stage2Clear = false;
                stage2Now = false;
                bossNow = true;
                fadeCount = 0;
                fadeManager2B.SetActive(false);
                stage2Prefab.SetActive(false);

                nowFlag = false;

            }

        }

    }

    public void StageBoss()
    {
        bossClear = true;

        //GameClearへ
        if (bossClear == true)
        {
            bossClear = false;
            bossNow = false;
            SceneManager.LoadScene("GameClear");
            //0415Playerとのやつができたら削除（デバッグ用）
            nowFlag = false;

        }

    }
}



