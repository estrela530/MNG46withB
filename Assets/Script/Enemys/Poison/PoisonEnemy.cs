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

    [SerializeField, Header("体力")] float enemyHP = 5;

    Rigidbody rigid;

    private float workeAria1 = 1;//
    private float workeAria2 = 1;//

    private float Rspeed;
    
    
    [Header("索敵時のスピード")]
    public float speed;
    [Header("発見時のスピード")]
    public float speedLoc;

    //[Header("この数値まで進む")] public float social;//この数値まで進む
    //private GameObject Enemy;

    [Header("追う時と索敵のフラグ")]
    public bool MoveFlag = false;//追う
    //public bool workFlag = true;//徘徊

    //レイ関連
    Ray ray;
    RaycastHit hitRay;
    LineRenderer lineRenderer;

    Renderer renderComponent;
    [SerializeField] float ColorInterval = 0.1f;
    [SerializeField] float DamageTime;
    [SerializeField, Header("ダメージ受けた時")]
    bool DamageFlag = false;

    [SerializeField, Header("死んだ時のエフェクト")]
    private GameObject DeathEffect;
    private ParticleSystem DeathParticle;   //ダメージのパーティクル

    GameObject stageMove1;

    
    void Start()
    {
        DeathParticle = DeathEffect.GetComponent<ParticleSystem>();
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
        //常にターゲットにむく
        this.transform.LookAt(new Vector3(Target.transform.position.x, this.transform.position.y, Target.transform.position.z));

        if (stageMove1.GetComponent<StageMove1>().nowFlag == true)
        {
            MoveFlag = false;
        }

        if (enemyHP <= 0 && !stageMove1.GetComponent<StageMove1>().bossNow)
        {
            gameObject.SetActive(false);//非表示
            TimerScript.enemyCounter += 1;
            var sum = Instantiate(DeathEffect,
                          this.transform.position,
                          Quaternion.identity);

        }

        dis = Vector3.Distance(transform.position, Target.transform.position);//二つの距離を計算して一定以下になれば追尾
        
        //if (dis < area)
        //{
        //    MoveFlag = true;
        //    workFlag = false;
        //}
        //else if(dis>area)
        //{
        //    MoveFlag = false;
        //    workFlag = true;
        //}
        lineRenderer.SetPosition(0, this.transform.position);

        //レイの処理
        if (Physics.Raycast(ray, out hitRay, 20))
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

        Debug.DrawRay(ray.origin, ray.direction * 10, Color.red, 0.1f);

        //if (MoveFlag)
        //{
        //    this.transform.LookAt(new Vector3(Target.transform.position.x, this.transform.position.y, Target.transform.position.z));//ターゲットにむく
        //    //if (dis >= social)
        //    //{
        //    //    transform.position += transform.forward * speedLoc * Time.deltaTime;//前進(スピードが変わる)
        //    //}

        //}

        //徘徊
        //if (workFlag)
        //{
        //    if (ww < workeAria1)
        //    {
        //        workNumber = 2;
        //    }
        //    if (ww2 < workeAria2)
        //    {
        //        workNumber = 1;
        //    }

        //    switch (workNumber)
        //    {

        //        case 1:

        //            this.transform.LookAt(this.workObj1.transform);//徘徊1の位置に向く
        //            transform.position += transform.forward * speed * Time.deltaTime;

        //            break;

        //        case 2:

        //            this.transform.LookAt(this.workObj2.transform);//徘徊2の位置に向く
        //            transform.position += transform.forward * speed * Time.deltaTime;

        //            break;
        //    }


        //}

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

    }
    private void OnDestroy()
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        DestroyImmediate(renderer.material); //マテリアルのメモリーを消す
    }
}
