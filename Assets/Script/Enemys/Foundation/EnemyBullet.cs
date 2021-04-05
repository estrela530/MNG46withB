using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
   
    private GameObject Bullet;
    public float bullteSpeed;
    public float desthTime;//消えるまでの時間
    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * bullteSpeed * Time.deltaTime;
        Destroy(this.gameObject,desthTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            Destroy(this.gameObject);
        }
    }

}
