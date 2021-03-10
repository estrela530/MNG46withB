using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//using UnityStandardAssets.Characters.ThirdPerson;
//[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]

public class EnemyMove : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject Target;//追尾する相手
    private float dis;//プレイヤーとの距離
    public float area;//この数値以下になったら追う


    private float workeAria1=1;//
    private float workeAria2=1;//

    private float ww;
    private float ww2;
    public GameObject workObj1;
    public GameObject workObj2;
    int workNumber = 1;


    public float speed;
    private GameObject Enemy;

    public bool MoveFlag = false;//追う
    public bool workFlag = true;//徘徊

    void Start()
    {
        Target = GameObject.Find("Player");//追尾させたいオブジェクトを書く
      
    }

    // Update is called once per frame
    void Update()
    {


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

            this.transform.LookAt(this.Target.transform);//ターゲットにむく

            transform.position += transform.forward * speed * Time.deltaTime;//前進

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
}
