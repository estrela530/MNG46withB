using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーが飛ばす欠片クラス
/// </summary>
public class Fragment : MonoBehaviour
{
    [SerializeField,Tooltip("欠片の速度")]
    private float speed = 5f; //欠片の速度
    private float floatAngle; //飛ばす角度
    Vector3 vectorAngle;      //飛ばす角度
    private float deleteCount;//存在時間
    private int deleteTimer;  //カウント用
    Vector3 parentPosition;   //親の位置を取得

    [SerializeField,Header("回復玉のプレファブを入れてね")]
    private GameObject healBall;
    
    

    /// <summary>
    /// 初期化&生成
    /// </summary>
    /// <param name="angle">発射角度</param>
    /// <param name="position">生成位置</param>
    /// <param name="deleteCount">存在時間</param>
    public void Initialize(float angle, Vector3 position, float deleteCount)
    {
        this.floatAngle = angle;
        //位置をちょっと高くする
        this.parentPosition = new Vector3(position.x, position.y + 0.5f, position.z);
        this.deleteCount = deleteCount;

        //位置初期化
        transform.position = this.parentPosition;

        //テスト↓ : 色変え
        GetComponent<Renderer>().material.color = Color.red;
    }

    /// <summary>
    /// 初期化&生成
    /// </summary>
    /// <param name="angle">発射角度</param>
    /// <param name="position">発射位置</param>
    /// <param name="deleteCount">生存時間</param>
    public void Initialize(Vector3 angle, Vector3 position, float deleteCount,float speed)
    {
        this.vectorAngle = angle;
        //位置をちょっと高くする
        this.parentPosition = new Vector3(position.x, position.y + 0.3f, position.z);
        this.deleteCount = deleteCount;

        this.speed = speed;

        //位置初期化
        transform.position = this.parentPosition;

        //テスト↓ : 色変え
        GetComponent<Renderer>().material.color = Color.red;
    }

    /// <summary>
    /// FPS固定のUpdate
    /// </summary>
    private void FixedUpdate()
    {
        Move();
        ResetTimeMeasure();
    }

    /// <summary>
    /// 時間を計測する
    /// </summary>
    void ResetTimeMeasure()
    {
        deleteTimer++;
        //1時間になったらリセットする
        if (deleteTimer > (60 * deleteCount))
        {
            ResetPosition();
        }
    }

    /// <summary>
    /// 角度を計算して移動させる
    /// </summary>
    private void Move()
    {
        Vector3 velocity = Vector3.zero;

        //角度を計算して移動量に変換する
        //velocity = GetDirection(angle);

        velocity = vectorAngle;

        transform.position += vectorAngle * Time.deltaTime * speed;

        //移動処理
        //transform.position += velocity * Time.deltaTime * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        //テスト↓ : 壁に当たったら削除
        if (other.gameObject.CompareTag("Wall"))
        {
            //回復玉の生成位置を決める
            Vector3 pos = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z);
           
            GameObject pre = Instantiate(healBall, new Vector3(pos.x, pos.y + 0.2f, pos.z), Quaternion.identity) as GameObject;

            ResetPosition();
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            Vector3 pos = this.transform.position;

            ResetPosition();
        }
    }

    /// <summary>
    /// オブジェクトを待機状態にする
    /// </summary>
    private void ResetPosition()
    {
        gameObject.SetActive(false);
        deleteTimer = 0;
        transform.localPosition = parentPosition;//位置も戻す
    }

    /// <summary>
    /// 度からラジアンに変換
    /// </summary>
    /// <param name="deg">角度</param>
    /// <returns>ラジアン角</returns>
    float DegreeToRadian(float deg)
    {
        return deg * Mathf.PI / 180;
    }

    /// <summary>
    /// 指定された角度をベクトルに変換する
    /// </summary>
    /// <param name="angle">回転角度</param>
    Vector3 GetDirection(float angle)
    {
        Vector3 direction = Vector3.zero;

        direction.x = Mathf.Cos(DegreeToRadian(angle));
        direction.z = Mathf.Sin(DegreeToRadian(angle));

        return direction;
    }
}
