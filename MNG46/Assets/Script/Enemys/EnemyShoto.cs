using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoto : MonoBehaviour
{
    
    public GameObject Bullet;

    GameObject gg;

    EnemyMove enemyMove;

    private int intarval;

    public float shotTime;
    private float ss;

   // public GameObject Smp;

    // Start is called before the first frame update
    void Start()
    {
        ss = 1;
        //gg = GameObject.Find("EnemyMove");
        enemyMove = GetComponent<EnemyMove>();
    }

    // Update is called once per frame
    void Update()
    {
        ss += Time.deltaTime;

        if (ss >= 2)
        {
            Shot();
            ss = 0;
        }

        //Debug.Log("gggggggggggg" + enemyMove.MoveFlag);
    }

    void Shot()
    {
            GameObject shot = Instantiate(Bullet, transform.position, transform.rotation);
            Rigidbody rigidbody = shot.GetComponent<Rigidbody>();
            rigidbody.AddForce(transform.forward * shotTime);

    }
}
