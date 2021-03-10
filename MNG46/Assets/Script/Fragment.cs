using UnityEngine;

/// <summary>
/// プレイヤーの飛ばした欠片
/// </summary>
public class Fragment : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;//欠片の速度
    private float angle;     //飛ばす角度
    Vector3 parentPosition;  //親の位置を取得

    [SerializeField, Header("仮)消えるまでの時間")]
    private int deleteCount = 1;
    private int deleteTimer = 0;//カウント用

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
        if (other.gameObject.tag == "Respawn")
        {
            ResetPosition();
        }
    }
}
