using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

public class BossArea : MonoBehaviour
{

    //[SerializeField] public float searchAngle;
    [SerializeField] GameObject Boss;
    private GameObject Target;//追尾する相手
    [SerializeField]
    float radius;
    private float dis;//プレイヤーとの距離
    Rigidbody rigid;
    public Mesh mesh;

    // Start is called before the first frame update
    void Start()
    {
        Boss.GetComponent<BossMove>();
        rigid = GetComponent<Rigidbody>();
        Target = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rigid.angularVelocity = Vector3.zero;
        rigid.velocity = Vector3.zero;
        //dis = Vector3.Distance(transform.position, Target.transform.position);//二つの距離を計算して一定以下になれば追尾

        //this.transform.LookAt(new Vector3(Target.transform.position.x, this.transform.position.y, Target.transform.position.z));//ターゲットにむく
    }


#if UNITY_EDITOR
    //サーチ範囲を表示
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 1f, 0, 1f);

        if (Boss.GetComponent<BossMove>().RushFlag == true)
        {
            Gizmos.DrawRay(transform.position, transform.forward * 50f);
        }
        
    }
#endif
}
