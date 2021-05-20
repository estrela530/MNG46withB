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

    [SerializeField, Header("体力")]
    float enemyHP = 5;
    
    
    [Header("追う時のフラグ")]
     public bool MoveFlag = true;//追う

    [Header("攻撃のフラグ")]
     public bool AttackFlag;//
    
    [SerializeField, Header("召喚するオブジェクト")]
    GameObject SummonEnemy;

    [SerializeField, Header("ここに召喚する")]
    GameObject SummonPosObj;

    [SerializeField, Header("次の生成時間")]
    float ResetTime;

    [SerializeField, Header("生成までの時間")]
    float PawnTime;

    [SerializeField, Header("エネミーの出現数")]
    int EnemyCount;//プレハブの出現数

    [SerializeField, Header("召喚するエネミーの上限")]
    int MaxEnemyCount;//プレハブの出現数

    [SerializeField, Header("ボスショット")]
    GameObject bossShot;

    public int moveState;

    bool PawnFalg;

    [SerializeField] private float DeathTime = 0;
    [SerializeField, Header("次のしーんに行くの開始までの時間")]  float NextTime;
    [SerializeField,Header("次のシーンに行くフラグ")] bool NextFlag;

    [SerializeField, Header("死んだ時のエフェクト")]
    private GameObject DeathEffect;
    private ParticleSystem DeathParticle;   //ダメージのパーティクル

    int nextState = 0;

    //レイ関連
    Ray ray;
    RaycastHit hitRay;
    LineRenderer lineRenderer;
    int enemyNumber = (1 << 13);

    Renderer renderComponent;
    [SerializeField] float ColorInterval = 0.1f;
    [SerializeField] float DamageTime;
    [SerializeField, Header("ダメージ受けた時")]
    bool DamageFlag = false;



    void Start()
    {
        DeathParticle = DeathEffect.GetComponent<ParticleSystem>();

        moveState = 0;
        bossShot.GetComponent<BossShot>();
        PawnFalg = false;
        //Target = GameObject.Find("Player");//追尾させたいオブジェクトを書く
        Target = GameObject.FindGameObjectWithTag("Player");
        rigid = GetComponent<Rigidbody>();
        MoveFlag = true;
        NextFlag = false;
        AttackFlag = true;

        renderComponent = GetComponent<Renderer>();

        ray = new Ray();
        lineRenderer = this.gameObject.GetComponent<LineRenderer>();

        //lineRenderer.SetPosition(0, this.transform.position);
        lineRenderer.enabled = false;
        ray.origin = this.transform.position;//自分の位置のレイ

        //ラインレンダラーの色
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.red;//初めの色
        lineRenderer.endColor = Color.red;//終わりの色

        lineRenderer.startWidth = 0.5f;
        lineRenderer.endWidth = 0.5f;

        //変えるかも?
        ray.direction = transform.forward;
    }

    //中断できる処理のまとまり
    IEnumerator Blink()
    {
        while (true)
        {
            renderComponent.enabled = !renderComponent.enabled;
            //何フレームとめる
            yield return new WaitForSeconds(ColorInterval);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rigid.angularVelocity = Vector3.zero;
        rigid.velocity = Vector3.zero;

        //ダメージ演出
        if (enemyHP > 0)
        {
            //ダメージ
            if (DamageFlag)
            {
                DamageTime += Time.deltaTime;
                StartCoroutine("Blink");
                if (DamageTime > 1)
                {
                    DamageTime = 0;
                    StopCoroutine("Blink");
                    renderComponent.enabled = true;
                    DamageFlag = false;
                }
            }
        }

        if (MoveFlag)
        {
            //レイの処理
            if (Physics.Raycast(ray, out hitRay, 20, enemyNumber))
            {
                if (hitRay.collider.gameObject.CompareTag("Player"))
                {
                    lineRenderer.enabled = true;

                    lineRenderer.SetPosition(1, hitRay.point);

                }
                else
                {
                    lineRenderer.enabled = false;//(弾が間にいると点滅みたいになる)
                }
            }

            ray.origin = this.transform.position;//自分の位置のレイ

            ray.direction = transform.forward;//自分の向きのレイ

            //ターゲットにむく
            this.transform.LookAt(
                       new Vector3(Target.transform.position.x,
                       this.transform.position.y,
                       Target.transform.position.z));

            switch (moveState)
            {
                //撃つ、召喚どっちか? 1～2が撃つ、3～4が召喚
                case 0:
                    if (bossShot.GetComponent<BossShot>().shotCount >= 3)
                    {
                        moveState = 3;//召喚
                    }
                    else if (bossShot.GetComponent<BossShot>().shotCount < 3)
                    {
                        moveState = 1;//
                    }
                    break;

                //見てるて撃つ
                case 1:
                    AttackFlag = true;
                   

                    lineRenderer.startColor = Color.red;//初めの色
                    lineRenderer.endColor = Color.red;//終わりの色

                    moveState = 2;

                    break;
                    
                //戻す
                case 2:
                    
                    moveState = 0;

                    break;
                    
                case 3:
                    if (EnemyCount == MaxEnemyCount)
                    {
                        moveState = 4;
                    }
                    //カウントの値まで生成
                    if (EnemyCount < MaxEnemyCount)
                    {
                        PawnTime -= Time.deltaTime;
                        if (PawnTime <= 0.0f)
                        {
                            PawnTime = ResetTime;//1秒沖に生成
                            var sum = Instantiate(SummonEnemy,
                                new Vector3(
                                     SummonPosObj.transform.position.x, 
                                     transform.position.y,
                                     SummonPosObj.transform.position.z),
                                Quaternion.identity);

                            EnemyCount++;
                        }
                        
                    }
                    break;

                //戻す
                case 4:
                    moveState = 0;
                    EnemyCount = 0;
                    bossShot.GetComponent<BossShot>().shotCount = 0;
                    break;
                    

            }
            //this.transform.LookAt(new Vector3(Target.transform.position.x, this.transform.position.y, Target.transform.position.z));//ターゲットにむく
           
            dis = Vector3.Distance(transform.position, Target.transform.position);//二つの距離を計算して一定以下になれば追尾

            lineRenderer.SetPosition(0, this.transform.position);
        }

        switch (nextState)
        {
            case 0:
                if (enemyHP <= 0)
                {
                    nextState = 1;
                }
                break;

            case 1:

                var sum = Instantiate(DeathEffect,
                           this.transform.position,
                           Quaternion.identity);

                nextState = 2;

                bossShot.GetComponent<BossShot>().shotCount = 0;
                moveState = 0;
                EnemyCount = 5;

                break;

            case 2:
                DeathTime += Time.deltaTime;
                if (DeathTime > NextTime)
                {

                    DeathTime = 0;

                    nextState = 3;
                }


                break;

            case 3:
                SceneManager.LoadScene("GameClear");
                gameObject.SetActive(false);//非表示
                break;

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
            DamageFlag = true;
        }

        if (other.gameObject.CompareTag("Player")|| (other.gameObject.CompareTag("Wall")))
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
