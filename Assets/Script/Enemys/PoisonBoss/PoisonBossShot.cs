using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonBossShot : MonoBehaviour
{
    [SerializeField] GameObject BulletP;

    [SerializeField] GameObject Move;

    [SerializeField] int intarval;

    [SerializeField] float shotTime;
    private float ss;
    
    public float min = -50;
    public float max = 50;
    
    public int shotCount;//撃った回数

    private float dis;//プレイヤーとの距離
    private GameObject Target;//追尾する相手

    //レイ関連
    Ray ray;
    RaycastHit hitRay;
    LineRenderer lineRenderer;

    int enemyNumber = (1 << 13 | 1 << 8|1<<9);

    public bool shotFlag;

    void Start()
    {
        ss = 1;
        Target = GameObject.FindGameObjectWithTag("Player");//追尾させたいオブジェクトを書く
        Move.GetComponent<PoisonBoss>();

        ray = new Ray();
        lineRenderer = this.gameObject.GetComponent<LineRenderer>();

        //lineRenderer.SetPosition(0, this.transform.position);
        lineRenderer.enabled = true;
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
        ss += Time.deltaTime;
        dis = Vector3.Distance(transform.position, Target.transform.position);//二つの距離を計算して一定以下になれば追尾

        Random.Range(min, max);

        Ray();
    }

    void Ray()
    {
        lineRenderer.SetPosition(0, this.transform.position);

        if (!Physics.Raycast(ray, out hitRay, 30, enemyNumber))
        {
            lineRenderer.enabled = false;
        }

        //レイの処理
        if (Physics.Raycast(ray, out hitRay, 30, enemyNumber))
        {
            if (hitRay.collider.gameObject.CompareTag("Player"))
            {
                lineRenderer.enabled = true;

                lineRenderer.SetPosition(1, hitRay.point);
                shotFlag = true;
                if (Move.GetComponent<PoisonBoss>().AttackFlag == true)
                {
                    ss += Time.deltaTime;
                    if (ss >= intarval)
                    {
                        poisonShot();
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

            }
            else
            {
                //lineRenderer.enabled = false;//(弾が間にいると点滅みたいになる)
                shotFlag = false;
            }
            if (!hitRay.collider.gameObject.CompareTag("Player"))
            {
                shotFlag = false;
                ss = 0;
                //ラインレンダラーの色
                lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                lineRenderer.startColor = Color.green;//初めの色
                lineRenderer.endColor = Color.green;//終わりの色
            }
        }

        ray.origin = this.transform.position;//自分の位置のレイ

        ray.direction = transform.forward;//自分の向きのレイ

        //Debug.DrawRay(ray.origin, ray.direction * 10, Color.red, 0.1f);
    }




    void poisonShot()
    {
        shotCount = shotCount + 1;
        if (Move.GetComponent<PoisonBoss>().MoveFlag == true)
        {

            GameObject shot = Instantiate(BulletP, transform.position, transform.rotation);
            Rigidbody rigidbody = shot.GetComponent<Rigidbody>();
            rigidbody.AddForce(transform.forward * shotTime);
        }

    }
}
