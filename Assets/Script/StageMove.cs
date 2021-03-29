using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageMove : MonoBehaviour
{
    [SerializeField, Header("各クリアフラグ")]
    bool stage1Clear;
    [SerializeField]
    bool stage2Clear;
    [SerializeField]
    bool stage3Clear;


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

    // Start is called before the first frame update
    void Start()
    {
        bool stage1Clear = false;
        bool stage2Clear = false;
        bool stage3Clear = false;
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーのポジションがStage1のゴールにたどり着いたらステージクリアフラグ
        if (GameObject.FindGameObjectWithTag("Player").transform.position == stage1GoalPosition.transform.position)
        {
            stage1Clear = true;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            stage1Clear = true;
        }

        //プレイヤーのポジションをStage2のスタートポジションにする
        if (stage1Clear == true)
        {
            GameObject.FindGameObjectWithTag("Player").transform.position = stage2StartPosition.transform.position;
            stage1Clear = false;
        }

        if (stage2Clear == true)
        {
            GameObject.FindGameObjectWithTag("Player").transform.position = stage3StartPosition.transform.position;
            stage2Clear = false;
        }
    }
}
