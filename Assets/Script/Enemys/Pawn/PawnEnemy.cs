using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;

public class PawnEnemy : MonoBehaviour
{
    [SerializeField, Header("死んだ時のエフェクト")]
    private GameObject deathEffect;

    [SerializeField, Tooltip("最大体力")]
    private float enemyHP = 1;
    [SerializeField, Tooltip("スピード")]
    private float moveSpeed = 1;
    [SerializeField, Tooltip("死亡エフェクトがでるまでの時間")]
    private float deathEffectTime = 1.0f;
    //[SerializeField,Tooltip("このオブジェクトが消えるまでの時間")]
    private float deathTime = 0;
    [SerializeField, Tooltip("オブジェクトが飛んでいく力")]
    private float jumpPower = 18.0f;
    [SerializeField, Tooltip("オブジェクトの最大到達地点")]
    private float topHeightPoint = 5;

    private GameObject[] child;          //子どもオブジェクト
    private GameObject stageMove1;       //ステージムーブ
    private GameObject target;           //追尾する相手
    private Rigidbody rigid;             //物理演算
    private ParticleSystem deathParticle;//ダメージのパーティクル

    private float distance;//プレイヤーとの距離

    private int deathState;//死亡状態
    private int childCount;//子どもの数

    private bool moveFlag = false;  //プレイヤーを追いかけるか
    private bool isDeadFlag = false;//死んでいるか？
    

    //Renderer renderComponent;

    void Start()
    {
        //オブジェクトを取得
        target = GameObject.FindGameObjectWithTag("Player");
        stageMove1 = GameObject.FindGameObjectWithTag("StageMove");

        //コンポーネント取得
        deathParticle = deathEffect.GetComponent<ParticleSystem>();
        stageMove1.GetComponent<StageMove1>();
        rigid = GetComponent<Rigidbody>();

        //値初期化
        deathState = 0;
        moveFlag = true;
        isDeadFlag = false;

        //子どもの数を取得
        childCount = gameObject.transform.childCount;
        //配列を子どもオブジェクトの数で初期化
        child = new GameObject[childCount];
        //オブジェクトを代入していく
        for (int i = 0; i < childCount; i++)
        {
            child[i] = gameObject.transform.GetChild(i).gameObject;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();       //移動
        DeathAction();//死んだときの行動

    }

    /// <summary>
    /// 移動
    /// </summary>
    void Move()
    {
        if (isDeadFlag) return;

        //移動量を初期化
        rigid.angularVelocity = Vector3.zero;
        rigid.velocity = Vector3.zero;

        //二つの距離を計算して一定以下になれば追尾
        distance = Vector3.Distance(transform.position, target.transform.position);

        //追いかける
        if (moveFlag)
        {
            this.transform.LookAt(new Vector3(target.transform.position.x, this.transform.position.y, target.transform.position.z));//ターゲットにむく
            if (distance >= 1)
            {
                transform.position += transform.forward * moveSpeed * Time.deltaTime;//前進(スピードが変わる)
            }
        }

        if (stageMove1.GetComponent<StageMove1>().nowFlag == true)
        {
            moveFlag = false;
        }
        else if (!stageMove1.GetComponent<StageMove1>().nowFlag)
        {
            moveFlag = true;
        }
    }

    /// <summary>
    /// 死んだときの状態による行動の遷移
    /// </summary>
    void DeathAction()
    {
        switch (deathState)
        {
            case 0:
                //体力がなくなったら死亡&状態遷移
                if (enemyHP <= 0)
                {
                    deathState = 1;
                    isDeadFlag = true;
                }
                break;

            case 1:
                //Y軸にも動けるようにした後、上に移動する
                rigid.constraints = RigidbodyConstraints.None;
                rigid.constraints = RigidbodyConstraints.FreezeRotation;
                rigid.AddForce(Vector3.up * jumpPower);

                if(this.transform.position.y > topHeightPoint)
                {
                    this.transform.position = new Vector3(this.transform.position.x, topHeightPoint, this.transform.position.z);
                }

                //一定時間経過後、状態遷移
                deathTime += Time.deltaTime;
                if (deathTime > deathEffectTime)
                {
                    deathTime = 0;
                    deathState = 2;
                }
                break;

            case 2:
                //自身と、自身の子どもを非表示にする
                for (int i = 0; i < childCount; i++)
                {
                    child[i] = gameObject.transform.GetChild(i).gameObject;
                    child[i].SetActive(false);
                }

                //パーティクルオブジェクトを生成
                var sum = Instantiate(deathEffect, this.transform.position, Quaternion.identity);

                //状態遷移
                deathState = 3;
                break;

            case 3:
                //死亡した
                if (!stageMove1.GetComponent<StageMove1>().bossNow)
                {
                    TimerScript.enemyCounter += 1;
                }
                Destroy(this.gameObject);
                break;
        }
    }

    /// <summary>
    /// 体力を取得
    /// </summary>
    /// <returns></returns>
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
