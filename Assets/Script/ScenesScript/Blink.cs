using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blink : MonoBehaviour
{

    //public
    public float speed = 1.0f;

    //private
    private Text text;
    private Image image;
    private float time;
    private GameObject gameObject;
    private MeshRenderer meshRenderer;
    private int alphaCount = 0;//点滅用カウント

    public bool fadeFlag = true;

    private enum ObjType
    {
        TEXT,
        IMAGE,
        GAMEOBJECT
    };
    private ObjType thisObjType = ObjType.TEXT;

    void Start()
    {
        //アタッチしてるオブジェクトを判別
        if (this.gameObject.GetComponent<Image>())
        {
            thisObjType = ObjType.IMAGE;
            image = this.gameObject.GetComponent<Image>();
        }
        else if (this.gameObject.GetComponent<Text>())
        {
            thisObjType = ObjType.TEXT;
            text = this.gameObject.GetComponent<Text>();
        }
        else if (this.gameObject.GetComponent<GameObject>())
        {
            thisObjType = ObjType.GAMEOBJECT;
            gameObject = this.gameObject.GetComponent<GameObject>();
            meshRenderer = gameObject.GetComponent<MeshRenderer>();
        }


        fadeFlag = true;
    }

    void Update()
    {
        //オブジェクトのAlpha値を更新
        if (thisObjType == ObjType.IMAGE)
        {
            image.color = GetAlphaColor2(image.color);
        }
        else if (thisObjType == ObjType.TEXT)
        {
            text.color = GetAlphaColor(text.color);
        }
        else if (thisObjType == ObjType.GAMEOBJECT)
        {
            //meshRenderer.material.color

            alphaCount++;

            if (alphaCount > 0)
            {
                meshRenderer.material.color = Color.red;
            }
            if (alphaCount > 4)
            {
                meshRenderer.material.color = Color.white;
            }
            if (alphaCount > 8)
            {
                alphaCount = 0;
            }
        }

    }

    //Alpha値を更新してColorを返す
    Color GetAlphaColor(Color color)
    {
        time += Time.deltaTime * 5.0f * speed;
        color.a = Mathf.Sin(time) * 0.5f + 0.5f;

        return color;
    }


    //Alpha値を更新してColorを返す
    Color GetAlphaColor2(Color color)
    {
        if (color.a <= 1 && fadeFlag)
        {
            Debug.Log("image");
            time += Time.deltaTime * speed;
            color.a = time * 0.5f + 0.5f;
        }
        else
        {
            fadeFlag = false;
        }

        //if (!fadeFlag && this.gameObject.activeSelf == false)
        //{
        //    color.a = 0;
        //    time = 0;
        //}

        return color;
    }

}