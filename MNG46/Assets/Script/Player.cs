using UnityEngine;

/// <summary>
/// ねじねじをまとめたクラス
/// </summary>
[RequireComponent(typeof(FragmentPool))]//自動的にFragmentPoolを追加
public class Player : MonoBehaviour
{
    [SerializeField, Header("伸びる速さ")]
    private float extendSpeed = 0.05f;
    [SerializeField, Header("縮む速さ")]
    private float shrinkSpeed = 0.5f;
    [SerializeField, Header("伸びる長さ")]
    private float maxNobiLength = 5.0f;
    [SerializeField/*, ReadOnly*/]
    public int neziCount;//どれくらいねじねじしているか
    [SerializeField, Header("レベルアップに必要なねじカウント")]
    private int[] levelCount = new int[3];//(20,50,70

    [SerializeField, Header("プレイヤーの欠片")]
    private GameObject fragmentPrefab;
    [SerializeField, Header("レベルによる飛ばす球数")]
    private int[] fragmentCount = new int[4];//(64,32,16,8
    [SerializeField]
    private int firstCreateFragment = 45;

    Renderer renderer;            //色変え用
    FragmentPool objectPool;      //オブジェクトプール
    Vector3 myScale = Vector3.one;//自身の大きさ


    private bool isTwisted;//ねじれているかどうか
    /// <summary>
    /// リセットしたかどうか(メッシュ側で取得&代入を行う)
    /// </summary>
    public bool isReset { get; set; } = false;

    private int neziLevel;//ねじレベル

    void Start()
    {
        //ねじれてる本体の色情報を取得
        renderer = transform.GetChild(0).GetComponent<Renderer>();

        objectPool = GetComponent<FragmentPool>();
        //最初の生成を行う
        objectPool.CreatePool(fragmentPrefab, firstCreateFragment);

        Initialize();
    }

    void Initialize()
    {
        isTwisted = false;
        neziCount = 0;
        neziLevel = 0;
        transform.localScale = Vector3.one;
    }

    // Update is called once per frame
    void Update()
    {
        Extend();       //伸びる
        ChangeLevel();  //レベル変更
        TwistedChange();//ねじチェンジ

        //Debug.Log("ねじレベル" + neziLevel);
    }

    /// <summary>
    /// ねじチェンジ
    /// </summary>
    void TwistedChange()
    {
        //ボタンを押している間ねじねじする
        if (Input.GetKey(KeyCode.Space))
        {
            TwistedAccumulate();//ねじねじ

        }
        else if (Input.GetKeyUp(KeyCode.Space))//離したら解放する
        {
            TwistedRelease();//解放
        }
    }

    /// <summary>
    /// 伸びる
    /// </summary>
    void Extend()
    {
        myScale = transform.localScale;

        if (isTwisted)
        {
            //1回maxNobiLengthより大きくなったらこれ以上処理しないね
            if (myScale.y >= maxNobiLength) return;

            //ねじっているとき上に伸びる
            myScale += new Vector3(0, extendSpeed, 0);
            //ねじねじしてる間カウントを増やす
            neziCount++;

            if (myScale.y > maxNobiLength)
            {
                myScale.y = maxNobiLength;
            }
        }
        else
        {
            //1回1以下になったら(以下略
            if (myScale.y <= 1.0f) return;

            myScale += new Vector3(0, -shrinkSpeed, 0);

            if (myScale.y <= 1.0f)
            {
                myScale.y = 1.0f;
            }
        }

        transform.localScale = myScale;
    }

    /// <summary>
    /// カウントによるレベルの変化
    /// </summary>
    void ChangeLevel()
    {
        //ねじカウントの値によるレベルの変化
        if (neziCount >= levelCount[0])
        {
            if (neziCount >= levelCount[2]) neziLevel = 3;
            else if (neziCount >= levelCount[1]) neziLevel = 2;
            else neziLevel = 1;
        }
        else
        {
            neziLevel = 0;
        }

        //常に行われる処理
        //ねじレベルによる色の変化
        switch (neziLevel)
        {
            case 0://レベル0
                renderer.material.color = Color.white;
                break;
            case 1:
                renderer.material.color = Color.magenta;
                break;
            case 2:
                renderer.material.color = Color.yellow;
                break;
            case 3:
                renderer.material.color = Color.red;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// ねじチャージ (Accumulate = 溜める)
    /// </summary>
    void TwistedAccumulate()
    {
        isTwisted = true;
        //neziCount++;//ねじねじしてる間カウントを増やす

        if (Input.GetKeyDown(KeyCode.Z))
        {
            isReset = true;
            Initialize();
        }
    }

    /// <summary>
    /// ねじリリース
    /// </summary>
    void TwistedRelease()
    {
        //常に行われる処理
        //ねじレベルによる色と球数の変化
        switch (neziLevel)
        {
            case 0://レベル0
                renderer.material.color = Color.white;
                break;
            case 1:
                renderer.material.color = Color.magenta;
                TestNeziShoot(fragmentCount[1]);
                break;
            case 2:
                renderer.material.color = Color.yellow;
                TestNeziShoot(fragmentCount[2]);
                break;
            case 3:
                renderer.material.color = Color.red;
                TestNeziShoot(fragmentCount[3]);
                break;
            default:
                break;
        }

        Initialize();
    }

    /// <summary>
    /// 欠片を飛ばす
    /// </summary>
    /// <param name="count">360で割った個数分出てくる</param>
    void TestNeziShoot(int count)
    {
        //ここで解放する
        for (int angle = 0; angle < 361; angle += count)//360で割った個数分出てくる
        {
            GameObject fragment = objectPool.GetObject();//生きているオブジェクトを代入

            if (fragment != null)
            {
                fragment.GetComponent<Fragment>().Initialize(angle, transform.position);
            }
        }
    }

    /// <summary>
    /// ねじねじしているか
    /// </summary>
    /// <returns></returns>
    public bool GetTwisted()
    {
        return isTwisted;
    }
}
