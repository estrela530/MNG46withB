using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnemyArea : MonoBehaviour
{

    private SpherecastCommand searchArea;//サーチ範囲
    [SerializeField] public float searchAngle;
    public GameObject Move;

    // Start is called before the first frame update
    void Start()
    {
        Move.GetComponent<EnemyMove>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        //プレイヤーの方向
        var playerDire = other.transform.position - this.transform.position;

        //自分の前方からプレイヤーの方向
        var angle = Vector3.Angle(transform.forward, playerDire);

        if (other.gameObject.CompareTag("Player"))
        {
            //サーチする角度の範囲内だったら発見
            if (angle <= searchAngle)
            {
                Move.GetComponent<EnemyMove>().MoveFlag = true;
                Move.GetComponent<EnemyMove>().workFlag = false;
                //MoveFlag = true;
                //workFlag = false;
                Debug.Log("主人公発見: " + angle);
            }
            ////サーチする角度の範囲内だったら発見
            //else
            //{
            //    Debug.Log("範囲外: " + angle);
            //}

        }

        ////サーチする角度の範囲外だったら索敵
        if (searchAngle <= angle)
        {
            Move.GetComponent<EnemyMove>().MoveFlag = false;
            Move.GetComponent<EnemyMove>().workFlag = true;
            Debug.Log("外: " + angle);
        }

        if (!other.gameObject.CompareTag("Player"))
        {
            //MoveFlag = false;
            //workFlag = true;
            Move.GetComponent<EnemyMove>().workFlag = true;
            Move.GetComponent<EnemyMove>().MoveFlag = false;
            //Move.GetComponent<EnemyMove>().workFlag = true;
            //Debug.Log("範囲外: " + angle);
        }

    }


#if UNITY_EDITOR
    //サーチ範囲を表示
    private void OnDrawGizmos()
    {
        Handles.color = Color.red;
        //Handles.DrawSolidDisc(transform.position, Vector3.up, 3.0f);
        //Handles.DrawSolidArc(transform.position, Vector3.up, Quaternion.Euler(0f, -searchAngle, 0f) * transform.forward, searchAngle * 5f, HandleUtility.GetHandleSize(Vector3.zero));
        Handles.DrawSolidArc(
            this.transform.position, //中心点
            Vector3.up, //表示する表面の方向
            Quaternion.Euler(0f, -searchAngle, 0f) * this.transform.forward, //扇の表示を開始する方向
            searchAngle, //扇の角度
            searchArea.radius//半径
            );
    }
#endif
}
