using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyArea : MonoBehaviour
{
    //[SerializeField] public float searchAngle;
    [SerializeField] GameObject Key;
    //[SerializeField] GameObject BossHP;
    private GameObject Target;//追尾する相手
    [SerializeField]
    float radius;
    private float dis;//プレイヤーとの距離
    Rigidbody rigid;

    //public Mesh mesh;

    // Start is called before the first frame update
    void Start()
    {
        Key.GetComponent<BossMove>();
        rigid = GetComponent<Rigidbody>();
        Target = GameObject.FindGameObjectWithTag("Player");
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
            Key.GetComponent<KeyEnemy>().MoveFlag = true;
        }
    }


#if UNITY_EDITOR
    //サーチ範囲を表示
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 1f, 0, 1f);

        if (Key.GetComponent<KeyEnemy>().MoveFlag == true)
        {
            Gizmos.DrawRay(transform.position, transform.forward * 50f);
        }
    }
#endif
}
