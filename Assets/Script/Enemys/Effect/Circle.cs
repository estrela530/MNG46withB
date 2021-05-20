using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Circle : MonoBehaviour
{
    private float turnY = 0;
    GameObject image_object;
    // Start is called before the first frame update
    void Start()
    {
        // オブジェクトの取得
        GameObject image_object = GameObject.Find("Image");
        // コンポーネントの取得
        Image image_component = image_object.GetComponent<Image>();
         turnY = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //turnY += Time.deltaTime;
        //image_object.transform.Rotate(new Vector3(0, turnY, 0));
       // transform.Rotate( new Vector3(0, turnY, 0));
    }
}
