using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{

    //private GameObject Target;//

    //private float disRes;//プレイヤーとの距離
    //public float area;//この数値以下に出現する

    //[SerializeField, Header("ボス")]
    //GameObject Boss;

    public GameObject PawnEnemy;

    [SerializeField, Header("エネミーの出現数")]
    int resCount;//プレハブの出現数

    public GameObject[] bases;
    private float time;
    [SerializeField, Header("時間")]
    float ResTime;

    // Start is called before the first frame update
    void Start()
    {
        //Target = GameObject.Find("Player");//距離を知りたいオブジェクトを書く
        //Boss.GetComponent<BossMove>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //disRes = Vector3.Distance(transform.position, Target.transform.position);//二つの距離を計算

        ////エリア以下の数値になったらでる
        //if (disRes < area)
        //{
           Resp();
        //}

        

    }

    void Resp()
    {
        if (resCount == bases.Length)//
        {
            time -= Time.deltaTime;
            if (time <= 0.0f)
            {
                time = ResTime;//1秒沖に生成
                GameObject.Instantiate(PawnEnemy);
                resCount++;//出現数を増やす
            }
        }
    }
}
