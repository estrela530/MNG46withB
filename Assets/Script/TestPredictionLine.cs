using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// その都度LineRendererの生成を試してみる
/// </summary>
public class TestPredictionLine : MonoBehaviour
{
    [SerializeField, Header("予測線の色")]
    Material predictionColor;
    [SerializeField, Header("最大描画距離")]
    float maxDistance = 10.0f;

    Vector3 angle;         //回転角度
    Vector3 position;      //生成位置

    Ray lineRay;              //レイの情報
    RaycastHit hit;           //当たったオブジェクトの情報
    Vector3 hitPosition;      //当たった位置の保存用
    Vector3 direction;        //レイの角度

    LineRenderer lineRenderer;//線の描画

    void Awake()
    {
        Debug.Log(this.name + "非表示だけど呼ばれたよ");

        //lineRenderer = gameObject.AddComponent<LineRenderer>();

        //lineRenderer.material = predictionColor;
        //lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        //lineRenderer.startColor = Color.red;
        //lineRenderer.endColor = Color.white;
        //lineRenderer.startWidth = 0.1f;
        //lineRenderer.endWidth = 0.0f;
        lineRay = new Ray();
    }

    private void Start()
    {
        Debug.Log(this.name + "Startだよ");
    }

    private void OnEnable()
    {
        Debug.Log(this.name + "OnEnableだよ");
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

        if(lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        lineRenderer.material = predictionColor;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        //オブジェクトを回転する
        transform.rotation = Quaternion.Euler(0, VectorToAngle(-angle), 0);
        transform.position = position;

        //レイの生成4/7ここでレイを生成してみた
        //lineRay = new Ray(position, -transform.forward);
        lineRay.origin = position;
        lineRay.direction = -transform.forward;
        //始点の設定
        lineRenderer.SetPosition(0, position);

        //描画距離と方向の乗算
        direction = -transform.forward * maxDistance;

        //初期化
        hitPosition = position + direction;

        //4/7の考察
        //Startの角度調整より先にUpdateの描画が呼ばれてしまっていると予想。
        //ならば、Startで一回初期の値で描画してしまえばいいのでは？
        //これだったらおそらくUpdateの描画よりも速く呼ばれるため、
        //一回目の描画は初期値で描画し、2フレーム目以降の描画は
        //Updateの処理に適した描画になってくれるのではないだろうか?

        //実際にここで描画したらラグはかなり縮まった。
        lineRenderer.SetPosition(1, hitPosition);

        //Slackに書く。
        /*おはようございます
         　前日に問題だった①の回転時の描画のラグなのですが、やり方が正しいかはわかりませんが、
           「Startの処理が全て終わる前に、Updateの描画が呼ばれているのでは?」
           という結果に至り、Startで回転の角度を求めた瞬間に1度描画して、
           Updateで再度描画しなおすことでラグが気にならない程度まで抑えることができました。*/

        if (Physics.Raycast(lineRay, out hit, maxDistance))
        {
            if (hit.collider.gameObject.CompareTag("Wall"))
            {
                //当たった位置を保存
                hitPosition = hit.point;
                
            }
        }
    }

    /// <summary>
    /// 軸(Vector3)をラジアン角(float)に変換する
    /// </summary>
    /// <param name="axis">軸</param>
    /// <returns></returns>
    float VectorToAngle(Vector3 axis)
    {
        float result;

        result = Mathf.Atan2(axis.x, axis.z) * (360 / (Mathf.PI * 2));

        return result;
    }

    private void OnDisable()
    {
        lineRay.origin = Vector3.zero;
        lineRay.direction = Vector3.zero;
        direction = Vector3.zero;

        //if(lineRenderer != null)
        //{
        //    Destroy(gameObject.GetComponent<LineRenderer>());
        //}
        
    }

    private void FixedUpdate()
    {
        if (direction == Vector3.zero) return;

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
    }
}
