using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToriteiBullet : MonoBehaviour
{
    private GameObject Bullet;
    [SerializeField, Header("弾の速度")] public float bullteSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += transform.forward * bullteSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall")
            || other.gameObject.CompareTag("Player")
            || other.gameObject.CompareTag("Fragment"))
        {
            Destroy(this.gameObject);
        }
    }
}
