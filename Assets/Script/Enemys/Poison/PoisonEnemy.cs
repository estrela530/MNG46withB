using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;

public class PoisonEnemy : MonoBehaviour
{
    
    private GameObject Target;//追尾する相手
    private float dis;//プレイヤーとの距離

    [SerializeField, Header("体力")] float enemyHP;
    [SerializeField, Header("最大体力")] float MaxEnemyHP;
    [SerializeField] GameObject BossHP;

    Rigidbody rigid;
    
    private float Rspeed;
    

    //[Header("この数値まで進む")] public float social;//この数値まで進む
    //private GameObject Enemy;

    [Header("追う時と索敵のフラグ")]
    public bool MoveFlag = false;//追う
    //public bool workFlag = true;//徘徊

    //レイ関連
    Ray ray;
    RaycastHit hitRay;
    LineRenderer lineRenderer;
    int enemyNumber = (1 << 13 | 1 << 8 | 1 << 9);

    Renderer renderComponent;
    [SerializeField] float ColorInterval = 0.1f;
    [SerializeField] float DamageTime;
    [SerializeField, Header("ダメージ受けた時")]
    bool DamageFlag = false;

    [SerializeField, Header("死んだ時のエフェクト")]
    private GameObject deathEffect;
    private ParticleSystem DeathParticle;   //ダメージのパーティクル

    //追加
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
    private GameObject[] child;

    GameObject stageMove1;

    
    void Start()
    {
        MaxEnemyHP = enemyHP;
        DeathParticle = deathEffect.GetComponent<ParticleSystem>();
        //Target = GameObject.Find("Player");//追尾させたいオブジェクトを書く
        Target = GameObject.FindGameObjectWithTag("Player");
        rigid = GetComponent<Rigidbody>();

        stageMove1 = GameObject.FindGameObjectWithTag("StageMove");
        stageMove1.GetComponent<StageMove1>();

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

        BossHP.SetActive(false);

    }

    void OnEnable()
    {
        BossHP.SetActive(true);
    }

    //中断できる処理のまとまり
    //IEnumerator Blink()
    //{
    //    while (true)
    //    {
    //        renderComponent.enabled = !renderComponent.enabled;
    //        //何フレームとめる
    //        yield return new WaitForSeconds(ColorInterval);
    //    }
    //}

    // Update is called once per frame
    void FixedUpdate()
    {
        DeathAction();

        if (isDeadFlag) return;

        rigid.angularVelocity = Vector3.zero;
        rigid.velocity = Vector3.zero;

        //最大体力以上にはならない。
        if (enemyHP >= MaxEnemyHP)
        {
            enemyHP = MaxEnemyHP;
        }

        //ダメージ演出
        if (enemyHP > 0)
        {
            //ダメージ
            if (DamageFlag)
            {
                DamageTime += Time.deltaTime;
               // StartCoroutine("Blink");
                if (DamageTime > 1)
                {
                    DamageTime = 0;
                    //StopCoroutine("Blink");
                    //renderComponent.enabled = true;
                    DamageFlag = false;
                }
            }
        }
        //常にターゲットにむく
        this.transform.LookAt(new Vector3(Target.transform.position.x, this.transform.position.y, Target.transform.position.z));

       

        if (stageMove1.GetComponent<StageMove1>().nowFlag == true)
        {
            MoveFlag = false;
        }

        

        dis = Vector3.Distance(transform.position, Target.transform.position);//二つの距離を計算して一定以下になれば追尾
        
        
        lineRenderer.SetPosition(0, this.transform.position);

        if (!Physics.Raycast(ray, out hitRay, 20,enemyNumber))
        {
            lineRenderer.enabled = false;
            MoveFlag = false;
        }
        //レイの処理
        if (Physics.Raycast(ray, out hitRay, 20,enemyNumber))
        {
            if (hitRay.collider.gameObject.CompareTag("Player"))
            {
                lineRenderer.enabled = true;

                MoveFlag = true;

                lineRenderer.SetPosition(1, hitRay.point);

            }
            else
            {
                lineRenderer.enabled = false;//(弾が間にいると点滅みたいになる)
                MoveFlag = false;
            }
        }

        ray.origin = this.transform.position;//自分の位置のレイ

        ray.direction = transform.forward;//自分の向きのレイ
        

    }
    void DeathAction()
    {
        switch (deathState)
        {
            case 0:
                //体力がなくなったら死亡&状態遷移
                if (enemyHP <= 0 )
                {
                    deathState = 1;
                    isDeadFlag = true;
                    DamageFlag = true;
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
    public float HpGet()
    {
        return enemyHP;
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

    }
    private void OnDestroy()
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        DestroyImmediate(renderer.material); //マテリアルのメモリーを消す
    }
}
