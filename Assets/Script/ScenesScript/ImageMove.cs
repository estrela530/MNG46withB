using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageMove : MonoBehaviour
{

    [SerializeField, Header("動かす方向")]
    Vector3 direction = Vector3.zero;

    [SerializeField, Header("動かす速度")]
    float speed = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.position += direction * speed * Time.deltaTime;
    }
}
