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

    [SerializeField, Header("この数値以下になったら変わる")]
    float ChangeAttackHP;

    
    //[SerializeField, Header("発見時のスピード")]
    //float speedLoc;

    [SerializeField, Header("突進時のスピード")]
    float RushSpeed;
    
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

    bool PawnFalg;

    [SerializeField] private float DeathTime = 0;
    [SerializeField, Header("次のしーんに行くの開始までの時間")]  float NextTime;
    [SerializeField,Header("次のシーンに行くフラグ")] bool NextFlag;

    //レイ関連
    Ray ray;
    RaycastHit hitRay;
    LineRenderer lineRenderer;

    Renderer renderComponent;
    [SerializeField] float ColorInterval = 0.1f;
    [SerializeField] float DamageTime;
    [SerializeField, Header("ダメージ受けた時")]
    bool DamageFlag = false;

    void Start()
    {
        PawnFalg = false;
        //Target = GameObject.Find("Player");//追尾させたいオブジェクトを書く
        Target = GameObject.FindGameObjectWithTag("Player");
        rigid = GetComponent<Rigidbody>();
        MoveFlag = true;
        NextFlag = false;

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

            //this.transform.LookAt(new Vector3(Target.transform.position.x, this.transform.position.y, Target.transform.position.z));//ターゲットにむく
            //召喚
            //if (enemyHP <= ChangeAttackHP)
            //{
            //    Pawn();
            //}

            dis = Vector3.Distance(transform.position, Target.transform.position);//二つの距離を計算して一定以下になれば追尾

            lineRenderer.SetPosition(0, this.transform.position);

          
            //レイの処理
            if (Physics.Raycast(ray, out hitRay, 20))
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
            
            ////追いかける
            //if (MoveFlag)
            //{
            //    this.transform.LookAt(new Vector3(Target.transform.position.x, this.transform.position.y, Target.transform.position.z));//ターゲットにむく
            //}

            //突進を実行するまで時間を足す
            RushRunTime += Time.deltaTime;

            if (RushRunTime >= RushIntarval)
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
        if (enemyHP <= 0)
        {
            MoveFlag = false;
            DeathTime += Time.deltaTime;
            if(DeathTime > NextTime)
            {
                NextFlag = true;
                DeathTime = 0;
            }
        }

        if(NextFlag)
        {
            gameObject.SetActive(false);//非表示
            //SceneManager.LoadScene("Result");
            SceneManager.LoadScene("GameClear");
        }
       
    }

    //召喚する処理
    void Pawn()
    {
        //カウントの値まで生成
        if (PawnCount<MaxPawnCount)
        {
            PawnTime -= Time.deltaTime;
            if(PawnTime <= 0.0f)
            {
                PawnTime = ResetTime;//1秒沖に生成
                GameObject.Instantiate(PawnEnemy);
                //GameObject.Instantiate(PawnEnemy);
                PawnCount++;
            }
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
