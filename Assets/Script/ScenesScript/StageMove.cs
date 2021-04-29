using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageMove : MonoBehaviour
{
    [SerializeField, Header("各クリアフラグ")]
    bool stage1Clear;
    [SerializeField]
    bool stage2Clear;
    [SerializeField]
    bool stage3Clear;
    [SerializeField]
    bool stage4Clear;
    [SerializeField]
    bool stage5Clear;
    [SerializeField]
    bool bossClear;

    [SerializeField, Header("現在進行ステージフラグ")]
    bool stage1Now = true;
    [SerializeField]
    bool stage2Now;
    [SerializeField]
    bool stage3Now;
    [SerializeField]
    bool stage4Now;
    [SerializeField]
    bool stage5Now;
    [SerializeField]
    bool bossNow;

    public bool nowFlag;

    bool positionMove12 = false;
    bool positionMove23 = false;
    bool positionMove34 = false;
    bool positionMove4B = false;

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
    [SerializeField, Header("ステージ3のスタートポジション")]
    GameObject stage3StartPosition;
    [SerializeField, Header("ステージ4のスタートポジション")]
    GameObject stage4StartPosition;
    [SerializeField, Header("ステージ5のスタートポジション")]
    GameObject stage5StartPosition;
    [SerializeField, Header("Bossステージのスタートポジション")]
    GameObject stageBossStartPosition;

    [SerializeField, Header("1-2シーン切り替え時エフェクトPrefab")]
    GameObject fadeManager;
    [SerializeField, Header("2-3シーン切り替え時エフェクトPrefab")]
    GameObject fadeManager23;
    [SerializeField, Header("3-4シーン切り替え時エフェクトPrefab")]
    GameObject fadeManager34;
    [SerializeField, Header("4-Bシーン切り替え時エフェクトPrefab")]
    GameObject fadeManager4B;

    [SerializeField, Header("ステージ1Prefab")]
    GameObject stage1Prefab;
    [SerializeField, Header("ステージ2Prefab")]
    GameObject stage2Prefab;
    [SerializeField, Header("ステージ3Prefab")]
    GameObject stage3Prefab;
    [SerializeField, Header("ステージ4Prefab")]
    GameObject stage4Prefab;
    [SerializeField, Header("ステージBossPrefab")]
    GameObject stageBossPrefab;




    // Start is called before the first frame update
    void Start()
    {
        stage1Clear = false;
        stage2Clear = false;
        stage3Clear = false;
        stage4Clear = false;
        stage5Clear = false;
        bossClear = false;

        stage1Now = true;
        stage2Now = false;
        stage3Now = false;
        stage4Now = false;
        stage5Now = false;
        bossNow = false;

        positionMove12 = false;
        positionMove23 = false;
        positionMove34 = false;
        positionMove4B = false;

        stage1Prefab.SetActive(true);
        stage2Prefab.SetActive(false);
        stage3Prefab.SetActive(false);
        stage4Prefab.SetActive(false);
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
    }


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
            }//プレイヤーのポジションゴール(旗)にたどり着いた時に今がステージ3ならステージ3クリアフラグ
            else if (stage3Now)
            {
                Stage3();
            }//プレイヤーのポジションゴール(旗)にたどり着いた時に今がステージ4ならステージ4クリアフラグ
            else if (stage4Now)
            {
                Stage4();
            }//プレイヤーのポジションゴール(旗)にたどり着いた時に今がステージ5ならステージ5クリアフラグ
            else if (stage5Now)
            {
                Stage5();
            }//プレイヤーのポジションゴール(旗)にたどり着いた時に今がボスステージならゲームクリアフラグ
            else if (bossNow)
            {
                StageBoss();
            }
        }

        //Aボタンを押したらシーン切り替え（デバッグ用）
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
                GameObject.Find("SlimePlayer").transform.position = stage2StartPosition.transform.position;
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
                //Time.timeScale = 1.0f;
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
            if (fadeCount >= fadeMax && positionMove23 == false)
            {
                GameObject.Find("SlimePlayer").transform.position = stage3StartPosition.transform.position;
                stage3Prefab.SetActive(true);
                positionMove23 = true;
            }

            fadeManager23.SetActive(true);

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
                stage2Clear = false;
                stage2Now = false;
                stage3Now = true;
                fadeCount = 0;
                fadeManager23.SetActive(false);
                stage2Prefab.SetActive(false);
                //0415Playerとのやつができたら削除（デバッグ用）
                nowFlag = false;

            }

        }

    }

    public void Stage3()
    {
        stage3Clear = true;

        //プレイヤーのポジションをStage4のスタートポジションにする
        if (stage3Clear == true)
        {
            fadeCount++;
            if (fadeCount >= fadeMax && positionMove34 == false)
            {
                GameObject.Find("SlimePlayer").transform.position = stage4StartPosition.transform.position;
                stage4Prefab.SetActive(true);
                positionMove34 = true;
            }

            fadeManager34.SetActive(true);

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
                stage3Clear = false;
                stage3Now = false;
                stage4Now = true;
                fadeCount = 0;
                fadeManager34.SetActive(false);
                stage3Prefab.SetActive(false);
                //0415Playerとのやつができたら削除（デバッグ用）
                nowFlag = false;

            }

        }

    }

    public void Stage4()
    {
        stage4Clear = true;

        //プレイヤーのポジションをStage5のスタートポジションにする
        if (stage4Clear == true)
        {
            fadeCount++;
            if (fadeCount >= fadeMax && positionMove4B == false)
            {
                GameObject.Find("SlimePlayer").transform.position = stage5StartPosition.transform.position;
                stageBossPrefab.SetActive(true);
                positionMove4B = true;
            }

            fadeManager4B.SetActive(true);

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
                stage4Clear = false;
                stage4Now = false;
                ////ステージ5がない
                //stage5Now = true;
                bossNow = true;
                fadeCount = 0;
                fadeManager4B.SetActive(false);
                stage4Prefab.SetActive(false);
                //0415Playerとのやつができたら削除（デバッグ用）
                nowFlag = false;

            }

        }

    }

    public void Stage5()
    {
        stage5Clear = true;

        //プレイヤーのポジションをBossStageのスタートポジションにする
        if (stage5Clear == true)
        {
            fadeCount++;
            if (fadeCount >= fadeMax)
            {
                GameObject.Find("SlimePlayer").transform.position = stageBossStartPosition.transform.position;
            }

            fadeManager.SetActive(true);

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
                stage5Clear = false;
                stage5Now = false;
                bossNow = true;
                fadeCount = 0;
                fadeManager.SetActive(false);
                //0415Playerとのやつができたら削除（デバッグ用）
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



