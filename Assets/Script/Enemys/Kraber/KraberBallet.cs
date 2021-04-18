using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KraberBallet : MonoBehaviour
{
    private GameObject Bullet;
    [SerializeField, Header("弾の速度")] public float bullteSpeed;
    [SerializeField, Header("消えるまでの時間")] float desthTime;//消えるまでの時間

    [SerializeField, Header("クレバーエネミーをいれる")] GameObject kraberEnmy;


    // Start is called before the first frame update
    void Start()
    {
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += transform.forward * bullteSpeed * Time.deltaTime;
        
        Destroy(this.gameObject, desthTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall")
            || other.gameObject.CompareTag("Player"))
        {
            Destroy(this.gameObject);
        }
    }

}
