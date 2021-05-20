using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShot2 : MonoBehaviour
{
    public GameObject Bullet;

    public GameObject Move;

    public int intarval;
    public float min = -100;
    public float max = 100;

    public float shotTime;
    private float ss;

    private float dis;//プレイヤーとの距離
    private GameObject Target;//追尾する相手
    public bool shotFlag;

    [SerializeField, Header("索敵の長さ")] float searchRange = 10;

    //レイ関連
    Ray ray;
    RaycastHit hitRay;
    LineRenderer lineRenderer;
    int enemyNumber = (1 << 13 | 1 << 8);

    // Start is called before the first frame update
    void Start()
    {
        ss = 1;
        Target = GameObject.FindGameObjectWithTag("Player");//追尾させたいオブジェクトを書く
        Move.GetComponent<EnemyMove>();

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

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        dis = Vector3.Distance(transform.position, Target.transform.position);//二つの距離を計算して一定以下になれば追尾

       Random.Range(min, max);


        Ray();

        
       
    }

    void Ray()
    {
        lineRenderer.SetPosition(0, this.transform.position);

        //レイの処理
        if (Physics.Raycast(ray, out hitRay, searchRange, enemyNumber))
        {
            if (hitRay.collider.gameObject.CompareTag("Player"))
            {
                lineRenderer.enabled = true;

                Move.GetComponent<EnemyMove>().MoveFlag = true;
                lineRenderer.SetPosition(1, hitRay.point);
                shotFlag = true;

                ss += Time.deltaTime;
                if (ss >= intarval)
                {
                    Shot();
                    ss = 0;

                }
                //攻撃する前に色を変える
                if (ss >= intarval - 1)
                {
                    //ラインレンダラーの色
                    lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                    lineRenderer.startColor = Color.red;//初めの色
                    lineRenderer.endColor = Color.red;//終わりの色

                }
            }
            else
            {
                lineRenderer.enabled = false;//(弾が間にいると点滅みたいになる)
                Move.GetComponent<EnemyMove>().MoveFlag = false;
                Move.GetComponent<EnemyMove>().workFlag = true;
                shotFlag = false;
            }
            if (!hitRay.collider.gameObject.CompareTag("Player"))
            {
                shotFlag = false;
                //ラインレンダラーの色
                lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                lineRenderer.startColor = Color.green;//初めの色
                lineRenderer.endColor = Color.green;//終わりの色
            }
        }

        ray.origin = this.transform.position;//自分の位置のレイ

        ray.direction = transform.forward;//自分の向きのレイ

        Debug.DrawRay(ray.origin, ray.direction * 10, Color.red, 0.1f);
    }

    void Shot()
    {
        if (shotFlag)
        {
            Vector3 ff = new Vector3(dis + Random.Range(min, max), 0, dis);
            GameObject shot = Instantiate(Bullet, transform.position, transform.rotation);
            Rigidbody rigidbody = shot.GetComponent<Rigidbody>();
            rigidbody.AddForce(ff * shotTime);

            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.startColor = Color.green;//初めの色
            lineRenderer.endColor = Color.green;//終わりの色
        }

    }
}
