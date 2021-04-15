using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

public class BossArea : MonoBehaviour
{
    [SerializeField] private SpherecastCommand searchArea;//サーチ範囲
    [SerializeField] public float searchAngle;
    public GameObject Boss;
    [SerializeField]
    float radius;

    Rigidbody rigid;
    // Start is called before the first frame update
    void Start()
    {
        Boss.GetComponent<BossMove>();
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rigid.angularVelocity = Vector3.zero;
        rigid.velocity = Vector3.zero;
    }

    public void OnTriggerStay(Collider other)
    {
        //プレイヤーの方向
        var playerDire = other.transform.position - this.transform.position;

        //自分の前方からプレイヤーの方向
        var angle = Vector3.Angle(transform.forward, playerDire);

        //触れているとき
        if (other.gameObject.CompareTag("Player"))
        {
            //サーチする角度の範囲内だったら発見
            if (angle <= searchAngle)
            {
                
            }

            //サーチする角度の範囲外だったら索敵
            if (searchAngle <= angle)
            {
                //Boss.GetComponent<KraberEnemy>().MoveFlag = false;
                //Boss.GetComponent<KraberEnemy>().workFlag = true;
                //Debug.Log("外: " + angle);
            }

        }


    }
    public void OnTriggerEnter(Collider other)
    {
        //サーチする角度の範囲外だったら索敵
        if (!other.gameObject.CompareTag("Player"))
        {
            //Boss.GetComponent<KraberEnemy>().MoveFlag = false;
            //Boss.GetComponent<KraberEnemy>().workFlag = true;
        }
    }


#if UNITY_EDITOR
    //サーチ範囲を表示
    private void OnDrawGizmos()
    {
        Handles.color = new Color(0.0f,1.0f,0.0f,0.5f);
        Handles.DrawSolidArc(transform.position,
            Vector3.up,
            Quaternion.Euler(0f, -searchAngle, 0f) * transform.forward
            , searchAngle * 2f,
            radius);
    }
#endif
}
