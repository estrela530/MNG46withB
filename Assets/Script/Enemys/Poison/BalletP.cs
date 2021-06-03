using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalletP : MonoBehaviour
{
    [SerializeField] GameObject poisonBullet;
    [SerializeField,Header("弾の速度")] float bullteSpeed;
    [SerializeField, Header("消えるまでの時間")] float desthTime;//消えるまでの時間
    // Start is called before the first frame update
    void Start()
    {

    }


    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += transform.forward * bullteSpeed * Time.deltaTime;
        //Destroy(this.gameObject, desthTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")
            || other.gameObject.CompareTag("Fragment")
            || other.gameObject.CompareTag("HealBall"))
        {
            Destroy(this.gameObject);
        }

        if (other.gameObject.CompareTag("Wall"))
        {
            Destroy(this.gameObject);
            Instantiate(poisonBullet, this.transform.position, Quaternion.identity);
        }
    }

}
