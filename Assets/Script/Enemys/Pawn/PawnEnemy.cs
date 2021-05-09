using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;

public class PawnEnemy : MonoBehaviour
{
    private GameObject Target;//追尾する相手
    private float dis;//プレイヤーとの距離
    //public float area;//この数値以下になったら追う

    Rigidbody rigid;

    private GameObject Enemy;

    [SerializeField, Header("体力")] float enemyHP = 2;


    [SerializeField, Header("スピード")]
    float speedLoc;
    
    [Header("追う時のフラグ")]
    public bool MoveFlag;//追う


    GameObject stageMove1;


    void Start()
    {
        stageMove1 = GameObject.FindGameObjectWithTag("StageMove");
        stageMove1.GetComponent<StageMove1>();
        //Target = GameObject.Find("Player");//追尾させたいオブジェクトを書く
        Target = GameObject.FindGameObjectWithTag("Player");
        rigid = GetComponent<Rigidbody>();
        MoveFlag = true;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        rigid.angularVelocity = Vector3.zero;
        rigid.velocity = Vector3.zero;

       

        if (enemyHP <= 0)
        {
            //Destroy(transform.parent);
            Destroy(this.gameObject);
        }


        dis = Vector3.Distance(transform.position, Target.transform.position);//二つの距離を計算して一定以下になれば追尾



        

        //追いかける
        if (MoveFlag)
        {
            this.transform.LookAt(new Vector3(Target.transform.position.x, this.transform.position.y, Target.transform.position.z));//ターゲットにむく
            if (dis >= 1)
            {
                transform.position += transform.forward * speedLoc * Time.deltaTime;//前進(スピードが変わる)
            }
        }
        Debug.Log("動くff" + MoveFlag);

        if (stageMove1.GetComponent<StageMove1>().nowFlag == true)
        {
            MoveFlag = false;
        }

    }

    public float HpGet()
    {
        return enemyHP;
    }


    //(仮)指定されたtagに当たると消える
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Fragment"))
        {
            enemyHP = enemyHP - 1;
        }


    }
    private void OnDestroy()
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        DestroyImmediate(renderer.material); //マテリアルのメモリーを消す
    }

}
