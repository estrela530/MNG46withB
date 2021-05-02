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

    private float ww;
    private float ww2;
    
    [Header("索敵に向かう場所")]
    public GameObject workObj1;
    public GameObject workObj2;

    int workNumber = 1;

    [Header("索敵時のスピード")]
    public float speed;
    [Header("発見時のスピード")]
    public float speedLoc;

    //[Header("この数値まで進む")] public float social;//この数値まで進む
    //private GameObject Enemy;

    [Header("追う時と索敵のフラグ")]
    public bool MoveFlag = false;//追う
    public bool workFlag = true;//徘徊

    void Start()
    {
        //Target = GameObject.Find("Player");//追尾させたいオブジェクトを書く
        Target = GameObject.FindGameObjectWithTag("Player");
        rigid = GetComponent<Rigidbody>();
        //target = Target.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        rigid.angularVelocity = Vector3.zero;
        rigid.velocity = Vector3.zero;

        if (enemyHP <= 0)
        {
            gameObject.SetActive(false);//非表示
            //SceneManager.LoadScene("Result");
            //SceneManager.LoadScene("GameClear");

        }

        dis = Vector3.Distance(transform.position, Target.transform.position);//二つの距離を計算して一定以下になれば追尾

        ww = Vector3.Distance(transform.position, workObj1.transform.position);//二つの距離を計算
        ww2 = Vector3.Distance(transform.position, workObj2.transform.position);//二つの距離を計算

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

        if (MoveFlag)
        {
            this.transform.LookAt(new Vector3(Target.transform.position.x, this.transform.position.y, Target.transform.position.z));//ターゲットにむく
            //if (dis >= social)
            //{
            //    transform.position += transform.forward * speedLoc * Time.deltaTime;//前進(スピードが変わる)
            //}

        }

        //徘徊
        if (workFlag)
        {
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
