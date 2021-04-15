using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;

public class BossMove : MonoBehaviour
{
    private GameObject Target;//追尾する相手
    private float dis;//プレイヤーとの距離
    //public float area;//この数値以下になったら追う

    Rigidbody rigid;

    private GameObject Enemy;

    [SerializeField, Header("体力")] float enemyHP = 5;
    [SerializeField, Header("この数値以下になったら攻撃が変わる")] float ChangeShotHP;

    
    [SerializeField, Header("発見時のスピード")]
    float speedLoc;

    [SerializeField, Header("突進時のスピード")]
    float RushSpeed;

    [SerializeField, Header("この数値まで進む")]
    float social;//この数値まで進む
    
    [Header("追う時のフラグ")]
     public bool MoveFlag = true;//追う

    [Header("突進のフラグ")]
     public bool RushFlag = false;//突進

    [SerializeField, Header("何秒止まるか")]
     float freezeTime;

     private float lookTime;

    [Header("何秒に実行するか")]
    float RushRunTime;

    [SerializeField,Header("突進のインターバル")]
    float RushIntarval;

    [SerializeField, Header("何秒止まるか")]
    GameObject RespawnPosition;


    void Start()
    {
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
            gameObject.SetActive(false);//非表示
            //SceneManager.LoadScene("Result");
            SceneManager.LoadScene("GameClear");
        }
        //召喚
        if(enemyHP<=ChangeShotHP)
        {
            MoveFlag = false;
            RushRunTime -= Time.deltaTime;
            RushFlag = false;

            RespawnPosition.SetActive(true);

        }

        dis = Vector3.Distance(transform.position, Target.transform.position);//二つの距離を計算して一定以下になれば追尾
        
        //追いかける
        if (MoveFlag)
        {
            this.transform.LookAt(new Vector3(Target.transform.position.x, this.transform.position.y, Target.transform.position.z));//ターゲットにむく
            if (dis >= social)
            {
                transform.position += transform.forward * speedLoc * Time.deltaTime;//前進(スピードが変わる)
            }
        }


        RushRunTime += Time.deltaTime;

        if(RushRunTime >=RushIntarval)
        {
            RushFlag = true;
            RushRunTime = 0;
        }

        //突進の処理
        if (RushFlag)
        {
            MoveFlag = false;
            lookTime += Time.deltaTime;

            //見つめてる
            if (lookTime <= freezeTime)
            {
                this.transform.LookAt(new Vector3(Target.transform.position.x, this.transform.position.y, Target.transform.position.z));//ターゲットにむく
            }
            //突進する
            if (lookTime >= freezeTime)
            {
                transform.position += transform.forward * RushSpeed * Time.deltaTime;//前進(スピードが変わる)
            }
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

        if (other.gameObject.CompareTag("Player")|| (other.gameObject.CompareTag("Wall")))
        {
            lookTime = 0;
            MoveFlag = true;
            RushFlag = false;
        }

    }
    private void OnDestroy()
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        DestroyImmediate(renderer.material); //マテリアルのメモリーを消す
    }

}
