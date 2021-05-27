using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotK : MonoBehaviour
{
    public GameObject Bullet;
    public GameObject powerUpBullet;

    public GameObject Move;

    public int intarval;
    public float min = -100;
    public float max = 100;

    public float shotTime;
    private float ss;
    [SerializeField] int ShotCount = 3;
    private float dis;//プレイヤーとの距離
    private GameObject Target;//追尾する相手
    //private List<GameObject> UpBulletList;

    // Start is called before the first frame update
    void Start()
    {
        ss = 1;
        Target = GameObject.FindGameObjectWithTag("Player");//追尾させたいオブジェクトを書く
        Move.GetComponent<KraberEnemy>();
        //UpBulletList = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        ss += Time.deltaTime;
        dis = Vector3.Distance(transform.position, Target.transform.position);//二つの距離を計算して一定以下になれば追尾

        Random.Range(min, max);
        

        if(Move.GetComponent<KraberEnemy>().MoveFlag == true)
        {
            if (ss >= intarval)
            {
                Shot();
                ss = 0;
            }
        }
        if (Move.GetComponent<KraberEnemy>().MoveFlag == false)
        {
            if (ss >= intarval)
            {
                PowerShot();
                ss = 0;
            }
        }

        if (Move.GetComponent<KraberEnemy>().MoveFlag == true)
        {
            if (ss >= intarval)
            {
                PowerShot();
                ss = 0;
            }
        }

    }

    void Shot()
    {
        if (Move.GetComponent<KraberEnemy>().MoveFlag == true)
        {
            Vector3 ff = new Vector3(dis + Random.Range(min, max), 0, dis);
            GameObject shot = Instantiate(Bullet, transform.position, transform.rotation);
            Rigidbody rigidbody = shot.GetComponent<Rigidbody>();
            //rigidbody.AddForce(transform.forward * shotTime);
            rigidbody.AddForce(ff * shotTime);
        }

        //if (Move.GetComponent<KraberEnemy>().MoveFlag == true)
        //{
        //    Vector3 ff = new Vector3(dis, 0, dis);
        //    if (1 < ShotCount)
        //    {
        //        for (int i = 0; i < ShotCount; i++)
        //        {

        //            //Rigidbody rigidbody = ShotUp.GetComponent<Rigidbody>();
        //            //rigidbody.AddForce(ff * shotTime);
        //        }
        //    }


        //    //rigidbody.AddForce(transform.forward * shotTime);
        //}
    }



    void PowerShot()
    {

        if (Move.GetComponent<KraberEnemy>().powerFlag == true)
        {
            Vector3 ff = new Vector3(dis + Random.Range(min, max), 0, dis);
            GameObject shot = Instantiate(powerUpBullet, transform.position, transform.rotation);
            Rigidbody rigidbody = shot.GetComponent<Rigidbody>();
            //rigidbody.AddForce(transform.forward * shotTime);
            rigidbody.AddForce(ff * shotTime);

        }

    }
}
