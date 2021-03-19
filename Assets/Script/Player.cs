using UnityEngine;
using UnityEngine.SceneManagement;

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

    [SerializeField, Header("プレイヤーの欠片プレファブ")]
    private GameObject fragmentPrefab;
    [SerializeField, Tooltip("レベルによる飛ばす球数")]
    private int[] fragmentCount = new int[4];//(180,90,45

    //[SerializeField]
    //private Slider redSlider;
    //[SerializeField]
    //private Slider greenSlider;
    //[SerializeField]
    //private int maxHp = 10;
    //private float saveValue;

    [SerializeField]
    private int firstCreateFragment = 20;
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


    private Rigidbody rigid;

    public float maxHp = 10;
    public float currentHp = 10; //現在の体力
    private float saveValue = 10;//最大体力を入れておかないと

    public float decreaseHp = 0.01f;//体力の減少量

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
    }
    Direction direction = Direction.DOWN;

    void Start()
    {
        //ねじれてる本体の色情報を取得
        renderer = transform.GetChild(0).GetComponent<Renderer>();

        //プールの生成と、初期オブジェクトの追加
        objectPool = GetComponent<FragmentPool>();
        objectPool.CreatePool(fragmentPrefab, firstCreateFragment);

        //移動量を消すために必要だった
        rigid = GetComponent<Rigidbody>();

        Initialize();
    }

    void Initialize()
    {
        isTwisted = false;
        isRelease = false;
        neziCount = 0;
        neziLevel = 0;
        transform.localScale = Vector3.one;
        saveValue = currentHp;//赤ゲージを緑に合わせる。
    }

    // Update is called once per frame
    void Update()
    {
        Move();         //動く
        Extend();       //伸びる
        ChangeLevel();  //レベル変更
        TwistedChange();//ねじチェンジ

        if(currentHp < 0.5f)
        {
            SceneManager.LoadScene("Result");
        }
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
        //else
        //{
        //    //動いてないとき
        //    rigid.constraints = RigidbodyConstraints.FreezePosition;

        //}

        //正規化
        velocity = velocity.normalized;

        //移動処理
        position = velocity * moveSpeed * Time.deltaTime;

        transform.position += position;

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
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
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

            //体力を滑らかに減らす
            currentHp -= decreaseHp;
            if(currentHp <= 1)
            {
                //ねじりすぎて死なないようにする。
                currentHp = 1.0f;
            }

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

            //赤ゲージをなめらかに現在の体力値まで減らす。
            saveValue -= 0.1f;

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
        rigid.constraints = RigidbodyConstraints.FreezePosition;//移動を固定
        rigid.constraints = RigidbodyConstraints.FreezeRotation;//移動を固定
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
            currentHp = saveValue;
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

        //ねじレベルによる色と球数の変化
        switch (neziLevel)
        {
            case 1:
                TestNeziShoot(fragmentCount[1]);
                break;
            case 2:
                TestNeziShoot(fragmentCount[2]);
                break;
            case 3:
                TestNeziShoot(fragmentCount[3]);
                break;
            default:
                Debug.Log("存在しないレベルで解放しようとしています。");
                break;
        }
    }

    /// <summary>
    /// 欠片を飛ばす(向きを指定できるようにしないといけない)
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
                //この実装は、UpdateでGetComponentしているので良くない
                fragment.GetComponent<Fragment>().Initialize(angle, transform.position);
            }
        }
    }

    /// <summary>
    /// 回復
    /// </summary>
    /// <param name="healBall">オブジェクトのスクリプトを取得</param>
    private void Heal(GameObject healBall)
    {
        int healAmounst = healBall.GetComponent<TestHealBall>().GetLevel();

        switch (healAmounst)
        {
            case 1:
                currentHp += 0.2f;
                saveValue += 0.2f;
                break;
            case 2:
                currentHp += 0.5f;
                saveValue += 0.5f;
                break;
            case 3:
                currentHp += 1.0f;
                saveValue += 1.0f;
                break;
            default:
                Debug.Log("存在しないレベルでの回復が行われようとしています");
                break;
        }

        //最大体力以上にはならない。
        if(currentHp >= maxHp)
        {
            currentHp = maxHp;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("HealBall"))
        {
            Heal(other.gameObject);   //回復
            Destroy(other.gameObject);//回復玉を消す
        }
        if(other.gameObject.CompareTag("Enemy"))
        {
            currentHp -= 1.0f;//ダメージを受ける(あとで関数つくる)
            saveValue -= 1.0f;//ダメージを受ける(あとで関数つくる)
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            //移動量を0にする。
            rigid.velocity = Vector3.zero;
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

    /// <summary>
    /// 現在の体力を取得
    /// </summary>
    /// <returns></returns>
    public float GetHp()
    {
        return currentHp;
    }

    /// <summary>
    /// 保存した値を取得
    /// </summary>
    /// <returns></returns>
    public float GetSavevalue()
    {
        return saveValue;
    }
}
