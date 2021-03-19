using System.Collections.Generic;
using UnityEngine;

public class Fragment : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;//欠片の速度
    private float angle;     //飛ばす角度
    Vector3 parentPosition;  //親の位置を取得


    [SerializeField]
    private GameObject healBall;

    [SerializeField, Header("仮)消えるまでの時間")]
    private float deleteCount = 1;
    private int deleteTimer = 0;//カウント用

    private List<GameObject> children;//子オブジェクトリスト

    //private void Start()
    //{
    //    children = new List<GameObject>();//リストを生成
    //    //最初に二つ作っておく
    //    for (int i = 0; i < 3; i++)
    //    {
    //        GameObject obj = CreateChildren();//オブジェクト生成
    //        obj.SetActive(false);             //生成時は非表示にする
    //        children.Add(obj);                //リストに入れる
    //    }
    //}


    public void Initialize(float angle, Vector3 position)
    {
        this.angle = angle;
        //位置をちょっと高くする
        this.parentPosition = new Vector3(position.x, position.y + 0.5f, position.z);

        //位置初期化
        transform.position = this.parentPosition;

        //テスト↓ : 色変え
        GetComponent<Renderer>().material.color = Color.red;
    }

    // Update is called once per frame
    void Update()
    {
        Move();

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
        velocity = GetDirection(angle);

        //移動処理
        transform.position += velocity * Time.deltaTime * speed;
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

    private void OnTriggerEnter(Collider other)
    {
        //テスト↓ : 壁に当たったら削除(タグも仮置き)
        if (other.gameObject.CompareTag("Wall"))
        {
            Vector3 pos = this.transform.position;

            GameObject pre = Instantiate(healBall, pos, Quaternion.identity) as GameObject;

            //GameObject test = GetObject(); ;//先に作っておく

            //if(test != null)
            //{
            //    test.GetComponent<TestHealBall>().Initialize(pos);
            //}


            ResetPosition();
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            Vector3 pos = this.transform.position;
            
            ResetPosition();
        }
    }

    /// <summary>
    /// 新しく生成し、子オブジェクトに設定して返す
    /// </summary>
    /// <returns></returns>
    private GameObject CreateChildren()
    {
        GameObject obj = Instantiate(healBall);
        obj.transform.parent = this.transform;

        return obj;
    }

    private GameObject GetObject()
    {
        ////使用中でないものを探す
        //foreach (var child in children)
        //{
        //    if (child.activeSelf == false)
        //    {
        //        child.SetActive(true);//使用中にして返す
        //        return child;
        //    }
        //}

        //使用中でないものを探す
        foreach (Transform child in this.transform)
        {
            if (child.gameObject.activeSelf == false)
            {
                child.gameObject.SetActive(true);//使用中にして返す
                return child.gameObject;
            }
        }

        GameObject obj = CreateChildren();
        obj.transform.parent = this.transform;
        obj.SetActive(true);
        children.Add(obj);
        return obj;
    }
}
