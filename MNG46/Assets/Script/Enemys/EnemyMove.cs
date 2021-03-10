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
    

    public float speed;
    private GameObject Enemy;

    public bool MoveFlag;

    void Start()
    {
        Target = GameObject.Find("Player");//追尾させたいオブジェクトを書く
      
    }

    // Update is called once per frame
    void Update()
    {


        dis = Vector3.Distance(transform.position, Target.transform.position);//二つの距離を計算して一定以下になれば追尾
        

        if (dis < area)
        {
            MoveFlag = true;
        }
        else if(dis>area)
        {
            MoveFlag = false;
        }
        

        if (MoveFlag)
        {
            //var aim = this.Target.transform.position - this.transform.position;
            //var look = Quaternion.LookRotation(aim);
            //this.transform.localRotation = look;

            this.transform.LookAt(this.Target.transform);
            
            transform.position += transform.forward * speed * Time.deltaTime;

        }


    }
}
