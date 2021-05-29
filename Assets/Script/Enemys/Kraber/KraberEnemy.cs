using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;

public class KraberEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject Target;//追尾する相手
    private float dis;//プレイヤーとの距離

    //[SerializeField, Header("弾オブジェクト")] GameObject bullet;

    [SerializeField, Header("体力")] float enemyHP = 5;

    [SerializeField, Header("最大体力")] float MaxEnemyHP;

    [SerializeField, Header("止まってる時間")] float freezTime;
    [SerializeField, Header("いつまで止まるか")] float stopTime;

    Rigidbody rigid;

    // private float workeAria1 = 1;//
    //private float workeAria2 = 1;//
    [SerializeField, Header("最初の位置")] Vector3 startPos;

    private float ww;
    
    [Header("戻る場所")]
    public GameObject workObj1;
   // public GameObject workObj2;

    //int workNumber = 1;

    [Header("索敵時のスピード")]
    public float speed;
    [Header("発見時のスピード")]
    public float speedLoc;

    [Header("この数値まで進む")]
    public float social;//この数値まで進む
    public float keep;//この数値まで離れない
    private GameObject Enemy;

    [Header("追う時と索敵のフラグ")]
    public bool MoveFlag = false;//追う
    public bool workFlag = true;//徘徊
    public bool powerFlag = false;//

    [SerializeField, Header("ダメージ受けた時")]
    bool DamageFlag;

   [SerializeField] float DamageTime;

    [SerializeField, Header("死んだ時のエフェクト")]
    private GameObject DeathEffect;
    private ParticleSystem DeathParticle;   //ダメージのパーティクル

    Ray ray;
    RaycastHit hitRay;
    LineRenderer lineRenderer;
    int enemyNumber = (1 << 13 | 1 << 8);

    Renderer renderComponent;

    //public MeshRenderer meshRenderer;

    [SerializeField] float ColorInterval = 0.1f;
    //[SerializeField] float Interval = 0;

    GameObject stageMove1;


    void Start()
    {
        DeathParticle = DeathEffect.GetComponent<ParticleSystem>();
        MaxEnemyHP = enemyHP;
        stageMove1 = GameObject.FindGameObjectWithTag("StageMove");
        stageMove1.GetComponent<StageMove1>();
        ray = new Ray();
        lineRenderer = this.gameObject.GetComponent<LineRenderer>();
        //Target = GameObject.Find("Player");//追尾させたいオブジェクトを書く
        Target = GameObject.FindGameObjectWithTag("Player");
        rigid = GetComponent<Rigidbody>();
        
        DamageFlag = false;

        renderComponent = GetComponent<Renderer>();

        //lineRenderer.SetPosition(0, this.transform.position);
        lineRenderer.enabled = false;
        lineRenderer.startWidth = 0.5f;
        lineRenderer.endWidth = 0.5f;
        ray.origin = this.transform.position;//自分の位置のレイ

        //ラインレンダラーの色
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.green;//初めの色
        lineRenderer.endColor = Color.green;//終わりの色

        ////////////////
        ray.direction = transform.forward;

        startPos = GetComponent<Transform>().position;//最初のポジション

       
        //bullet.GetComponent<KraberBallet>();
        //target = Target.transform.position;

        //StartCoroutine("Blink");

        //renderComponent = this.gameObject.transform.GetChild(0).GetComponent<Renderer>();
    }

    //中断できる処理のまとまり
    IEnumerator Blink()
    {
        while(true)
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

        //上に行かない処理
        if(this.transform.position.y < startPos.y)
        {
            Vector3 resetPos = new Vector3(transform.position.x, startPos.y, transform.position.z);

            this.transform.position = resetPos;
        }

        this.transform.LookAt(new Vector3(Target.transform.position.x, this.transform.position.y, Target.transform.position.z));//ターゲットにむく

        //最大体力以上にはならない。
        if (enemyHP >= MaxEnemyHP)
        {
            enemyHP = MaxEnemyHP;
        }

        if (stageMove1.GetComponent<StageMove1>().nowFlag == true)
        {
            MoveFlag = false;
            powerFlag = false;
        }

        //meshRenderer.transform.GetChild(0).GetComponent<MeshRenderer>();
        if (enemyHP <= 0 && !stageMove1.GetComponent<StageMove1>().bossNow)
        {
            Destroy(this.gameObject);
            //gameObject.SetActive(false);//非表示
            TimerScript.enemyCounter += 1;
            renderComponent.enabled = false;
            var sum = Instantiate(DeathEffect,
                          this.transform.position,
                          Quaternion.identity);
        }

        dis = Vector3.Distance(transform.position, Target.transform.position);//二つの距離を計算して一定以下になれば追尾

        ww = Vector3.Distance(transform.position, workObj1.transform.position);//二つの距離を計算

        lineRenderer.SetPosition(0, this.transform.position);
        if (!Physics.Raycast(ray, out hitRay, 40,enemyNumber))
        {
            MoveFlag = false;
            lineRenderer.enabled = false;
            workFlag = true;
        }
        if (Physics.Raycast(ray, out hitRay, 40,enemyNumber))
        {
            if (!hitRay.collider.gameObject.CompareTag("Player"))
            {
                lineRenderer.enabled = false;//(弾が間にいると点滅みたいになる)
            }
            if(hitRay.collider.gameObject.CompareTag("Player"))
            {
                lineRenderer.enabled = true;
                lineRenderer.SetPosition(1, hitRay.point);

                MoveFlag = true;

                
            }
            else
            {
                lineRenderer.enabled = false;//(弾が間にいると点滅みたいになる)
                MoveFlag = false;
                workFlag = true;
            }
        }

        ray.origin = this.transform.position;//自分の位置のレイ
        
        ray.direction = transform.forward;//自分の向きのレイ

        //Debug.DrawRay(ray.origin, ray.direction * 10, Color.red, 0.1f);
        
        if (MoveFlag)
        {
            //Debug.Log("ugoku---------------");
            this.transform.LookAt(new Vector3(Target.transform.position.x, this.transform.position.y, Target.transform.position.z));//ターゲットにむく
            if (dis <= keep)
            {
                
                transform.position -= transform.forward * speedLoc * Time.deltaTime;//後進(スピードが変わる)
            }
            if (dis <= social)
            {
                transform.position += transform.forward * speedLoc * Time.deltaTime;//前進(スピードが変わる)
            }

        }

        ////徘徊
        //if (workFlag)
        //{
        //    powerFlag = false;
         
        //    this.transform.LookAt(this.workObj1.transform);//徘徊1の位置に向く
        //    transform.position += transform.forward * speed * Time.deltaTime;
            
        //}

        if (powerFlag)
        {

            this.transform.LookAt(new Vector3(Target.transform.position.x, this.transform.position.y, Target.transform.position.z));//ターゲットにむく
            freezTime += Time.deltaTime;
            if(freezTime>=stopTime)
            {
                MoveFlag = true;
                freezTime = 0;
            }

        }
        
        if(enemyHP>0)
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
        if (other.gameObject.CompareTag("Wall"))
        {
            MoveFlag = false;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Wall")|| 
            other.gameObject.CompareTag("Enemy"))
        {
            //MoveFlag = false;
            powerFlag = true;
        }

        if (other.gameObject.CompareTag("Wall"))
        {
            workFlag = false;
        }
    }

    private void OnDestroy()
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        DestroyImmediate(renderer.material); //マテリアルのメモリーを消す
    }


}
