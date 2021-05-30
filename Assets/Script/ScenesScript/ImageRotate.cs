using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageRotate : MonoBehaviour
{
    [SerializeField, Header("回転させる軸")]
    Vector3 axis = Vector3.zero;
    [SerializeField, Header("回転角度")]
    float angle = 0;
    [SerializeField, Header("回転速度")]
    float rotateSpeed = 0;

    Quaternion defaultAngle;//初期角度

    float time = 0;

    // Start is called before the first frame update
    void Start()
    {
        defaultAngle = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        transform.rotation = Quaternion.AngleAxis(Mathf.Sin(time * rotateSpeed) * angle, axis) * defaultAngle;
    }
}
