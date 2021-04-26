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
    [SerializeField, Header("止まってる時間")] float freezTime;
    [SerializeField, Header("いつまで止まるか")] float stopTime;
    [SerializeField, Header("パワーアップした時の速度")] float upSpeed;

    Rigidbody rigid;
    [SerializeField]

   // private float workeAria1 = 1;//
    //private float workeAria2 = 1;//

    private float Rspeed;

    private float ww;
    private float ww2;

    Color color;

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

    float DamageTime;

    public MeshRenderer meshRenderer;

    [SerializeField] float ColorInterval = 0.1f;
    [SerializeField] float Interval = 0;

    void Start()
    {
        //Target = GameObject.Find("Player");//追尾させたいオブジェクトを書く
        Target = GameObject.FindGameObjectWithTag("Player");
        rigid = GetComponent<Rigidbody>();
        color = GetComponent<Renderer>().material.color;
        DamageFlag = false;
        //bullet.GetComponent<KraberBallet>();
        //target = Target.transform.position;

        //StartCoroutine("Blink");
    }

    //中断できる処理のまとまり
    IEnumerator Blink()
    {
        while(true)
        {
            var renderComponent = GetComponent<Renderer>();
            renderComponent.enabled = !renderComponent.enabled;
            //何フレームとめる
            yield return new WaitForSeconds(ColorInterval);
        }
    }

    // Update is called once per frame
    void Update()
    {
        rigid.angularVelocity = Vector3.zero;
        rigid.velocity = Vector3.zero;
        meshRenderer.transform.GetChild(0).GetComponent<MeshRenderer>();
        if (enemyHP <= 0)
        {
            gameObject.SetActive(false);//非表示
            //SceneManager.LoadScene("Result");
            //SceneManager.LoadScene("GameClear");
        }

        dis = Vector3.Distance(transform.position, Target.transform.position);//二つの距離を計算して一定以下になれば追尾

        ww = Vector3.Distance(transform.position, workObj1.transform.position);//二つの距離を計算
       
        
        if (MoveFlag)
        {
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

        //徘徊
        if (workFlag)
        {
            powerFlag = false;
         
            this.transform.LookAt(this.workObj1.transform);//徘徊1の位置に向く
            transform.position += transform.forward * speed * Time.deltaTime;
            
        }

        if (powerFlag)
        {
            //bullet.GetComponent<KraberBallet>().bullteSpeed = upSpeed;

            this.transform.LookAt(new Vector3(Target.transform.position.x, this.transform.position.y, Target.transform.position.z));//ターゲットにむく
            freezTime += Time.deltaTime;
            if(freezTime>=stopTime)
            {
                MoveFlag = true;
                freezTime = 0;
            }

        }
        
        //ダメージ
        if(DamageFlag)
        {
            DamageTime += Time.deltaTime;
            
            if (DamageTime>1)
            {
                //meshRenderer.material.color = Color.black;
                DamageTime = 0;
                //DamageFlag = false;
                StartCoroutine("Blink");
                DamageFlag = false;
            }
        }
        if(!DamageFlag)
        {
            Interval += Time.deltaTime;
            if(Interval>1)
            {
                StopCoroutine("Blink");
                Interval = 0;
            }
            
        }
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
        if (other.gameObject.CompareTag("Wall"))
        {
            MoveFlag = false;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            MoveFlag = false;
            powerFlag = true;
        }

        if (other.gameObject.CompareTag("Work"))
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
