using UnityEngine;

/// <summary>
/// ねじねじをまとめたクラス
/// </summary>
[RequireComponent(typeof(FragmentPool))]//自動的にFragmentPoolを追加
public class Player : MonoBehaviour
{
    //↓テスト : ボタンの種類をインスペクタで変えられるようにする。
    //[SerializeField, Header("ボタンのバージョン切り替え")]
    //private bool bottunVersion = false;//(false = Version1 : true = Version2)

    [SerializeField, Header("移動速度")]
    private float moveSpeed = 5.0f;
    [SerializeField, Header("伸びる速さ")]
    private float extendSpeed = 0.02f;
    [SerializeField, Header("縮む速さ")]
    private float shrinkSpeed = 0.1f;
    [SerializeField, Header("伸びる長さ")]
    private float maxNobiLength = 5.0f;
    [SerializeField/*, ReadOnly*/]
    public int neziCount;//どれくらいねじねじしているか
    [SerializeField, Header("レベルアップに必要なねじカウント")]
    private int[] levelCount = new int[3];//(50,110,180

    [SerializeField, Header("プレイヤーの欠片")]
    private GameObject fragmentPrefab;
    [SerializeField, Header("レベルによる飛ばす球数")]
    private int[] fragmentCount = new int[4];//(180,90,45

    [SerializeField]
    private int firstCreateFragment = 45;
    /// <summary>
    /// リセットしたかどうか(メッシュ側で取得&代入を行う)
    /// </summary>
    public bool isReset { get; set; } = false;

    Renderer renderer;            //色変え用
    FragmentPool objectPool;      //オブジェクトプール
    Vector3 myScale = Vector3.one;//自身の大きさ

    private bool isTwisted;//ねじれているかどうか
    private bool isRelease;//解放中かどうか
    private int neziLevel; //ねじレベル

    private Vector3 position;//位置
    private Vector3 velocity;//移動量

    enum Direction
    {
        UP,   //上
        DOWN, //下
        RIGHT,//右
        LEFT, //左

        TOP_RIGHT, //右上
        TOP_LEFT,  //左上
        DOWN_RIGHT,//右下
        DOWN_LEFT  //左下
    }Direction direction = Direction.DOWN;

    void Start()
    {
        //ねじれてる本体の色情報を取得
        renderer = transform.GetChild(0).GetComponent<Renderer>();

        objectPool = GetComponent<FragmentPool>();
        //最初の生成を行う
        objectPool.CreatePool(fragmentPrefab, firstCreateFragment);

        position = this.transform.position;
        //初期位置
        position = Vector3.zero;
        //位置を反映
        this.transform.position = position;

        transform.localScale = Vector3.one;

        Initialize();
    }

    void Initialize()
    {
        isTwisted = false;
        isRelease = false;
        neziCount = 0;
        neziLevel = 0;
        transform.localScale = Vector3.one;
    }

    // Update is called once per frame
    void Update()
    {
        Move();         //動く
        Extend();       //伸びる
        ChangeLevel();  //レベル変更
        TwistedChange();//ねじチェンジ

        //Debug.Log("ねじレベル" + neziLevel);
    }

    /// <summary>
    /// プレイヤーの移動
    /// </summary>
    private void Move()
    {
        //ねじっているor解放中なら動けない
        if (isTwisted || isRelease) return;

        velocity = Vector3.zero;

        Vector3 inputVelocity = Vector3.zero;
        inputVelocity.x = Input.GetAxisRaw("Horizontal");
        inputVelocity.z = Input.GetAxisRaw("Vertical");

        //右
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D) || inputVelocity.x > 0)
        {
            velocity.x = 1.0f;
            direction = Direction.RIGHT;
        }
        //左
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) || inputVelocity.x < 0)
        {
            velocity.x = -1.0f;
            direction = Direction.LEFT;
        }
        //上
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) || inputVelocity.z > 0)
        {
            velocity.z = 1.0f;
            direction = Direction.UP;

            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D) || inputVelocity.x > 0)
            {
                direction = Direction.TOP_RIGHT;
            }
            else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) || inputVelocity.x < 0)
            {
                direction = Direction.TOP_LEFT;
            }

        }
        //下
        else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S) || inputVelocity.z < 0)
        {
            velocity.z = -1.0f;
            direction = Direction.DOWN;

            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D) || inputVelocity.x > 0)
            {
                direction = Direction.DOWN_RIGHT;
            }
            else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) || inputVelocity.x < 0)
            {
                direction = Direction.DOWN_LEFT;
            }
        }

        //正規化
        velocity.Normalize();

        //移動処理
        position += velocity * moveSpeed * Time.deltaTime;

        transform.position = position;

        ChangeDirection();
    }

    /// <summary>
    /// プレイヤーの向き替え
    /// </summary>
    private void ChangeDirection()
    {
        //移動しているとき、移動方向に回転させる
        if (velocity != Vector3.zero)
        {
            Quaternion dir = Quaternion.identity;

            switch (direction)
            {
                case Direction.UP:
                    dir = Quaternion.Euler(0, 0, 0);
                    break;
                case Direction.DOWN:
                    dir = Quaternion.Euler(0, 180f, 0);
                    break;
                case Direction.RIGHT:
                    dir = Quaternion.Euler(0, 90f, 0);
                    break;
                case Direction.LEFT:
                    dir = Quaternion.Euler(0, 270f, 0);
                    break;
                case Direction.TOP_RIGHT:
                    dir = Quaternion.Euler(0, 45f, 0);
                    break;
                case Direction.TOP_LEFT:
                    dir = Quaternion.Euler(0, 315f, 0);
                    break;
                case Direction.DOWN_RIGHT:
                    dir = Quaternion.Euler(0, 135f, 0);
                    break;
                case Direction.DOWN_LEFT:
                    dir = Quaternion.Euler(0, 225f, 0);
                    break;
                default:
                    break;
            }

            transform.rotation = dir;//回転角度を反映
        }
        else
        {
            //動いていないとき
        }
    }

    /// <summary>
    /// ねじチェンジ
    /// </summary>
    void TwistedChange()
    {
        //解放中なら処理しない
        if (isRelease) return;

        //ボタンを押している間ねじねじする
        if (Input.GetKey(KeyCode.Space) || Input.GetKey("joystick button 5"))
        {
            TwistedAccumulate();//ねじねじ

        }
        else if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp("joystick button 5"))//離したら解放する
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
                Initialize();    //元の大きさに戻ったら初期化
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

        TwistedCancel();
    }

    /// <summary>
    /// ねじキャンセル
    /// </summary>
    void TwistedCancel()
    {
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown("joystick button 4"))
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
        isRelease = true; //解放中にする
        isTwisted = false;//ねじっていない

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
