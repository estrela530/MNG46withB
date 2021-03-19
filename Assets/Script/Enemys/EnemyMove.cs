using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;

//using UnityStandardAssets.Characters.ThirdPerson;
//[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
[RequireComponent(typeof(Collider))]

public class EnemyMove : MonoBehaviour
{
    private SpherecastCommand searchArea;//サーチ範囲

   
    [SerializeField] public float searchAngle = 100f;

    // Start is called before the first frame update
    private GameObject Target;//追尾する相手
    private float dis;//プレイヤーとの距離
   // public float area;//この数値以下になったら追う
    public float social;//この数値まで進む

    [SerializeField] float enemyHP = 5;
    
        

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

        

        if (enemyHP <= 0)
        {
            gameObject.SetActive(false);//非表示
            //SceneManager.LoadScene("Result");
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
            if(dis>=social)
            {
               transform.position += transform.forward * speedLoc * Time.deltaTime;//前進(スピードが変わる)
            }

            
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

    private void OnTriggerStay(Collider other)
    {
        //プレイヤーの方向
        var playerDire = other.transform.position - this.transform.position;

        //自分の前方からプレイヤーの方向
        var angle = Vector3.Angle(transform.forward, playerDire);

        if (other.gameObject.CompareTag("Player"))
        {
            //サーチする角度の範囲内だったら発見
            if(angle<=searchAngle)
            {
                MoveFlag = true;
                workFlag = false;
                Debug.Log("主人公発見: " + angle);
            }
            //サーチする角度の範囲内だったら発見
            else
            {
                Debug.Log("範囲外: " + angle);
            }

        }

        if (!other.gameObject.CompareTag("Player"))
        {
            MoveFlag = false;
            workFlag = true;
            Debug.Log("範囲外: " + angle);
        }

    }

#if UNITY_EDITOR
    //サーチ範囲を表示
    private void OnDrawGizmos()
    {
        Handles.color = Color.red;
        //Handles.DrawSolidDisc(transform.position, Vector3.up, 3.0f);
        //Handles.DrawSolidArc(transform.position, Vector3.up, Quaternion.Euler(0f, -searchAngle, 0f) * transform.forward, searchAngle * 5f, HandleUtility.GetHandleSize(Vector3.zero));
        Handles.DrawSolidArc(
            this.transform.position, //中心点
            Vector3.up, //表示する表面の方向
            Quaternion.Euler(0f, -searchAngle, 0f)*this.transform.forward, //扇の表示を開始する方向
            searchAngle, //扇の角度
            searchArea.radius//半径
            );

    }
#endif

}
