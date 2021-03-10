using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
   
    private GameObject Bullet;
    public float bullteSpeed;
    public float desth;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * bullteSpeed * Time.deltaTime;
       
    }

}
