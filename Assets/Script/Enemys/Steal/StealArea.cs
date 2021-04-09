using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class StealArea : MonoBehaviour
{

    [SerializeField] private SpherecastCommand searchArea;//サーチ範囲
    [SerializeField] public float searchAngle;
    public GameObject Move;

    [SerializeField]
    float radius;

    // Start is called before the first frame update
    void Start()
    {
        Move.GetComponent<StealEnemy>();

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnTriggerStay(Collider other)
    {
        //プレイヤーの方向
        var playerDire = other.transform.position - this.transform.position;

        //自分の前方からプレイヤーの方向
        var angle = Vector3.Angle(transform.forward, playerDire);

        //触れているとき
        if (other.gameObject.CompareTag("HealBall"))
        {
            //サーチする角度の範囲内だったら発見
            if (angle <= searchAngle)
            {
                Move.GetComponent<StealEnemy>().MoveFlag = true;
                Move.GetComponent<StealEnemy>().workFlag = false;
            }

            ////サーチする角度の範囲外だったら索敵
            //if (searchAngle <= angle)
            //{
            //    Move.GetComponent<StealEnemy>().MoveFlag = false;
            //    Move.GetComponent<StealEnemy>().workFlag = true;
            //    //Debug.Log("外: " + angle);
            //}


        }

    }
    public void OnTriggerEnter(Collider other)
    {
        ////サーチする角度の範囲外だったら索敵
        //if (!other.gameObject.CompareTag("HealBall"))
        //{
        //    Move.GetComponent<StealEnemy>().MoveFlag = false;
        //    Move.GetComponent<StealEnemy>().workFlag = true;
        //}
    }


#if UNITY_EDITOR
    //サーチ範囲を表示
    private void OnDrawGizmos()
    {
        Handles.color = Color.yellow;
        Handles.DrawSolidArc(transform.position,
            Vector3.up,
            Quaternion.Euler(0f, -searchAngle, 0f) * transform.forward
            , searchAngle * 2f,
            radius);
    }
#endif

}
