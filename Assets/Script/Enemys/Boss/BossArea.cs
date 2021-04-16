using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

public class BossArea : MonoBehaviour
{

    //[SerializeField] public float searchAngle;
    [SerializeField] GameObject Boss;

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

  

#if UNITY_EDITOR
    //サーチ範囲を表示
    private void OnDrawGizmos()
    {
        Handles.color = new Color(0.0f,1.0f,0.0f,0.5f);

        if(Boss.GetComponent<BossMove>().RushFlag == true)
        {
            //Handles.DrawSolidDisc()
            //Handles.DrawWireRegularPolygon(4, new Vector3(24f, 0f, 0f), Quaternion.LookRotation(new Vector3(-1f, 1f, 0f)), 2f);
        }
        
    }
#endif
}
