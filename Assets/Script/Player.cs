using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// プレイヤーの操作クラス
/// </summary>
[RequireComponent(typeof(FragmentPool))]//自動的にFragmentPoolを追加
[RequireComponent(typeof(PredictionLinePool))]
public class Player : MonoBehaviour
{
    //↓テスト : ボタンの種類をインスペクタで変えられるようにする。
    //[SerializeField, Header("ボタンのバージョン切り替え")]
    //private bool bottunVersion = false;//(false = Version1 : true = Version2)

    [SerializeField, Header("移動速度")]
    private float moveSpeed = 8.0f;
    [SerializeField, Header("伸びる速さ")]
    private float extendSpeed = 0.06f;
    [SerializeField, Header("縮む速さ")]
    private float shrinkSpeed = 0.3f;
    [SerializeField, Header("伸びる長さ")]
    private float maxNobiLength = 5.0f;
    [SerializeField, Header("レベルアップに必要なねじカウント")]
    private int[] levelCount = new int[3];//(20,40,60
    [SerializeField, Tooltip("どれくらいねじれているか(値を入れないでね!!!)")]
    public int neziCount;

    [SerializeField, Tooltip("最初に生成しておくオブジェクトの数")]
    private int firstCreateFragment = 20;
    [SerializeField, Header("プレイヤーの欠片プレファブ")]
    private GameObject fragmentPrefab;
    [SerializeField, Tooltip("レベルによる飛ばす球数")]
    private int[] fragmentCount = new int[3];//(180,90,45
    [SerializeField, Header("レベルによる欠片の飛距離(時間)")]
    private float[] deleteCount = new float[3];//(0.5,1.5,10

    [SerializeField, Tooltip("最大体力")]
    private float maxHp = 10;
    [SerializeField, Tooltip("体力の減少量")]
    private float decreaseHp = 0.01f;//体力の減少量
    [SerializeField, Header("回復玉のレベルによる回復量")]
    private float[] healValue = new float[3];//(0.2,0.5,1

    [SerializeField, Header("ここから下音声------------------------------------------------------------------------------------------------------------")]
    private int iziranaide;
    private AudioSource audioSource;
    public AudioClip releaseSE;//解放した瞬間
    public AudioClip twistedSE; //ねじっているとき
    public AudioClip healSE;    //回復した瞬間
    public AudioClip cancelSE;    //キャンセルした瞬間

    /// <summary>
    /// リセットしたかどうか(メッシュ側で取得&代入を行う)
    /// </summary>
    public bool isReset { get; set; } = false;

    MeshRenderer meshRenderer;            //色変え用
    FragmentPool fragmentPool;      //オブジェクトプール
    PredictionLinePool predictionPool;
    Vector3 myScale = Vector3.one;//自身の大きさ

    private bool isTwisted;//ねじれているかどうか
    private bool isRelease;//解放中かどうか
    public bool isDamage; //ダメージを受けているかどうか
    private int neziLevel; //ねじレベル

    private Vector3 position;//位置
    private Vector3 velocity;//移動量
    private Rigidbody rigid; //物理演算

    public float currentHp; //現在の体力
    private float saveValue;//体力一時保存用
    private float moveCount = 0.5f;//移動SEの鳴らす間隔


    float[] preTrigger = new float[2];//LT,RTトリガーの保存用キー
    float[] nowTrigger = new float[2];//LT,RTトリガーの取得用キー

    
    



    public GameObject testPrefab;

    private enum Keys
    {
        L_Trigger = 0,
        R_Trigger = 1
    }
    Keys key;


    /// <summary>
    /// プレイヤーの向いてる方向
    /// </summary>
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
        meshRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
        //プールの生成と、初期オブジェクトの追加
        fragmentPool = GetComponent<FragmentPool>();
        fragmentPool.CreatePool(fragmentPrefab, firstCreateFragment);

        //プールの生成と、初期オブジェクトの追加
        predictionPool = GetComponent<PredictionLinePool>();
        predictionPool.CreatePredictionLinePool(testPrefab, fragmentCount[2]);

        //移動量を消すために必要だった
        rigid = GetComponent<Rigidbody>();

        //オーディオソースを取得
        audioSource = GetComponent<AudioSource>();

        currentHp = saveValue = maxHp;

        Initialize();
    }

    /// <summary>
    /// 値の初期化
    /// </summary>
    void Initialize()
    {
        isTwisted = false;
        isRelease = false;
        isDamage = false;
        neziCount = 0;
        neziLevel = 0;
        transform.localScale = Vector3.one;
        saveValue = currentHp;//赤ゲージを緑に合わせる。

        //音を止めたい
        audioSource.Stop();
    }

    /// <summary>
    /// 毎フレーム呼び出される
    /// 入力が必要な更新を行う。
    /// </summary>
    void Update()
    {
        nowTrigger[0] = Input.GetAxisRaw("L_Trigger");
        nowTrigger[1] = Input.GetAxisRaw("R_Trigger");

        Move();         //動く
        TwistedChange();//ねじチェンジ

        //入力を使う処理が終わってからキーをコピーしないと動かなかった
        for (int i = 0; i < nowTrigger.Length; i++)
        {
            preTrigger[i] = nowTrigger[i];
        }
    }

    /// <summary>
    /// 一定秒数ごとに呼ばれる(現在は0.02秒)
    /// 「Edit」→「ProjectSetting」→「Time」→「TimeManager」
    /// </summary>
    private void FixedUpdate()
    {
        //ここでは入力にラグが出てしまうため、入力以外の処理を行うといい。

        TwistedExtend();//伸びる
        ChangeLevel();  //レベル変更
        InvincibleTime(1);//無敵時間
    }

    /// <summary>
    /// プレイヤーの移動
    /// ねじり中and解放中は動けない
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
                    Debug.Log("存在しない向きに回転しようとしています。");
                    break;
            }

            //移動中音を鳴らす
            moveCount += Time.deltaTime;

            if (moveCount > 0.5f)
            {
                audioSource.PlayOneShot(releaseSE, 0.2f);
                moveCount = 0f;
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
    /// キーの入力で処理を分岐させる。
    /// </summary>
    void TwistedChange()
    {
        //解放中なら処理しない
        if (isRelease) return;

        //if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("joystick button 5") || GetKeyDown(Keys.R_Trigger))
        //{
        //    //ねじってる音を鳴らす
        //    audioSource.PlayOneShot(twistedSE, 1f);
        //}


        //ボタンを押している間ねじねじする
        if (Input.GetKey(KeyCode.Space) || Input.GetKey("joystick button 5") || GetKey(Keys.R_Trigger))
        {
            TwistedAccumulate();//ねじねじ

        }
        else if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp("joystick button 5") || GetKeyUP(Keys.R_Trigger))//離したら解放する
        {
            TwistedRelease();//解放
        }
    }

    /// <summary>
    /// 押してる間
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    private bool GetKey(Keys key)
    {
        if (nowTrigger[(int)key] > 0)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// 入力された瞬間
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    private bool GetKeyDown(Keys key)
    {
        if (preTrigger[(int)key] == 0 && nowTrigger[(int)key] > 0)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 離した瞬間
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    private bool GetKeyUP(Keys key)
    {
        if (preTrigger[(int)key] != 0 && nowTrigger[(int)key] == 0)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// ねじチャージ 
    /// ねじりフラグをTrueにする。
    /// </summary>
    void TwistedAccumulate()
    {
        ////ねじってる音を鳴らす
        //audioSource.PlayOneShot(twistedSE, 0.5f);

        isTwisted = true;
        rigid.constraints = RigidbodyConstraints.FreezePosition;//移動を固定
        rigid.constraints = RigidbodyConstraints.FreezeRotation;//移動を固定
        TwistedCancel();
    }

    /// <summary>
    /// ねじキャンセル
    /// Zキーを押したら初期化メソッドが呼ばれる。
    /// </summary>
    void TwistedCancel()
    {
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown("joystick button 4") || GetKeyDown(Keys.L_Trigger))
        {
            isReset = true;
            currentHp = saveValue;
            //isRelease = false;//これが無いとInitializeがよばれないバグが起きて、動けなくなる。
            Initialize();

            //キャンセルした音
            audioSource.PlayOneShot(cancelSE);
        }
    }

    /// <summary>
    /// ねじリリース
    /// レベルに応じて、弾発射のメソッドを呼び出す
    /// </summary>
    void TwistedRelease()
    {
        isRelease = true; //解放中にする
        isTwisted = false;//ねじっていない

        //ねじレベルによる色と球数の変化
        switch (neziLevel)
        {
            case 0:
                //何もしないよ
                break;
            case 1:
                InitFragment(fragmentCount[0], deleteCount[0]);
                break;
            case 2:
                InitFragment(fragmentCount[1], deleteCount[1]);
                break;
            case 3:
                InitFragment(fragmentCount[2], deleteCount[2]);
                break;
            default:
                Debug.Log("存在しないレベルで解放しようとしています。");
                break;
        }
    }

    /// <summary>
    /// 欠片を飛ばす
    /// </summary>
    /// <param name="bulletNum">球数</param>
    void InitFragment(int bulletNum, float deleteCount)
    {
        //-----------------------旧弾解放の処理--------------------------------------------------------
        ////ここで解放する
        //for (int angle = 0; angle < 360; angle += count)//360で割った個数分出てくる
        //{
        //    GameObject fragment = fragmentPool.GetObject();//生きているオブジェクトを代入

        //    if (fragment != null)
        //    {
        //        //この実装は、UpdateでGetComponentしているので良くない
        //        fragment.GetComponent<Fragment>().Initialize(angle, transform.position, deleteCount);
        //    }
        //}
        //---------------------------------------------------------------------------------------------

        
        for (int i = 0; i < bulletNum; i++)
        {
            //①現在の向き(軸)を取得し、値を正規化して0～1の値にする。
            //②360を球数で割って、角度(i)をかけることで、発射角度を計算することができる。
            //Quaternion.Eulerは引数にVector3を使用し、その軸を何度回転させるかという関数
            //→Quaternion.Euler(0,90,0) = Y軸を90度回転させる。
            //③角度に、軸をかけることで、軸を基準にした角度(Vector3型)がもとまる。
            //→(0,1,0)の軸に90°回転をかけると、Y軸が90°回転する(0,90,0)

            Vector3 axis = transform.forward;
            axis.Normalize();
            axis = Quaternion.Euler(0, (360 / bulletNum) * i, 0) * axis;

            GameObject fragment = fragmentPool.GetActiveObject();//生きているオブジェクトを代入
            if (fragment != null)
            {
                //この実装は、UpdateでGetComponentしているので良くない
                fragment.GetComponent<Fragment>().Initialize(axis, transform.position, deleteCount);
            }
        }

        //解放した音
        audioSource.PlayOneShot(releaseSE, 2.0f);
    }

    /// <summary>
    /// 伸びる&縮む
    /// 体力も減らす
    /// レベル用カウントを増やす
    /// </summary>
    void TwistedExtend()
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
            if (currentHp <= 1)
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
                Initialize();//元の大きさに戻ったら初期化
            }
        }

        transform.localScale = myScale;
    }

    /// <summary>
    /// カウントによるレベルの変化
    /// </summary>
    void ChangeLevel()
    {
        if (neziCount > levelCount[2]) return;//これ以上は処理しない

        //ねじカウントの値によるレベルの変化
        if (neziCount >= levelCount[0])
        {
            //memo : 現状なんか納得いかない感じの書き方になってしまっている。
            //レベルが上がった瞬間に1度だけ呼び出せるようにしたい。

            if (neziCount == levelCount[2])
            {
                neziLevel = 3;
                InitPredictionLine(fragmentCount[2]);
            }
            else if (neziCount == levelCount[1])
            {
                neziLevel = 2;
                InitPredictionLine(fragmentCount[1]);
            }
            else if (neziCount == levelCount[0])
            {
                neziLevel = 1;
                InitPredictionLine(fragmentCount[0]);
            }

            //--------旧レベルの変更処理----------
            //if (neziCount >= levelCount[2])
            //{
            //    neziLevel = 3;
            //}
            //else if (neziCount >= levelCount[1])
            //{
            //    neziLevel = 2;
            //}
            //else
            //{
            //    neziLevel = 1;
            //}
            //------------------------------------
        }
        else
        {
            neziLevel = 0;
            //予測線リストの全てのオブジェクトを非アクティブにする。
            predictionPool.ResetActive();
        }

        //ねじレベルによる色の変化
        //memo : これも1度呼ぶだけでいいんだよね
        switch (neziLevel)
        {
            case 0://レベル0
                meshRenderer.material.color = Color.white;
                break;
            case 1:
                meshRenderer.material.color = Color.magenta;
                break;
            case 2:
                meshRenderer.material.color = Color.yellow;
                break;
            case 3:
                meshRenderer.material.color = Color.red;
                break;
            default:
                Debug.Log("存在しないレベルで色変えが行われようとしています");
                break;
        }
    }

    /// <summary>
    /// 予測線の生成
    /// </summary>
    /// <param name="count">生成個数</param>
    void InitPredictionLine(int count)
    {
        //https://gamelab.hatenablog.com/entry/AimForPlayer

        //予測線リストの全てのオブジェクトを非アクティブにする。
        predictionPool.ResetActive();

        for (int i = 0; i < count; i++)
        {
            Vector3 angle = transform.forward;
            angle.Normalize();
            angle = Quaternion.Euler(0, (360 / count) * i, 0) * angle;

            GameObject predictionLine = predictionPool.GetActiveObject();//生きているオブジェクトを代入
            if (predictionLine != null)
            {
                predictionLine.GetComponent<PredictionLine>().Initialize(angle, transform.position);
            }
        }
    }

    /// <summary>
    /// 回復 
    /// </summary>
    /// <param name="healBall">オブジェクトのスクリプトを取得</param>
    private void Heal(GameObject healBall)
    {
        int healAmounst = healBall.GetComponent<HealBall>().GetHealLevel();

        //回復の音
        audioSource.PlayOneShot(healSE);

        switch (healAmounst)
        {
            case 1:
                currentHp += healValue[0];
                saveValue += healValue[0];
                break;
            case 2:
                currentHp += healValue[1];
                saveValue += healValue[1];
                break;
            case 3:
                currentHp += healValue[2];
                saveValue += healValue[2];
                break;
            default:
                Debug.Log("存在しないレベルでの回復が行われようとしています");
                break;
        }

        //最大体力以上にはならない。
        if (currentHp >= maxHp)
        {
            currentHp = saveValue = maxHp;
        }
    }

    /// <summary>
    /// ダメージを受ける
    /// </summary>
    /// <param name="damage">ダメージ量</param>
    private void Damage(int damage)
    {
        if (isDamage) return;

        if (currentHp > 0)
        {
            currentHp -= damage;
            saveValue -= damage;

            isDamage = true;
        }

        if (currentHp < 1.0f)
        {
            Debug.Log("死んだよ");
            SceneManager.LoadScene("GameOver");
        }
    }

    float count = 0;
    float alpha = 0;

    /// <summary>
    /// 無敵時間
    /// </summary>
    /// <param name="time">何秒無敵にするか</param>
    private void InvincibleTime(float time)
    {
        if (!isDamage) return;

        meshRenderer.material.color = Color.red;

        alpha = Mathf.Sin(Time.time) / 2 + 0.5f;

        count += Time.deltaTime;

        if (time < count)
        {
            count = 0;
            isDamage = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("HealBall"))
        {
            Heal(other.gameObject);   //回復
            Destroy(other.gameObject);//回復玉を消す
        }
        else if(other.gameObject.CompareTag("PoisonBall"))
        {
            Damage(1);                //ダメージ
            Destroy(other.gameObject);//毒玉を消す
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Damage(1);
        }
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

    /// <summary>
    /// 現在の位置を取得
    /// </summary>
    /// <returns></returns>
    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
