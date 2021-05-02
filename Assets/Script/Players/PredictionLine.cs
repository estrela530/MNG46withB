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
    float maxDistance = 0f;//最大描画距離(レベル依存)

    Vector3 angle;      //回転角度
    Vector3 position;   //生成位置
    Vector3 hitPosition;//当たった位置の保存用
    Vector3 direction;  //レイの角度
    Vector3 firstPos;   //最初に当たった位置を保存しておく

    Ray lineRay;              //レイの情報
    RaycastHit hit;           //当たったオブジェクトの情報
    LineRenderer lineRenderer;//線の描画

    int layerMask = ~(1 << 10);//無視するレイヤーの設定

    void Awake()
    {
        lineRay = new Ray();
        lineRenderer = this.gameObject.GetComponent<LineRenderer>();
        lineRenderer.material = predictionColor;
        #region スクリプト側で値を設定する場合
        //lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        //lineRenderer.startColor = Color.red;
        //lineRenderer.endColor = Color.white;
        //lineRenderer.startWidth = 0.1f;
        //lineRenderer.endWidth = 0.0f;    
        #endregion
    }

    /// <summary>
    /// 予測線生成
    /// </summary>
    /// <param name="angle">回転角度</param>
    /// <param name="position">生成位置</param>
    /// <param name="distance">長さ</param>
    public void Initialize(Vector3 angle, Vector3 position, float distance)
    {
        this.angle = angle;
        this.position = position;
        maxDistance = distance;

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
        //表示位置は初期化しておこうね!
        hitPosition = position + direction;

        DrawLine();
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
        DrawLine();
    }

    private void DrawLine()
    {
        //Wall = 当たった位置まで予測線を伸ばす
        //Enemy = 当たった位置まで予測線を伸ばす

        //【Tips】Raycast(使用するレイ、当たった情報、長さ、無視するレイヤー)
        if (Physics.Raycast(lineRay, out hit, maxDistance, layerMask))
        {
            if (hit.collider.gameObject.CompareTag("Wall") ||
                hit.collider.gameObject.CompareTag("Enemy"))
            {
                //当たった位置を保存
                hitPosition = hit.point;

            }
        }
        //描画する
        lineRenderer.SetPosition(1, hitPosition);

        ////レイを可視化する
        //Debug.DrawRay(lineRay.origin, lineRay.direction * maxDistance, Color.red, 0.1f);
    }
}
