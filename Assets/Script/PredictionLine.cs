using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾道予測線クラス
/// </summary>
public class PredictionLine : MonoBehaviour
{
    [SerializeField, Header("予測線の色")]
    Material predictionColor;

    Vector3 angle;         //回転角度
    Vector3 position;      //生成位置
    //GameObject child;      //子オブジェクト
    //bool colorFlag = false;//色変更フラグ

    Ray lineRay;              //レイの情報
    RaycastHit hit;           //当たったオブジェクトの情報
    Vector3 hitPosition;      //当たった位置の保存用
    Vector3 direction;        //レイの角度
    float maxDistance = 10.0f;//最大描画距離

    LineRenderer lineRenderer;//線の描画
    bool isDraw = false;

    void Awake()
    {
        lineRenderer = this.gameObject.GetComponent<LineRenderer>();

        lineRenderer.material = predictionColor;
        //lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        //lineRenderer.startColor = Color.red;
        //lineRenderer.endColor = Color.white;
        //lineRenderer.startWidth = 0.1f;
        //lineRenderer.endWidth = 0.0f;
        lineRay = new Ray();
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

        //if(!colorFlag)
        //{
        //    //色は1度しか変更しない
        //    child = transform.GetChild(0).gameObject;
        //    child.GetComponent<Renderer>().material.color = predictionColor;
        //    colorFlag = true;
        //}

        //オブジェクトを回転する
        transform.rotation = Quaternion.Euler(0, VectorToAngle(-angle), 0);
        transform.position = position;

        //レイの生成
        //lineRay = new Ray(position, -transform.forward);
        lineRay.origin = position;
        lineRay.direction = -transform.forward;
        //始点の設定
        lineRenderer.SetPosition(0, position);
        //描画距離と方向の乗算
        direction = -transform.forward * maxDistance;
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
        //result = Mathf.Atan2(axis.z, axis.x) * (360 / (Mathf.PI * 2));

        return result;
    }

    private void OnDisable()
    {
        lineRay.origin = Vector3.zero;
        lineRay.direction = Vector3.zero;
        direction = Vector3.zero;
    }

    private void FixedUpdate()
    {
        //現在の問題点
        //予測線が解放したときに残ってしまう(壁の向こう側とかに出てきちゃう)
        //予測線の生成時、角度修正が入って、一瞬変に見える。


        if (Physics.Raycast(lineRay, out hit, maxDistance))
        {
            //回復玉なら反応しない
            if (hit.collider.gameObject.CompareTag("HealBall"))
            {
                isDraw = false;
            }
            else
            {
                //当たった位置を保存
                hitPosition = hit.point;
                isDraw = true;
            }
        }
        else
        {
            isDraw = false;
        }

        if (isDraw)
        {
            //終点の設定
            lineRenderer.SetPosition(1, hitPosition);
        }
        else
        {
            //終点の設定
            lineRenderer.SetPosition(1, position + direction);
        }
        //Debug.DrawRay(lineRay.origin, lineRay.direction * maxDistance, Color.red, 0.1f);
    }
}
