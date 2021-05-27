using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonShot : MonoBehaviour
{

    [SerializeField] GameObject BulletP;

    [SerializeField] GameObject Move;

    [SerializeField] int intarval;

    [SerializeField] float shotTime;
    private float ss;


    // Start is called before the first frame update
    void Start()
    {
        ss = 1;
        //gg = GameObject.Find("EnemyMove");
        Move.GetComponent<PoisonEnemy>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ss += Time.deltaTime;

        if (ss >= intarval)
        {
            poisonShot();
            ss = 0;
        }

        //Debug.Log("gggggggggggg" + enemyMove.MoveFlag);
    }

    void poisonShot()
    {
        if (Move.GetComponent<PoisonEnemy>().MoveFlag == true)
        {

            GameObject shot = Instantiate(BulletP, transform.position, transform.rotation);
            Rigidbody rigidbody = shot.GetComponent<Rigidbody>();
            rigidbody.AddForce(transform.forward * shotTime);
        }

    }
}

