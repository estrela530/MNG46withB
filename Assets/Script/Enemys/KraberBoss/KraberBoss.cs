﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class KraberBoss : MonoBehaviour
{
    private GameObject Target;//追尾する相手
    private float dis;//プレイヤーとの距離
    //public float area;//この数値以下になったら追う

    Rigidbody rigid;

    private GameObject Enemy;

    [SerializeField, Header("体力")]
    float enemyHP = 5;

    [SerializeField, Header("この値以下になったら攻撃変化")]
    float changeHP = 3;


    [Header("追う時のフラグ")]
    public bool MoveFlag = true;//追う

    [Header("攻撃のフラグ")]
    public bool AttackFlag;//

    [Header("拡散のフラグ")]
    public bool pawerFlag;//

    [SerializeField, Header("召喚するオブジェクト")]
    GameObject SummonEnemy;

    [SerializeField, Header("ここに召喚する")]
    GameObject SummonPosObj;

    [SerializeField, Header("召喚のエフェクト")]
    GameObject SummonEffect;

    [SerializeField, Header("召喚のエフェクトの魔法陣")]
    GameObject MagicCircle;

    private ParticleSystem SummonParticle;
    private int EffectCount;

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
    
    [SerializeField] private float DeathTime = 0;
    [SerializeField, Header("次のしーんに行くの開始までの時間")] float NextTime;
    [SerializeField, Header("次のシーンに行くフラグ")] bool NextFlag;

    [SerializeField, Header("死んだ時のエフェクト")]
    private GameObject DeathEffect;
    private ParticleSystem DeathParticle;   //ダメージのパーティクル

    int nextState = 0;

    Renderer renderComponent;
    [SerializeField] float ColorInterval = 0.1f;
    [SerializeField] float DamageTime;
    [SerializeField, Header("ダメージ受けた時")]
    bool DamageFlag = false;

    [SerializeField] Animation anime;

    void Start()
    {
        anime = GetComponent<Animation>();

        DeathParticle = DeathEffect.GetComponent<ParticleSystem>();
        SummonParticle = SummonEffect.GetComponent<ParticleSystem>();

        moveState = 0;
        bossShot.GetComponent<ToriteiShot>();
        pawerFlag = false;
        //Target = GameObject.Find("Player");//追尾させたいオブジェクトを書く
        Target = GameObject.FindGameObjectWithTag("Player");
        rigid = GetComponent<Rigidbody>();
        MoveFlag = true;
        NextFlag = false;
        AttackFlag = true;

        renderComponent = GetComponent<Renderer>();


    }

    //中断できる処理のまとまり
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
        rigid.angularVelocity = Vector3.zero;
        rigid.velocity = Vector3.zero;

        if (enemyHP > 0)
        {
            if (DamageFlag)
            {
                StartCoroutine("WaitForIt");
            }
        }

        if (MoveFlag)
        {


            //ターゲットにむく
            this.transform.LookAt(
                       new Vector3(Target.transform.position.x,
                       this.transform.position.y,
                       Target.transform.position.z));

            //攻撃の処理
            switch (moveState)
            {
                //撃つ、拡散、召喚どれか? 1～2が撃つ、3～4が召喚、5～6が拡散
                case 0:
                    if (bossShot.GetComponent<KraberBossShot>().shotCount >= 3)
                    {
                        moveState = 3;//召喚
                    }
                    else if (bossShot.GetComponent<KraberBossShot>().shotCount < 3 
                        && changeHP < enemyHP)
                    {
                        moveState = 1;//ノーマルショット
                    }

                    else if (bossShot.GetComponent<KraberBossShot>().shotCount < 3
                        && changeHP<=enemyHP )
                    {
                        moveState = 5;//拡散ショット
                    }
                    break;

                //見て撃つ
                case 1:
                    AttackFlag = true;

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
                               SummonPosObj.transform.position,
                                Quaternion.identity);
                            EnemyCount++;
                        }

                    }

                    //召喚のエフェクト
                    if (EffectCount < 1)
                    {
                        //エフェクトパーティクル
                        var eff = Instantiate(SummonEffect,
                               SummonPosObj.transform.position,
                               Quaternion.identity);

                        MagicCircle.SetActive(true);


                        EffectCount++;
                    }
                    break;

                //戻す
                case 4:

                    bossShot.GetComponent<KraberBossShot>().shotCount = 0;

                    SummonParticle.Stop();//パーティクルを消す

                    MagicCircle.SetActive(false);
                    moveState = 0;
                    EnemyCount = 0;
                    EffectCount = 0;
                    break;


                //見てるて撃つ
                case 5:
                    pawerFlag = true;

                    AttackFlag = false;

                    moveState = 6;

                    break;

                //戻す
                case 6:

                    moveState = 0;

                    break;

            }
            //this.transform.LookAt(new Vector3(Target.transform.position.x, this.transform.position.y, Target.transform.position.z));//ターゲットにむく

            dis = Vector3.Distance(transform.position, Target.transform.position);//二つの距離を計算して一定以下になれば追尾

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
                anime.Play();
                var sum = Instantiate(DeathEffect,
                           this.transform.position,
                           Quaternion.identity);

                nextState = 2;

                bossShot.GetComponent<KraberBossShot>().shotCount = 0;
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
