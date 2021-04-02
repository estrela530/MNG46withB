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

    Vector3 position;//位置の保存用

    private Ray ray;
    RaycastHit raycastHit;

    MeshRenderer targetRender;
    MeshRenderer alphaMaterial;


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

        //常にカメラから

        ray = new Ray(position, - (position - player.transform.position));

        if(Physics.Raycast(ray,out raycastHit))
        {
            if (raycastHit.collider.CompareTag("Wall"))
            {
                targetRender = raycastHit.collider.gameObject.GetComponent<MeshRenderer>();

                if(targetRender.material.color == Color.white)
                {
                    targetRender.material.color = new Color(1, 0, 1, 0.2f);
                }


            }
            else
            {
                if(targetRender != null)
                {
                    targetRender.material.color = new Color(1, 1, 1, 1);
                }
                
            }
        }
    }

    /// <summary>
    /// 位置の変更(プレイヤーが中心)
    /// </summary>
    void SetPosision()
    {
        position = player.transform.position + offsetPos;

        //値を反映
        transform.position = position;
    }

    /// <summary>
    /// 角度の変更
    /// </summary>
    void SetAngle()
    {
        //値を反映
        this.transform.eulerAngles = offsetRot;
    }
}
