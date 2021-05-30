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

    float dethTime;

    [SerializeField, Header("死んだ時のエフェクト")]
    private GameObject DeathEffect;
    private ParticleSystem DeathParticle;   //ダメージのパーティクル

    //レイ関連
    Ray ray;
    RaycastHit hitRay;
    LineRenderer lineRenderer;

    void Start()
    {
        //Target = GameObject.Find("Player");//追尾させたいオブジェクトを書く
        Target = GameObject.FindGameObjectWithTag("Player");
        rigid = GetComponent<Rigidbody>();
        MoveFlag = true;


        stageMove1 = GameObject.FindGameObjectWithTag("StageMove");
        stageMove1.GetComponent<StageMove1>();
        //KeyObject.SetActive(false);
    }


    // Update is called once per frame
    void FixedUpdate()
    {
       

        DeathParticle = DeathEffect.GetComponent<ParticleSystem>();
        rigid.angularVelocity = Vector3.zero;
        rigid.velocity = Vector3.zero;
        if (stageMove1.GetComponent<StageMove1>().nowFlag == true)
        {
            MoveFlag = false;
        }
        if (MoveFlag)
        {
            //召喚
            Pawn();
            dis = Vector3.Distance(transform.position, Target.transform.position);//二つの距離を計算して一定以下になれば追尾

            //向く
            this.transform.LookAt(new Vector3(Target.transform.position.x, this.transform.position.y, Target.transform.position.z));//ターゲットにむく

            //前進
            transform.position += -transform.forward * RunSpeed * Time.deltaTime;//前進(スピードが変わる)
           
        }
        if(enemyHP >= 1)
        {
            KeyObject.SetActive(false);
        }
        //死んだら鍵を出す
        if (enemyHP <= 0 && !stageMove1.GetComponent<StageMove1>().bossNow)
        {
            KeyObject.transform.position = this.transform.position;
            KeyObject.SetActive(true);
            dethTime += Time.deltaTime;
            if (dethTime>2)
            {
                MoveFlag = false;
                TimerScript.enemyCounter += 1;
                var sum = Instantiate(DeathEffect,
                               this.transform.position,
                               Quaternion.identity);

                Destroy(this.gameObject);

                gameObject.SetActive(false);//非表示
                dethTime = 0;
            }
           
            
        }
        
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
                    new Vector3(this.transform.position.x, transform.position.y, this.transform.position.z),
                    Quaternion.identity);
                PawnCount++;
            }
            PawnCount = 0;
        }
    }


    public int HpGet()
    {
        return (int)enemyHP;
    }


    //(仮)指定されたtagに当たると消える
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Fragment"))
        {
            enemyHP = enemyHP - 1;
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
