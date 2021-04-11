﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾道予測線クラス
/// </summary>
public class PredictionLine : MonoBehaviour
{
    [SerializeField, Header("予測線の色")]
    Material predictionColor;
    [SerializeField,Header("最大描画距離")]
    float maxDistance = 10.0f;

    Vector3 angle;         //回転角度
    Vector3 position;      //生成位置

    Ray lineRay;              //レイの情報
    RaycastHit hit;           //当たったオブジェクトの情報
    Vector3 hitPosition;      //当たった位置の保存用
    Vector3 direction;        //レイの角度

    LineRenderer lineRenderer;//線の描画

    Vector3 firstPos;//最初に当たった位置を保存しておく

    void Awake()
    {
        lineRay = new Ray();
        lineRenderer = this.gameObject.GetComponent<LineRenderer>();
        lineRenderer.material = predictionColor;
        //lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        //lineRenderer.startColor = Color.red;
        //lineRenderer.endColor = Color.white;
        //lineRenderer.startWidth = 0.1f;
        //lineRenderer.endWidth = 0.0f;    
    }

    /// <summary>
    /// 予測線生成
    /// </summary>
    /// <param name="angle">回転角度</param>
    /// <param name="position">生成位置</param>
    public void Initialize(Vector3 angle, Vector3 position)
    {
        this.angle = angle;
        this.position = position;

        //オブジェクトを回転する
        transform.rotation = Quaternion.Euler(0, VectorToAngle(-angle), 0);
        transform.position = position;

        //レイの生成4/7ここでレイを生成してみた
        lineRay.origin = position;
        lineRay.direction = -transform.forward;
        //始点の設定
        lineRenderer.SetPosition(0, position);
        //描画距離と方向の乗算
        direction = -transform.forward * maxDistance;

        if (Physics.Raycast(lineRay, out hit, maxDistance))
        {
            if (hit.collider.gameObject.CompareTag("Wall"))
            {
                //当たった位置を保存
                hitPosition = hit.point;

            }
            else
            {
                //初期化
                hitPosition = position + direction;
            }
        }

        //初めに描画する
        lineRenderer.SetPosition(1, hitPosition);
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

        result = Mathf.Atan2(axis.x, axis.z) * (360 / (Mathf.PI * 2));

        return result;
    }

    private void FixedUpdate()
    {
        foreach (RaycastHit hit in Physics.RaycastAll(lineRay))
        {
            if (hit.collider.gameObject.CompareTag("Wall"))
            {
                //当たった位置を保存
                hitPosition = hit.point;
                lineRenderer.SetPosition(1, hitPosition);
            }
            else
            {
                lineRenderer.SetPosition(1, hitPosition);
            }
        }


        //if (Physics.Raycast(lineRay, out hit, maxDistance))
        //{
        //    //回復玉なら反応しない
        //    if (hit.collider.gameObject.CompareTag("HealBall"))
        //    {
        //        lineRenderer.SetPosition(1, hitPosition);
        //        //isDraw = false;
        //    }
        //    else if (hit.collider.gameObject.CompareTag("Wall"))
        //    {
        //        //当たった位置を保存
        //        hitPosition = hit.point;
        //        lineRenderer.SetPosition(1, hitPosition);
        //        //isDraw = true;
        //    }
        //    else
        //    {
        //        lineRenderer.SetPosition(1, hitPosition);
        //    }
        //}
        //else
        //{
        //    lineRenderer.SetPosition(1, hitPosition);
        //    //isDraw = false;
        //}
        //if (isDraw)
        //{
        //    //終点の設定
        //    lineRenderer.SetPosition(1, hitPosition);
        //}
        //else
        //{
        //    //終点の設定
        //    lineRenderer.SetPosition(1, hitPosition);
        //}
        //Debug.DrawRay(lineRay.origin, lineRay.direction * maxDistance, Color.red, 0.1f);
    }
}
