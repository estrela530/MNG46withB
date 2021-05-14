using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

public class BossArea : MonoBehaviour
{

    //[SerializeField] public float searchAngle;
    [SerializeField] GameObject Boss;
    [SerializeField] GameObject BossHP;
    private GameObject Target;//追尾する相手
    [SerializeField]
    float radius;
    private float dis;//プレイヤーとの距離
    Rigidbody rigid;

    //public Mesh mesh;

    // Start is called before the first frame update
    void Start()
    {
        Boss.GetComponent<BossMove>();
        rigid = GetComponent<Rigidbody>();
        Target = GameObject.FindGameObjectWithTag("Player");
        BossHP.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rigid.angularVelocity = Vector3.zero;
        rigid.velocity = Vector3.zero;
        
    }

    public void OnTriggerStay(Collider other)
    {
        
        if (other.gameObject.CompareTag("Player"))
        {
            Boss.GetComponent<BossMove>().MoveFlag = true;
            BossHP.SetActive(true);
        }
    }
    
}
