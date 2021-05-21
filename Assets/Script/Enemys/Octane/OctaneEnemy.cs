using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;

//[RequireComponent(typeof(BoxCollider))]

public class OctaneEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject Target;//追尾する相手
    private float dis;//プレイヤーとの距離
    // public float area;//この数値以下になったら追う

    [Header("体力")]public float enemyHP = 5;

    Rigidbody rigid;

    private float workeAria1 = 1;//
    private float workeAria2 = 1;//

    private float Rspeed;

    private float ww;
    private float ww2;
    Player player;
    Color color;
    //Player player;
    
    int workNumber = 1;
    
    [Header("発見時のスピード")]
    public float speedLoc;

    [Header("この数値まで進む")] public float social;//この数値まで進む
    private GameObject Enemy;

    [Header("追う時")]
    public bool MoveFlag = true;//追う
    //public bool lookFlag = false;

    [SerializeField, Header("何秒止まるか")]
    public float freezeTime;
    public float lookTime;

    //召喚関連
    [SerializeField, Header("召喚したエネミーの数")]
    int EnemyCount;//プレハブの出現数
    

    [SerializeField, Header("召喚するオブジェクト")]
    GameObject SummonEnemy;

    [SerializeField,Header("ここに召喚する")]
     GameObject SummonPosObj;

    //[SerializeField, Header("召喚エフェクトのポジション")]
    //GameObject EffectPosObj;

    [SerializeField, Header("召喚のエフェクト")]
     GameObject SummonEffect;
    [SerializeField, Header("召喚のエフェクトの魔法陣")]
    GameObject MagicCircle;

    private ParticleSystem SummonParticle;
    private int EffectCount;

    [SerializeField, Header("次からの生成時間")]
    float ResetTime;

    [SerializeField, Header("生成までの時間")]
    float PawnTime;

    [SerializeField, Header("召喚するエネミーの上限")]
    int MaxEnemyCount;//プレハブの出現数

    int enemyNumber = (1<<13| 1 << 8);


    [SerializeField] private float DeathTime = 0;

    [SerializeField, Header("次のしーんに行くの開始までの時間")]
    float NextTime;

    [SerializeField, Header("次のシーンに行くフラグ")]
    bool NextFlag;

    int nextState = 0;

    int effectChildCount;

    GameObject[] childs;


    [SerializeField] GameObject BossHpSlider;


    //レイ関連
    Ray ray;
    RaycastHit hitRay;
    LineRenderer lineRenderer;
    
    Vector3 velocity = Vector3.zero;

    public int moveState;
    public int attackCount;//突進した回数

   GameObject stageMove1;


    Renderer renderComponent;
    [SerializeField] float ColorInterval = 0.1f;
    [SerializeField] float DamageTime;
    [SerializeField, Header("ダメージ受けた時")]
    bool DamageFlag = false;

    [SerializeField, Header("死んだ時のエフェクト")]
    private GameObject DeathEffect;
    private ParticleSystem DeathParticle;   //ダメージのパーティクル

    // Start is called before the first frame update
    void Start()
    {
        EffectCount = 0;

        BossHpSlider.SetActive(false);

        NextFlag = false;

        DeathParticle = DeathEffect.GetComponent<ParticleSystem>();
        SummonParticle = SummonEffect.GetComponent<ParticleSystem>();

        attackCount = 0;
        renderComponent = GetComponent<Renderer>();

        
        
        moveState = 0;
        stageMove1 = GameObject.FindGameObjectWithTag("StageMove");
        stageMove1.GetComponent<StageMove1>();
        Target = GameObject.FindGameObjectWithTag("Player");
        rigid = GetComponent<Rigidbody>();
        color = GetComponent<Renderer>().material.color;

        ray = new Ray();
        lineRenderer = this.gameObject.GetComponent<LineRenderer>();

        //lineRenderer.SetPosition(0, this.transform.position);
        lineRenderer.enabled = false;
        ray.origin = this.transform.position;//自分の位置のレイ

        //ラインレンダラーの色
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.green;//初めの色
        lineRenderer.endColor = Color.green;//終わりの色

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
        if(stageMove1.GetComponent<StageMove1>().bossNow)
        {
            BossHpSlider.SetActive(true);
        }
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


        if (stageMove1.GetComponent<StageMove1>().nowFlag == true)
        {
            MoveFlag = false;
            moveState = 0;
        }

        switch (moveState)
        {
            //召喚、突進どっちか? 1～3が突進、5
            case 0:
                if (attackCount >= 3)
                {
                    moveState = 5;//召喚
                }
                else if(attackCount<3)
                {
                    moveState = 1;//突進
                }
                break;

            //見てる時
            case 1:
                lookTime += Time.deltaTime;

                if (lookTime <= freezeTime)
                {
                    this.transform.LookAt(new Vector3(Target.transform.position.x, this.transform.position.y, Target.transform.position.z));//ターゲットにむく

                    //レイの処理
                    if (Physics.Raycast(ray, out hitRay, 30,enemyNumber))
                    {
                        if (hitRay.collider.gameObject.CompareTag("Player"))
                        {
                            lineRenderer.enabled = true;

                            lineRenderer.SetPosition(1, hitRay.point);
                            //hitRay.point;
                        }

                    }
                }

                if (lookTime >= freezeTime - 0.5f)
                {
                    lineRenderer.startColor = Color.red;//初めの色
                    lineRenderer.endColor = Color.red;//終わりの色
                }

                if (lookTime >= freezeTime)
                {
                    moveState = 2;
                }
                break;

            //位置を取得と攻撃カウントを+1する
            case 2:
                //playerPos = Target.transform.position;
                attackCount = attackCount + 1;
                //this.transform.LookAt(new Vector3(playerPos.x, this.transform.position.y, playerPos.z));//ターゲットにむく
                moveState = 3;
                break;
            
                //突進
            case 3:
                lineRenderer.startColor = Color.green;//初めの色
                lineRenderer.endColor = Color.green;//終わりの色

                //transform.position = Vector3.MoveTowards(transform.position, 
                //    new Vector3(playerPos.x, this.transform.position.y, playerPos.z), 
                //    speedLoc * Time.deltaTime);
                transform.position += transform.forward * speedLoc * Time.deltaTime;//前進(スピードが変わる)

                lineRenderer.enabled = false;//(弾が間にいると点滅みたいになる)

                lookTime = 0;

                //if (/*playerPos.x == transform.position.x*/
                //    /*&&*/ playerPos.z == transform.position.z)
                //{
                //    moveState = 4;
                //}
                break;

            //戻す
            case 4:
                moveState = 0;
                break;

           
            case 5:
                if (EnemyCount == MaxEnemyCount)
                {
                    moveState = 6;
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
                        //EffectCount = 0;
                        
                    }
                    //moveState = 6;

                }
                //召喚のエフェクト
                if (EffectCount < 1)
                {
                    //エフェクトパーティクル
                    var eff = Instantiate(SummonEffect,
                           SummonPosObj.transform.position,
                           Quaternion.identity);
                    //SummonEffect.SetActive(true);

                    ////エフェクト画像
                    //var effM = Instantiate(MagicCircle,
                    //       SummonPosObj.transform.position,
                    //       Quaternion.identity);
                    MagicCircle.SetActive(true);

                    //effectChildCount = SummonEffect.transform.childCount;

                    //childs = new GameObject[effectChildCount];

                    //for (int i = 0; i < effectChildCount; i++)
                    //{
                    //    childs[i] = SummonEffect.transform.GetChild(i).gameObject;
                    //}

                    EffectCount++;
                }
                

                break;
            //戻す
            case 6:
                //for (int i = 0; i < effectChildCount; i++)
                //{
                //    Destroy(childs[i]);
                //}
                SummonParticle.Stop();//パーティクルを消す
                //Destroy(MagicCircle);//画像を消す
                MagicCircle.SetActive(false);
                attackCount = 0;
                moveState = 0;
                EnemyCount = 0;
                EffectCount = 0;
                break;

        }
        switch(nextState)
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

                attackCount = 0;
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
        

        dis = Vector3.Distance(transform.position, Target.transform.position);//二つの距離を計算して一定以下になれば追尾
        
        //レイ
        lineRenderer.SetPosition(0, this.transform.position);

        ray.origin = this.transform.position;//自分の位置のレイ

        ray.direction = transform.forward;//自分の向きのレイ

        Debug.DrawRay(ray.origin, ray.direction * 10, Color.red, 0.1f);
        
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
            DamageFlag = true;
        }
        if (other.gameObject.CompareTag("Wall")
            /*|| other.gameObject.CompareTag("Enemy")*/)
        {
            moveState = 4;
            MoveFlag = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //MoveFlag = false;
            moveState = 4;
        }
        

    }

    private void OnDestroy()
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        DestroyImmediate(renderer.material); //マテリアルのメモリーを消す
    }
}
