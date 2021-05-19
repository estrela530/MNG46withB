using UnityEngine;

/// <summary>
/// カメラ追従クラス
/// </summary>
public class CameraMove : MonoBehaviour
{
    [SerializeField,Tooltip("シーン内のプレイヤーをドラッグアンドドロップ")]
    protected GameObject player;
    [SerializeField,Tooltip("原点からの距離(プレイヤーが原点)")]
    protected Vector3 offsetPos = new Vector3(2.5f, 10f, -5);
    [SerializeField,Tooltip("角度のオフセット")]
    protected Vector3 offsetRot = new Vector3(60, -30, 0);

    protected Vector3 position;//位置の保存用

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
    public virtual void SetPosision()
    {
        position = player.transform.position + offsetPos;

        //値を反映
        transform.position = position;
    }

    /// <summary>
    /// 角度の変更
    /// </summary>
    public virtual void SetAngle()
    {
        //値を反映
        this.transform.eulerAngles = offsetRot;
    }
}
