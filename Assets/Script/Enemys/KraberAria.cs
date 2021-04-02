using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class KraberAria : MonoBehaviour
{
    [SerializeField] private SpherecastCommand searchArea;//サーチ範囲
    [SerializeField] public float searchAngle;
    public GameObject Kraber;

    Rigidbody rigid;
    // Start is called before the first frame update
    void Start()
    {
        Kraber.GetComponent<KraberEnemy>();
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
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
                Kraber.GetComponent<KraberEnemy>().MoveFlag = true;
                Kraber.GetComponent<KraberEnemy>().workFlag = false;
                //MoveFlag = true;
                //workFlag = false;
                // Debug.Log("主人公発見: " + angle);
            }

            ////サーチする角度の範囲外だったら索敵
            //if (searchAngle <= angle)
            //{
            //    Kraber.GetComponent<KraberEnemy>().MoveFlag = false;
            //    Kraber.GetComponent<KraberEnemy>().workFlag = true;
            //    //Debug.Log("外: " + angle);
            //}
            
        }

        if(other.gameObject.CompareTag("Wall"))
        {
            Kraber.GetComponent<KraberEnemy>().MoveFlag = false;
            Kraber.GetComponent<KraberEnemy>().workFlag = true;
        }

    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            Kraber.GetComponent<KraberEnemy>().MoveFlag = false;
            Kraber.GetComponent<KraberEnemy>().workFlag = true;
        }
        ////サーチする角度の範囲外だったら索敵
        //if (!other.gameObject.CompareTag("Player"))
        //{
        //    Kraber.GetComponent<KraberEnemy>().MoveFlag = false;
        //    Kraber.GetComponent<KraberEnemy>().workFlag = true;
        //}
    }


#if UNITY_EDITOR
    //サーチ範囲を表示
    private void OnDrawGizmos()
    {
        Handles.color = Color.blue;
        Handles.DrawSolidArc(transform.position,
            Vector3.up,
            Quaternion.Euler(0f, -searchAngle, 0f) * transform.forward
            , searchAngle * 2f,
            4f);
    }
#endif
}
