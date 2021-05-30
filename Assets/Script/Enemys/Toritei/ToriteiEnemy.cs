using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToriteiEnemy : MonoBehaviour
{
    private GameObject Target;//追尾する相手
    private float dis;//プレイヤーとの距離
                      // public float area;//この数値以下になったら追う

    [SerializeField, Header("体力")] float enemyHP;

    [SerializeField, Header("最大体力")] float MaxEnemyHP;


    Rigidbody rigid;

    //Color color;


    [Header("追う時と索敵のフラグ")]
    public bool MoveFlag = false;//追う

    GameObject stageMove1;

    Renderer renderComponent;
    [SerializeField] float ColorInterval = 0.1f;
    [SerializeField] float DamageTime;
    [SerializeField, Header("ダメージ受けた時")]
    public bool DamageFlag;

    [SerializeField, Header("死んだ時のエフェクト")]
    private GameObject DeathEffect;
    private ParticleSystem DeathParticle;   //ダメージのパーティクル

    [SerializeField, Header("死ぬエフェがでるまでの時間")]
    float DeathEffectTime = 0.5f;

    private int deathState;

    //[SerializeField] Animation anime;
    [SerializeField] private float DeathTime = 0;

    GameObject[] enemyParts;
    int partsCount;

    Ray ray;
    RaycastHit hitRay;
    LineRenderer lineRenderer;
    int enemyNumber = (1 << 13 | 1 << 8);

    private AudioSource audioSource;
    public AudioClip sibouSE;
    private int seCount;//



    // Start is called before the first frame update
    void Start()
    {
        MaxEnemyHP = enemyHP;
        DeathParticle = DeathEffect.GetComponent<ParticleSystem>();
        renderComponent = GetComponent<Renderer>();
        stageMove1 = GameObject.FindGameObjectWithTag("StageMove");
        stageMove1.GetComponent<StageMove1>();

        Target = GameObject.FindGameObjectWithTag("Player");
        rigid = GetComponent<Rigidbody>();
        //color = GetComponent<Renderer>().material.color;

        partsCount = gameObject.transform.childCount;
        enemyParts = new GameObject[partsCount];

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

        audioSource = GetComponent<AudioSource>();//SE
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
            if (DamageFlag)
            {
                StartCoroutine("WaitForIt");
            }

        }
        if (stageMove1.GetComponent<StageMove1>().nowFlag == true)
        {
            MoveFlag = false;
        }

        //常にターゲットにむく
        this.transform.LookAt(new Vector3(Target.transform.position.x, this.transform.position.y, Target.transform.position.z));

        if (!Physics.Raycast(ray, out hitRay, 20, enemyNumber))
        {
            MoveFlag = false;
        }

        if (Physics.Raycast(ray, out hitRay, 20, enemyNumber))
        {
            if (hitRay.collider.gameObject.CompareTag("Player"))
            {
                MoveFlag = true;
                
                //lineRenderer.enabled = true;
                lineRenderer.SetPosition(1, hitRay.point);
            }
            //else if (!hitRay.collider.gameObject.CompareTag("Player"))
            //{
            //    lineRenderer.enabled = false;//(弾が間にいると点滅みたいになる)
               
            //    MoveFlag = false;
            //}
            //else
            //{
            //    lineRenderer.enabled = false;//(弾が間にいると点滅みたいになる)
            //    MoveFlag = false;
               
            //}
        }

        ray.origin = this.transform.position;//自分の位置のレイ

        ray.direction = transform.forward;//自分の向きのレイ

        switch (deathState)
        {
            case 0:
                if (enemyHP <= 0 && !stageMove1.GetComponent<StageMove1>().bossNow)
                {
                    deathState = 1;
                }

                break;

            case 1:
                //アニメーション再生
                //anime.Play();

                //Debug.Log("再生ーーーーーーー");
                DeathTime += Time.deltaTime;
                if (DeathTime > 1)
                {

                    DeathTime = 0;

                    deathState = 2;
                }

                if (DeathTime > 0.1f)
                {
                    if (seCount < 1)
                    {
                        audioSource.PlayOneShot(sibouSE);//SEを鳴らす
                        seCount++;
                    }
                }
                break;

            case 2:
                for (int i = 0; i < partsCount; i++)
                {
                    enemyParts[i] = gameObject.transform.GetChild(i).gameObject;
                    enemyParts[i].SetActive(false);
                }

                // DeathEffectTime -= Time.deltaTime;
                var sum = Instantiate(DeathEffect,
                          this.transform.position,
                          Quaternion.identity);
                deathState = 3;
                

                break;

            case 3:
                if (!stageMove1.GetComponent<StageMove1>().bossNow)
                {
                    TimerScript.enemyCounter += 1;
                }
                Destroy(this.gameObject);
                //gameObject.SetActive(false);//非表示
                break;

        }

        dis = Vector3.Distance(transform.position, Target.transform.position);//二つの距離を計算して一定以下になれば追尾

        if (MoveFlag)
        {
            this.transform.LookAt(new Vector3(Target.transform.position.x, this.transform.position.y, Target.transform.position.z));//ターゲットにむく

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
            MoveFlag = true;
            DamageFlag = true;

        }

        //回復玉に当たったら回復する
        if (other.gameObject.CompareTag("HealBall"))
        {
            enemyHP = enemyHP + 1;
        }

    }

    //ノックバック処理
    //void NockBack(GameObject other, float velocity)
    //{
    //    Vector3 angles = other.transform.localEulerAngles;//当たったオブジェクトの角度
    //    Vector3 directions = Quaternion.Euler(angles) * Vector3.forward;//Wuaternionに変換しつつ正面ベクトル(0, 0 ,1)とかけて

    //    this.transform.position += directions * velocity * Time.deltaTime;
    //}

    private void OnDestroy()
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        DestroyImmediate(renderer.material); //マテリアルのメモリーを消す
    }
}
