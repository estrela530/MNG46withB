using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾道予測線クラス
/// </summary>
public class PredictionLine : MonoBehaviour
{
    [SerializeField, Header("予測線の色")]
    Color predictionColor;

    Vector3 angle;         //回転角度
    Vector3 position;      //生成位置
    GameObject child;      //子オブジェクト
    bool colorFlag = false;//色変更フラグ

    /// <summary>
    /// 予測線生成
    /// </summary>
    /// <param name="angle">回転角度</param>
    /// <param name="position">生成位置</param>
    public void Initialize(Vector3 angle, Vector3 position)
    {
        this.angle = angle;
        this.position = position;

        if(!colorFlag)
        {
            //色は1度しか変更しない
            child = transform.GetChild(0).gameObject;
            child.GetComponent<Renderer>().material.color = predictionColor;
            colorFlag = true;
        }

        //オブジェクトを回転する
        transform.rotation = Quaternion.Euler(0, VectorToAngle(angle), 0);
        transform.position = position;
    }

    /// <summary>
    /// 軸(Vector3)をラジアン角(float)に変換する
    /// </summary>
    /// <param name="axis">軸</param>
    /// <returns></returns>
    float VectorToAngle(Vector3 axis)
    {
        float result;

        // https://docs.unity3d.com/ja/current/ScriptReference/Mathf.Atan2.html
        //Atan2 : 二点の角度を取得する
        //  |    ②           |
        //  |   /           ②|-------○←ここの角度
        //  |  /              |        |
        //  | /↑             |        |
        //  ①---------0°    0--------------
        //                            ①

        //result = Mathf.Atan2(axis.x, axis.z) * Mathf.Rad2Deg;
        result = Mathf.Atan2(axis.x, axis.z) * (360 / (Mathf.PI * 2));

        return result;
    }
}
