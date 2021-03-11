using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//using UnityStandardAssets.Characters.ThirdPerson;
//[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
[RequireComponent(typeof(Collider))]

public class EnemyMove : MonoBehaviour
{

    // Start is called before the first frame update
    private GameObject Target;//追尾する相手
    private float dis;//プレイヤーとの距離
    public float area;//この数値以下になったら追う

    public int enemyHP;

    private float workeAria1=1;//
    private float workeAria2=1;//

    private float Rspeed;

    private float ww;
    private float ww2;
    public GameObject workObj1;
    public GameObject workObj2;
    int workNumber = 1;

    //Vector3 x;
    //Vector3 y;
    //Vector3 z;
    //Vector3 target;

    public float speed;
    public float speedLoc;
    private GameObject Enemy;

    public bool MoveFlag = false;//追う
    public bool workFlag = true;//徘徊

    void Start()
    {
        Target = GameObject.Find("Player");//追尾させたいオブジェクトを書く
        //target = Target.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //z = (target - transform.position).normalized;
        //x = Vector3.Cross(Vector3.up, z).normalized;
        //y = Vector3.Cross(z, x).normalized;
        
        if (enemyHP < 0)
        {
            gameObject.SetActive(false);//非表示
        }

        dis = Vector3.Distance(transform.position, Target.transform.position);//二つの距離を計算して一定以下になれば追尾
        ww = Vector3.Distance(transform.position, workObj1.transform.position);//二つの距離を計算して一定以下になれば追尾
        ww2 = Vector3.Distance(transform.position, workObj2.transform.position);//二つの距離を計算して一定以下になれば追尾

        if (dis < area)
        {
            MoveFlag = true;
            workFlag = false;
        }
        else if(dis>area)
        {
            MoveFlag = false;
            workFlag = true;
        }
        

        if (MoveFlag)
        {
            //var aim = this.Target.transform.position - this.transform.position;
            //var look = Quaternion.LookRotation(aim);
            //this.transform.localRotation = look;
            //Target.transform.rotation();
            //Vector3 look = Target.transform.position - transform.position;
            //look.Normalize();
            //transform.rotation = Quaternion.RotateTowards(transform.rotation,)
            //Target.transform.position = new Vector3(transform.position.x, 0, transform.position.z);

           this.transform.LookAt(new Vector3(Target.transform.position.x, this.transform.position.y, Target.transform.position.z));//ターゲットにむく

            //this.transform.eulerAngles = new Vector3(0, transform.rotation.y,0);
            transform.position += transform.forward * speedLoc * Time.deltaTime;//前進(スピードが変わる)

            
        }

        //徘徊
        if (workFlag)
        {
            if(ww<workeAria1)
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
    //(仮)指定されたtagに当たると消える
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            enemyHP = enemyHP - 1;
        }
    }
}
