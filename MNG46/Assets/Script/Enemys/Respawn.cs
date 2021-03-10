using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{

    private GameObject Res;//
    private GameObject Target;//

    private float disRes;//プレイヤーとの距離
    public float area;//この数値以下に出現する

    public GameObject Enemy;
    
    private int resCount;//プレハブの出現数

    public GameObject[] bases;
    private float time;

    // Start is called before the first frame update
    void Start()
    {
        Target = GameObject.Find("Player");//距離を知りたいオブジェクトを書く
    }

    // Update is called once per frame
    void Update()
    {
        disRes = Vector3.Distance(transform.position, Target.transform.position);//二つの距離を計算

        //エリア以下の数値になったらデル
        if (disRes < area)
        {
            Resp();
        }
        
    }

    void Resp()
    {
        if(resCount == bases.Length)//
        {
            time -= Time.deltaTime;
            if (time <= 0.0f)
            {
                time = 1.0f;//1秒沖に生成
                GameObject.Instantiate(Enemy);
                resCount++;//出現数を増やす
            }
        }
    }
}
