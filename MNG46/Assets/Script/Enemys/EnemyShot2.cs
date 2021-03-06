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

    // Start is called before the first frame update
    void Start()
    {
        ss = 1;
        Target = GameObject.Find("Player");//追尾させたいオブジェクトを書く
        Move.GetComponent<EnemyMove>();
        
    }

    // Update is called once per frame
    void Update()
    {
        ss += Time.deltaTime;
        dis = Vector3.Distance(transform.position, Target.transform.position);//二つの距離を計算して一定以下になれば追尾

       Random.Range(min, max);
        

        if (ss >= intarval)
        {
            Shot();
            ss = 0;
        }
    }

    void Shot()
    {
        if (Move.GetComponent<EnemyMove>().MoveFlag == true)
        {

            Vector3 ff = new Vector3(dis+Random.Range(min, max), 0, dis);
            GameObject shot = Instantiate(Bullet, transform.position, transform.rotation);
            Rigidbody rigidbody = shot.GetComponent<Rigidbody>();
            //rigidbody.AddForce(transform.forward * shotTime);
            rigidbody.AddForce(ff * shotTime);
        }

    }
}
