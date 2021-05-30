using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyEnemy : MonoBehaviour
{
    private GameObject Target;//追尾する相手
    private float dis;//プレイヤーとの距離
    //public float area;//この数値以下になったら追う

    Rigidbody rigid;

    private GameObject Enemy;

    [SerializeField, Header("体力")]
    float enemyHP = 5;
    
    [SerializeField, Header("逃げのスピード")]
    float RunSpeed;

    [Header("見つけた時のフラグ")]
    public bool MoveFlag = false;
    

    [SerializeField, Header("何秒止まるか")]
    float freezeTime;
    
    
    [SerializeField, Header("召喚するオブジェクト")]
    GameObject PawnEnemy;

    [SerializeField, Header("次の生成時間")]
    float ResetTime;

    [SerializeField, Header("生成までの時間")]
    float PawnTime;

    [SerializeField, Header("エネミーの出現数")]
    int PawnCount;//プレハブの出現数

    [SerializeField, Header("召喚するエネミーの上限")]
    int MaxPawnCount;//プレハブの出現数

    [SerializeField, Header("鍵オブジェクト")]
    GameObject KeyObject;

    GameObject stageMove1;

    [SerializeField, Header("最初の位置")] Vector3 startPos;


    [SerializeField, Header("ダメージ受けた時")]
    bool DamageFlag;

    float dethTime;

    [SerializeField, Header("死んだ時のエフェクト")]
    private GameObject deathEffect;
    private ParticleSystem DeathParticle;   //ダメージのパーティクル

    //レイ関連
    Ray ray;
    RaycastHit hitRay;
    LineRenderer lineRenderer;

    private int deathState;//死亡状態
    private bool isDeadFlag = false;//死んでいるか？
    [SerializeField, Tooltip("オブジェクトが飛んでいく力")]
    private float jumpPower = 18.0f;
    [SerializeField, Tooltip("オブジェクトの最大到達地点")]
    private float topHeightPoint = 5;
    private float deathTime = 0;
    [SerializeField, Tooltip("死亡エフェクトがでるまでの時間")]
    private float deathEffectTime = 1.0f;
    private int childCount;//子どもの数
    private GameObject[] child;          //

    void Start()
    {
        //Target = GameObject.Find("Player");//追尾させたいオブジェクトを書く
        Target = GameObject.FindGameObjectWithTag("Player");
        rigid = GetComponent<Rigidbody>();
        MoveFlag = true;

        startPos = GetComponent<Transform>().position;//最初のポジション
        Transform myTransform = this.transform;

        stageMove1 = GameObject.FindGameObjectWithTag("StageMove");
        stageMove1.GetComponent<StageMove1>();

        //子どもの数を取得
        childCount = gameObject.transform.childCount;
        //配列を子どもオブジェクトの数で初期化
        child = new GameObject[childCount];
        //オブジェクトを代入していく
        for (int i = 0; i < childCount; i++)
        {
            child[i] = gameObject.transform.GetChild(i).gameObject;
        }
        deathState = 0;
        isDeadFlag = false;
        KeyObject.SetActive(false);
    }

    IEnumerator WaitForIt()
    {
        // 1秒間処理を止める
        yield return new WaitForSeconds(1);

        // １秒後ダメージフラグをfalseにして点滅を戻す
        DamageFlag = false;
        gameObject.GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f, 1f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        DeathAction();

        if (isDeadFlag) return;

        rigid.angularVelocity = Vector3.zero;
        rigid.velocity = Vector3.zero;

        DeathParticle = deathEffect.GetComponent<ParticleSystem>();
        rigid.angularVelocity = Vector3.zero;
        rigid.velocity = Vector3.zero;
        if (stageMove1.GetComponent<StageMove1>().nowFlag == true)
        {
            MoveFlag = false;
        }

        if (enemyHP > 0)
        {
            if (DamageFlag)
            {
                StartCoroutine("WaitForIt");
            }
        }

        //上に行かない処理
        if (this.transform.position.y < startPos.y)
        {
            Vector3 resetPos = new Vector3(transform.position.x, startPos.y, transform.position.z);

            this.transform.position = resetPos;
        }

        if (MoveFlag)
        {
            //召喚
            Pawn();
            dis = Vector3.Distance(transform.position, Target.transform.position);//二つの距離を計算して一定以下になれば追尾

            
            this.transform.LookAt(new Vector3(Target.transform.position.x, this.transform.position.y, Target.transform.position.z));//ターゲットにむく
            
            //背を向く
            //myTransform.Rotate(0, 180, 0);

            //前進
            transform.position += -transform.forward * RunSpeed * Time.deltaTime;//前進(スピードが変わる)
           
        }
        if(enemyHP >= 1)
        {
            KeyObject.SetActive(false);
        }
        //死んだら鍵を出す
        //if (enemyHP <= 0 && !stageMove1.GetComponent<StageMove1>().bossNow)
        //{
            

        //   dethTime += Time.deltaTime;
        //    if (dethTime>2)
        //    {
        //        MoveFlag = false;
        //        TimerScript.enemyCounter += 1;
        //        var sum = Instantiate(DeathEffect,
        //                       this.transform.position,
        //                       Quaternion.identity);

        //        Destroy(this.gameObject);
        //        PawnCount = 0;
        //        gameObject.SetActive(false);//非表示
        //        dethTime = 0;
        //    }
           
            
        //}
        
    }

    //召喚する処理
    void Pawn()
    {
        //カウントの値まで生成
        if (PawnCount < MaxPawnCount)
        {
            PawnTime -= Time.deltaTime;
            if (PawnTime <= 0.0f)
            {
                PawnTime = ResetTime;//1秒沖に生成
                var sum = Instantiate(PawnEnemy,
                    new Vector3(this.transform.position.x-0.1f, transform.position.y, this.transform.position.z-0.1f),
                    Quaternion.identity);
                PawnCount++;
            }
            PawnCount = 0;
        }
    }

    void DeathAction()
    {
        switch (deathState)
        {
            case 0:
                //体力がなくなったら死亡&状態遷移
                if (enemyHP <= 0 && !stageMove1.GetComponent<StageMove1>().bossNow)
                {
                    deathState = 1;
                    isDeadFlag = true;
                    DamageFlag = true;

                    MoveFlag = false;

                    KeyObject.transform.position =new Vector3( 
                        this.transform.position.x,
                        KeyObject.transform.position.y,
                        this.transform.position.z);

                    KeyObject.SetActive(true);

                    PawnCount += 2;
                }
                break;

            case 1:
                //Y軸にも動けるようにした後、上に移動する
                rigid.constraints = RigidbodyConstraints.None;
                rigid.constraints = RigidbodyConstraints.FreezeRotation;
                rigid.AddForce(Vector3.up * jumpPower);

                if (this.transform.position.y > topHeightPoint)
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


    public int HpGet()
    {
        return (int)enemyHP;
    }
    public bool DamageGet()
    {
        return DamageFlag;
    }

    //(仮)指定されたtagに当たると消える
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Fragment"))
        {
            enemyHP = enemyHP - 1;

            DamageFlag = true;
        }

        if (other.gameObject.CompareTag("Player") || (other.gameObject.CompareTag("Wall")))
        {
            MoveFlag = true;
        }

    }
    private void OnDestroy()
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        DestroyImmediate(renderer.material); //マテリアルのメモリーを消す
    }
}
