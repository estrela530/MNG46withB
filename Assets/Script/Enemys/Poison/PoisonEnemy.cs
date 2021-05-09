using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;

public class PoisonEnemy : MonoBehaviour
{

    // Start is called before the first frame update
    private GameObject Target;//追尾する相手
    private float dis;//プレイヤーとの距離
   // public float area;//この数値以下になったら追う


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

    void Start()
    {
        //Target = GameObject.Find("Player");//追尾させたいオブジェクトを書く
        Target = GameObject.FindGameObjectWithTag("Player");
        rigid = GetComponent<Rigidbody>();

        ray = new Ray();
        lineRenderer = this.gameObject.GetComponent<LineRenderer>();

        //lineRenderer.SetPosition(0, this.transform.position);
        lineRenderer.enabled = false;
        ray.origin = this.transform.position;//自分の位置のレイ

        //ラインレンダラーの色
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.red;//初めの色
        lineRenderer.endColor = Color.red;//終わりの色
        //変えるかも?
        ray.direction = transform.forward;
    }

    // Update is called once per frame
    void Update()
    {

        rigid.angularVelocity = Vector3.zero;
        rigid.velocity = Vector3.zero;

        //常にターゲットにむく
        this.transform.LookAt(new Vector3(Target.transform.position.x, this.transform.position.y, Target.transform.position.z));

        if (enemyHP <= 0)
        {
            gameObject.SetActive(false);//非表示

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
        }

    }
    private void OnDestroy()
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        DestroyImmediate(renderer.material); //マテリアルのメモリーを消す
    }
}
