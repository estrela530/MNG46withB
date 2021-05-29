using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KraberBossPawer : MonoBehaviour
{
    public GameObject powerUpBullet;

    public GameObject Move;

    public int intarval;
    public float min = -50;
    public float max = 50;

    public float shotTime;
    private float ss;
    private float dis;//プレイヤーとの距離
    private GameObject Target;//追尾する相手

    Ray ray;
    RaycastHit hitRay;
    LineRenderer lineRenderer;
    int enemyNumber = (1 << 13 | 1 << 8 | 1 << 9);

    Vector3 direction;
    Vector3 hitPosition;
    
    [SerializeField, Header("索敵の長さ")] float searchRange = 30;
    //private List<GameObject> UpBulletList;

    // Start is called before the first frame update
    void Start()
    {
        ss = 1;
        Target = GameObject.FindGameObjectWithTag("Player");//追尾させたいオブジェクトを書く
        Move.GetComponent<KraberBoss>();

        //描画距離と方向の乗算
        direction = -transform.forward * 20;
        //表示位置は初期化しておこうね!
        hitPosition = this.transform.position + direction;

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
        //UpBulletList = new List<GameObject>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ss += Time.deltaTime;
        dis = Vector3.Distance(transform.position, Target.transform.position);//二つの距離を計算して一定以下になれば追尾

        Random.Range(min, max);

        ray.origin = this.transform.position;//自分の位置のレイ

        ray.direction = transform.forward;//自分の向きのレイ

        

        if (Move.GetComponent<KraberBoss>().pawerFlag == true)
        {

            if (ss >= intarval)
            {
                ss = 0;
                PowerShot();

            }

            Ray();

            //攻撃する前に色を変える
            if (ss >= intarval - 1)
            {
                //ラインレンダラーの色
                lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                lineRenderer.startColor = Color.red;//初めの色
                lineRenderer.endColor = Color.red;//終わりの色
            }
        }
        
    }
    void Ray()
    {
        lineRenderer.SetPosition(0, this.transform.position);

        if (!Physics.Raycast(ray, out hitRay, searchRange, enemyNumber))
        {
            lineRenderer.enabled = false;
        }

        //レイの処理
        if (Physics.Raycast(ray, out hitRay, searchRange, enemyNumber))
        {

            lineRenderer.enabled = true;

            lineRenderer.SetPosition(1, hitRay.point);
        }

        ray.origin = this.transform.position;//自分の位置のレイ

        ray.direction = transform.forward;//自分の向きのレイ

        //Debug.DrawRay(ray.origin, ray.direction * 10, Color.red, 0.1f);
    }

    
    void PowerShot()
    {
        
        if (Move.GetComponent<KraberBoss>().pawerFlag == true)
        {

            Vector3 ff = new Vector3(dis + Random.Range(min, max), 0, dis);
            GameObject shot = Instantiate(powerUpBullet, transform.position, transform.rotation);
            Rigidbody rigidbody = shot.GetComponent<Rigidbody>();
            //rigidbody.AddForce(transform.forward * shotTime);
            rigidbody.AddForce(ff * shotTime);

            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.startColor = Color.green;//初めの色
            lineRenderer.endColor = Color.green;//終わりの色
        }

    }
}
