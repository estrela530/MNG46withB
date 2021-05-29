using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctaneWand : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject Target;//追尾する相手
    private float dis;//プレイヤーとの距離
                      // public float area;//この数値以下になったら追う


    [Header("体力")] public float enemyHP = 5;
    [SerializeField, Header("最大体力")] float MaxEnemyHP;

    Rigidbody rigid;

    private float workeAria1 = 1;//
    private float workeAria2 = 1;//

    private float Rspeed;

    private float ww;
    private float ww2;
    Player player;
    Color color;
    //Player player;

    [Header("索敵に向かう場所")]
    public GameObject workObj1;
    public GameObject workObj2;

    int workNumber = 1;

    [Header("索敵時のスピード")]
    public float speed;
    [Header("発見時のスピード")]
    public float speedLoc;

    [Header("この数値まで進む")] public float social;//この数値まで進む
    private GameObject Enemy;

    [Header("追う時と索敵のフラグ")]
    public bool MoveFlag = false;//追う
    public bool workFlag = true;//徘徊
    //public bool lookFlag = false;

    [SerializeField, Header("何秒止まるか")]
    public float freezeTime;
    public float lookTime;

    Ray ray;
    RaycastHit hitRay;
    LineRenderer lineRenderer;
    int enemyNumber = (1 << 13 | 1 << 8);

    Vector3 playerPos;
    Vector3 EnemyPos;
    Vector3 velocity = Vector3.zero;

    int moveState;
    
    GameObject stageMove1;

    Renderer renderComponent;
    [SerializeField] float ColorInterval = 0.1f;
    [SerializeField] float DamageTime;
    [SerializeField, Header("ダメージ受けた時")]
    bool DamageFlag;

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
    private GameObject[] child;          //子どもオブジェクト

    // Start is called before the first frame update
    void Start()
    {
        MaxEnemyHP = enemyHP;

        DeathParticle = deathEffect.GetComponent<ParticleSystem>();

        renderComponent = GetComponent<Renderer>();
        stageMove1 = GameObject.FindGameObjectWithTag("StageMove");
        stageMove1.GetComponent<StageMove1>();

        moveState = 0;
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
    }
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
            workFlag = false;
        }

        switch (moveState)
        {
            case 0:

                Work();

                if (MoveFlag)
                {
                    moveState = 1;
                }
                break;

            case 1://見てる時
                lookTime += Time.deltaTime;

                if (lookTime <= freezeTime)
                {
                    this.transform.LookAt(new Vector3(Target.transform.position.x, this.transform.position.y, Target.transform.position.z));//ターゲットにむく

                    //レイの処理
                    if (Physics.Raycast(ray, out hitRay, 15,enemyNumber))
                    {
                        if (hitRay.collider.gameObject.CompareTag("Player"))
                        {
                            lineRenderer.enabled = true;

                            lineRenderer.SetPosition(1, hitRay.point);
                            //hitRay.point;
                        }

                    }
                }

                if (lookTime >= freezeTime)
                {
                    moveState = 2;
                }

                if (lookTime >= freezeTime - 0.5f)
                {
                    lineRenderer.startColor = Color.red;//初めの色
                    lineRenderer.endColor = Color.red;//終わりの色
                }
                break;


            case 2://位置

                playerPos = Target.transform.position;
                //this.transform.LookAt(new Vector3(playerPos.x, this.transform.position.y, playerPos.z));//ターゲットにむく
                moveState = 3;
                break;


            case 3:
                lineRenderer.startColor = Color.green;//初めの色
                lineRenderer.endColor = Color.green;//終わりの色

                transform.position = Vector3.MoveTowards(transform.position, new Vector3(playerPos.x, this.transform.position.y, playerPos.z), speedLoc * Time.deltaTime);

                lineRenderer.enabled = false;//(弾が間にいると点滅みたいになる)
                lookTime = 0;
                if ( playerPos.z == transform.position.z)
                {
                    moveState = 0;
                }

                break;

        }
        //if (Target != null)
        //{
        //    transform.localScale = new Vector3(2, 2, 2);
        //}

       
        dis = Vector3.Distance(transform.position, Target.transform.position);//二つの距離を計算して一定以下になれば追尾

        ww = Vector3.Distance(transform.position, workObj1.transform.position);//二つの距離を計算
        ww2 = Vector3.Distance(transform.position, workObj2.transform.position);//二つの距離を計算

        lineRenderer.SetPosition(0, this.transform.position);



        ray.origin = this.transform.position;//自分の位置のレイ

        ray.direction = transform.forward;//自分の向きのレイ

        Debug.DrawRay(ray.origin, ray.direction * 10, Color.red, 0.1f);

        //if (MoveFlag)
        //{
        //    lookTime += Time.deltaTime;

        //    if(lookTime <=freezeTime)
        //    {
        //      this.transform.LookAt(new Vector3(Target.transform.position.x, this.transform.position.y, Target.transform.position.z));//ターゲットにむく

        //        //レイの処理
        //        if (Physics.Raycast(ray, out hitRay, 10))
        //        {
        //            if (hitRay.collider.gameObject.CompareTag("Player"))
        //            {
        //                lineRenderer.enabled = true;

        //                lineRenderer.SetPosition(1, hitRay.point);
        //                //hitRay.point;
        //            }

        //        }
        //    }

        //    if (lookTime >= freezeTime)
        //    {
        //        transform.position += transform.forward * speedLoc * Time.deltaTime;//前進(スピードが変わる)

        //        lineRenderer.enabled = false;//(弾が間にいると点滅みたいになる)

        //        //lookTime = 0;
        //    }

        //}
    }

    void Work()
    {
        //徘徊
        if (workFlag)
        {
            lookTime = 0;
            if (ww < workeAria1)
            {
                workNumber = 2;
            }
            if (ww2 < workeAria2)
            {
                workNumber = 1;
            }

            switch (workNumber)
            {

                case 1:

                    this.transform.LookAt(this.workObj1.transform);//徘徊1の位置に向く
                    transform.position += transform.forward * speed * Time.deltaTime;

                    break;

                case 2:

                    this.transform.LookAt(this.workObj2.transform);//徘徊2の位置に向く
                    transform.position += transform.forward * speed * Time.deltaTime;

                    break;
            }


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
            MoveFlag = true;
            workFlag = false;
            DamageFlag = true;
            //color.g = 160;
        }

        //回復玉に当たったら回復する
        if (other.gameObject.CompareTag("HealBall"))
        {
            enemyHP = enemyHP + 1;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") ||
            other.gameObject.CompareTag("Wall"))
        {
            workFlag = true;
            MoveFlag = false;
            moveState = 0;
        }

    }

    private void OnDestroy()
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        DestroyImmediate(renderer.material); //マテリアルのメモリーを消す
    }
}
