using UnityEngine;

/// <summary>
/// カメラ追従クラス
/// </summary>
public class CameraMove : MonoBehaviour
{
    [SerializeField,Tooltip("シーン内のプレイヤーをドラッグアンドドロップ")]
    private GameObject player;
    [SerializeField,Tooltip("原点からの距離(プレイヤーが原点)")]
    private Vector3 offsetPos = new Vector3(2.5f, 10f, -5);
    [SerializeField,Tooltip("角度のオフセット")]
    private Vector3 offsetRot = new Vector3(60, -30, 0);

    Vector3 position;

    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        SetPosision();
        SetAngle();
    }

    /// <summary>
    /// 位置の変更(プレイヤーが中心)
    /// </summary>
    void SetPosision()
    {
        position = transform.position;

        position = player.transform.position + offsetPos;

        //position.y = Mathf.Clamp(position.y, -10, 15);

        //ここに値を入れておく
        transform.position = position;
    }

    /// <summary>
    /// 角度の変更
    /// </summary>
    void SetAngle()
    {
        //回転角度はこれで固定かな...
        this.transform.eulerAngles = offsetRot;
    }
}
