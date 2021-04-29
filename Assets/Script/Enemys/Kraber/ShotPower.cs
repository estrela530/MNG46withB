using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotPower : MonoBehaviour
{
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

    // Start is called before the first frame update
    void Start()
    {
        ss = 1;
        Target = GameObject.FindGameObjectWithTag("Player");//追尾させたいオブジェクトを書く
        Move.GetComponent<KraberEnemy>();
    }

    // Update is called once per frame
    void Update()
    {
        ss += Time.deltaTime;
        dis = Vector3.Distance(transform.position, Target.transform.position);//二つの距離を計算して一定以下になれば追尾

        Random.Range(min, max);


        if (Move.GetComponent<KraberEnemy>().MoveFlag == false)
        {
            if (ss >= intarval)
            {
                Power();
                ss = 0;
            }
        }

        if (Move.GetComponent<KraberEnemy>().MoveFlag == true)
        {
            if (ss >= intarval)
            {
                Power();
                ss = 0;
            }
        }


    }
    
    void Power()
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
