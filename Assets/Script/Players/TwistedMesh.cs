using UnityEngine;

/// <summary>
/// 頂点をねじる
/// </summary>
public class TwistedMesh : MonoBehaviour
{
    [SerializeField, Header("ねじれる速度")]
    private float twistedSpeed = 10.8f;
    [SerializeField, Header("ねじれがもどる速度")]
    private float releaseSpeed = 50f;
    [SerializeField, Header("ねじれる量の上限")]
    private float maxTwistedCount = 720.0f;

    private float twistedCount;//ねじれ量

    //メッシュの回転
    MeshFilter meshFilter;
    private Vector3 axis;         //ねじれる軸
    private Vector3[] vertices;   //オリジナル(元の)頂点
    private Vector3[] newVertices;//ねじった後の頂点

    //変数取得ステップ
    GameObject parent;//ステップ①:親オブジェクト取得
    Player player;    //ステップ②:プレイヤースクリプト取得
    bool isTwisted;   //ステップ③:変数取得
    bool isReset;     //ステップ③:変数取得

    // Start is called before the first frame update
    void Start()
    {
        //このオブジェクトのメッシュフィルターを取得
        meshFilter = GetComponent<MeshFilter>();
        //ねじれる軸を設定
        axis = new Vector3(0, 1, 0);
        //このオブジェクトのメッシュポリゴンの頂点を取得
        vertices = meshFilter.mesh.vertices;
        //元の頂点数分のVector3を用意
        newVertices = new Vector3[vertices.Length];

        //変数取得ステップ
        parent = transform.parent.gameObject;  //①
        player = parent.GetComponent<Player>();//②
        isTwisted = player.GetTwisted();       //③
        isReset = player.isReset;              //③

        //Debug.Log("頂点数" + vertices.Length);
    }

    private void FixedUpdate()
    {
        ChangeAngle();    //ねじれ角度を変える
        TwistedVertices();//ねじれを反映させる
    }

    /// <summary>
    /// ねじれを元に戻す
    /// </summary>
    void ResetAngle()
    {
        twistedCount = 0;
        player.isReset = false;
    }

    /// <summary>
    /// ねじれる
    /// </summary>
    void ChangeAngle()
    {
        //ねじれているか取得
        isTwisted = player.GetTwisted();
        //リセットしたか
        isReset = player.isReset;

        if (isReset)
        {
            ResetAngle();
        }

        if (isTwisted)
        {
            if (twistedCount >= maxTwistedCount) return;

            //伸びる速度
            twistedCount += twistedSpeed;

            if (twistedCount > maxTwistedCount)
            {
                twistedCount = maxTwistedCount;
            }
        }
        else
        {
            if (twistedCount <= 0) return;

            //縮む速度
            twistedCount -= releaseSpeed;

            if (twistedCount < 0)
            {
                twistedCount = 0;
            }
        }
    }

    /// <summary>
    /// メッシュを回転させる
    /// </summary>
    void TwistedVertices()
    {
        //配列[].Length = その配列の要素数を取得
        for (int i = 0; i < newVertices.Length; i++)
        {
            newVertices[i] = Quaternion.Euler(axis * twistedCount * vertices[i].y) * vertices[i];
        }

        //法線の更新 法線ベクトルを自動で算出
        meshFilter.mesh.RecalculateNormals();

        //ねじった後の頂点を反映
        meshFilter.mesh.vertices = newVertices;
    }
}
