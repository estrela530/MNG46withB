using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// プレイヤーの操作クラス
/// </summary>
[RequireComponent(typeof(FragmentPool))]      //自動的にFragmentPoolを追加
[RequireComponent(typeof(PredictionLinePool))]//自動的にPredictionLinePoolを追加
public class Player : MonoBehaviour
{
    #region プレイヤーのステータス
    [SerializeField, Header("移動速度")]
    private float moveSpeed = 8.0f;
    [SerializeField, Header("伸びる速さ")]
    private float extendSpeed = 0.06f;
    [SerializeField, Header("縮む速さ")]
    private float shrinkSpeed = 0.3f;
    [SerializeField, Header("伸びる長さ")]
    private float maxNobiLength = 5.0f;
    [SerializeField, Header("レベルアップに必要なねじカウント")]
    private int[] levelCount = new int[3];//初期値()
    [SerializeField, Tooltip("最大体力")]
    private float maxHp = 10;
    [SerializeField, Tooltip("体力の減少量")]
    private float decreaseHp = 0.01f;
    [SerializeField, Header("回復玉のレベルによる回復量")]
    private float[] healValue = new float[3];//初期値(0.2,0.5,1)
    [SerializeField, Tooltip("無敵時間")]
    private float invincibleTime = 2.0f;
    //[SerializeField, Tooltip("どれくらいねじれているか(値を入れないでね!!!)")]
    private int neziCount;//どれくらいねじれているか
    #endregion

    #region 欠片と回復玉
    [SerializeField, Tooltip("最初に生成しておくオブジェクトの数")]
    private int firstCreateFragment = 20;
    [SerializeField, Header("プレイヤーの欠片プレファブ")]
    private GameObject fragmentPrefab;
    [SerializeField, Header("予測線プレファブ")]
    private GameObject predictionLine;
    [SerializeField, Header("欠片の速度")]
    private float fragmentSpeed = 10;//後に距離計算で必要
    [SerializeField, Tooltip("レベルによる飛ばす球数")]
    private int[] fragmentCount = new int[3];//初期値(180,90,45)
    [SerializeField, Header("レベルによる欠片の飛距離(時間)")]
    private float[] deleteCount = new float[3];//初期値(0.5,1.5,10)
    #endregion

    #region サウンド関連
    [SerializeField, Header("ここから下音声---------------------------------------------------")]
    int iziranaide;
    private AudioSource audioSource;
    public AudioClip releaseSE;//解放した瞬間
    public AudioClip twistedSE;//ねじっているとき
    public AudioClip healSE;   //回復した瞬間
    public AudioClip cancelSE; //キャンセルした瞬間
    #endregion

    MeshRenderer meshRenderer;        //色変え用
    FragmentPool fragmentPool;        //かけらプール
    PredictionLinePool predictionPool;//予測線プール
    Vector3 myScale = Vector3.one;    //自身の大きさ

    private bool isTwisted; //ねじれているかどうか
    private bool isRelease; //解放中かどうか
    private bool isInhale;  //吸い込んでいるか
    private bool isNockBack;//ノックバックアニメーション中か
    public bool isDamage;   //ダメージを受けているかどうか(確認用にpublicにしてる)

    private int neziLevel;     //ねじレベル
    private int alphaCount = 0;//点滅用カウント

    private Vector3 position; //位置
    private Vector3 velocity; //移動量
    private Rigidbody rigid;  //物理演算
    private Animator animator;//アニメーション用

    public float currentHp;         //現在の体力(確認用にpublicにしてる)
    private float saveValue;        //体力一時保存用
    private float moveCount = 0.5f; //移動SEの鳴らす間隔
    private float alphaTimer = 0;   //点滅時間加算用
    private float directionAngle;   //キーの倒した角度
    private float previousAngle = 0;//前フレームのキーを倒した角度

    float[] preTrigger = new float[4];//LT,RT,Vertical,Horizontalトリガーの保存用キー
    float[] nowTrigger = new float[4];//LT,RT,Vertical,Horizontalトリガーの取得用キー

    public static int debugDamageCount = 0; //リザルト表示用ダメージ数
    public static int debugTwistedCount = 0;//リザルト標示用解放数

    /// <summary>
    /// リセットしたかどうか(メッシュ側で取得&代入を行う)
    /// </summary>
    public bool isReset { get; set; } = false;

    /// <summary>
    /// LT,RTトリガーの入力用
    /// </summary>
    private enum Keys
    {
        L_Trigger = 0,
        R_Trigger = 1,
        Vertical = 2,
        Horizontal = 3
    }
    Keys key;

    #region カクカクの回転の方向Enum
    ///// <summary>
    ///// プレイヤーの向いてる方向
    ///// </summary>
    //enum Direction
    //{
    //    UP,   //上
    //    DOWN, //下
    //    RIGHT,//右
    //    LEFT, //左

    //    TOP_RIGHT, //右上
    //    TOP_LEFT,  //左上
    //    DOWN_RIGHT,//右下
    //    DOWN_LEFT  //左下
    //}
    //Direction direction = Direction.DOWN;
    #endregion


    bool testBool = false;

    void Awake()
    {
        //プールの生成と、初期オブジェクトの追加
        fragmentPool = GetComponent<FragmentPool>();
        fragmentPool.CreatePool(fragmentPrefab, firstCreateFragment);

        //プールの生成と、初期オブジェクトの追加
        predictionPool = GetComponent<PredictionLinePool>();
        predictionPool.CreatePredictionLinePool(predictionLine, fragmentCount[2]);

        //コンポーネントの取得
        meshRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
        rigid = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        //体力の設定
        currentHp = saveValue = maxHp;

        //一番最初に初期化
        Initialize();
    }

    /// <summary>
    /// 値の初期化
    /// </summary>
    void Initialize()
    {
        isTwisted = false;
        isRelease = false;
        isNockBack = false;
        animator.enabled = true;
        neziCount = 0;
        neziLevel = 0;
        transform.localScale = Vector3.one;
        saveValue = currentHp;//赤ゲージを緑に合わせる。
    }

    /// <summary>
    /// 毎フレーム呼び出される
    /// 入力が必要な更新を行う。
    /// </summary>
    void Update()
    {
        nowTrigger[0] = Input.GetAxisRaw("L_Trigger");
        nowTrigger[1] = Input.GetAxisRaw("R_Trigger");
        nowTrigger[2] = Input.GetAxisRaw("Horizontal");
        nowTrigger[3] = Input.GetAxisRaw("Vertical");

        InputVelocity();//移動用のキー入力を行う
        TwistedChange();//ねじチェンジ      

        //入力されたキー情報から角度を計算する。
        directionAngle = Mathf.Atan2(nowTrigger[2], nowTrigger[3]) * Mathf.Rad2Deg;

        //-180 ～ +180なのを、0～360に変換する
        if (directionAngle < 0)
        {
            directionAngle += 360;
        }

        if (previousAngle != directionAngle)
        {
            isInhale = true;
        }
        else if (previousAngle == directionAngle)
        {
            isInhale = false;
        }

        //前フレームの角度をコピーする
        previousAngle = directionAngle;


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
        InvincibleTime(invincibleTime);//無敵時間
        MoveDirection();//移動
        //Debug.Log("ねじり状態：" + isTwisted);
    }

    /// <summary>
    /// キー入力で移動量得る
    /// </summary>
    private void InputVelocity()
    {
        velocity = Vector3.zero;

        velocity.x = Input.GetAxisRaw("Horizontal");
        velocity.z = Input.GetAxisRaw("Vertical");

        #region RigidBodyを使わなかった頃の移動
        //velocity = Vector3.zero;
        //Vector3 inputVelocity = Vector3.zero;
        //inputVelocity.x = Input.GetAxisRaw("Horizontal");
        //inputVelocity.z = Input.GetAxisRaw("Vertical");

        ////右
        //if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D) || inputVelocity.x > 0)
        //{
        //    velocity.x = 1.0f;
        //    direction = Direction.RIGHT;
        //}
        ////左
        //else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) || inputVelocity.x < 0)
        //{
        //    velocity.x = -1.0f;
        //    direction = Direction.LEFT;
        //}

        ////上
        //if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) || inputVelocity.z > 0)
        //{
        //    velocity.z = 1.0f;
        //    direction = Direction.UP;

        //    if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D) || inputVelocity.x > 0)
        //    {
        //        direction = Direction.TOP_RIGHT;
        //    }
        //    else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) || inputVelocity.x < 0)
        //    {
        //        direction = Direction.TOP_LEFT;
        //    }

        //}
        ////下
        //else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S) || inputVelocity.z < 0)
        //{
        //    velocity.z = -1.0f;
        //    direction = Direction.DOWN;

        //    if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D) || inputVelocity.x > 0)
        //    {
        //        direction = Direction.DOWN_RIGHT;
        //    }
        //    else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) || inputVelocity.x < 0)
        //    {
        //        direction = Direction.DOWN_LEFT;
        //    }
        //}

        ////正規化
        //velocity = velocity.normalized;
        //inputVelocity.Normalize();

        ////移動処理
        //position = velocity * moveSpeed * Time.deltaTime;

        ////transform.position += position;   
        #endregion
    }

    /// <summary>
    /// 移動量と向きを反映させる
    /// </summary>
    private void MoveDirection()
    {
        //移動していたら
        if (velocity != Vector3.zero)
        {
            #region カクカク回転角度の変化
            ////右向きにする
            //if (testVel.x > 0)
            //{
            //    direction = Direction.RIGHT;
            //}
            ////左向きにする
            //else if (testVel.x < 0)
            //{
            //    direction = Direction.LEFT;
            //}

            ////上向きにする
            //if (testVel.z > 0)
            //{
            //    direction = Direction.UP;

            //    //右上向きにする
            //    if (testVel.x > 0)
            //    {
            //        direction = Direction.TOP_RIGHT;
            //    }
            //    //左上向きにする
            //    else if (testVel.x < 0)
            //    {
            //        direction = Direction.TOP_LEFT;
            //    }
            //}
            ////下向きにする
            //else if (testVel.z < 0)
            //{
            //    direction = Direction.DOWN;

            //    //右下向きにする
            //    if (testVel.x > 0)
            //    {
            //        direction = Direction.DOWN_RIGHT;
            //    }
            //    //左下向きにする
            //    else if (testVel.x < 0)
            //    {
            //        direction = Direction.DOWN_LEFT;
            //    }
            //}
            #endregion

            ChangeDirection(); //向きの反映

            //ねじっているor解放中なら動けない
            if (isTwisted || isRelease || isNockBack) return;

            MoveSE(0.8f, 0.2f);//足音を鳴らす
            animator.SetFloat("Speed", velocity.magnitude);

            velocity.Normalize();
            rigid.velocity = velocity * moveSpeed;
        }
        else//移動していなかったら
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;

            animator.SetFloat("Speed", 0.0f);
        }
    }

    /// <summary>
    /// 移動中の足音SEを鳴らす
    /// </summary>
    /// <param name="interval">間隔</param>
    /// <param name="volume">音量</param>
    private void MoveSE(float interval, float volume)
    {
        //移動中音を鳴らす
        moveCount += Time.deltaTime;

        //一定時間ごとに音を鳴らす
        if (moveCount > interval)
        {
            audioSource.PlayOneShot(releaseSE, volume);
            moveCount = 0f;
        }
    }

    /// <summary>
    /// プレイヤーの向き替え
    /// </summary>
    private void ChangeDirection()
    {
        #region カクカクする回転
        //Quaternion dir = Quaternion.identity;
        ////現在の向きに回転させる
        //switch (direction)
        //{
        //    case Direction.UP:
        //        dir = Quaternion.Euler(0, 0, 0);
        //        break;
        //    case Direction.DOWN:
        //        dir = Quaternion.Euler(0, 180f, 0);
        //        break;
        //    case Direction.RIGHT:
        //        dir = Quaternion.Euler(0, 90f, 0);
        //        break;
        //    case Direction.LEFT:
        //        dir = Quaternion.Euler(0, 270f, 0);
        //        break;
        //    case Direction.TOP_RIGHT:
        //        dir = Quaternion.Euler(0, 45f, 0);
        //        break;
        //    case Direction.TOP_LEFT:
        //        dir = Quaternion.Euler(0, 315f, 0);
        //        break;
        //    case Direction.DOWN_RIGHT:
        //        dir = Quaternion.Euler(0, 135f, 0);
        //        break;
        //    case Direction.DOWN_LEFT:
        //        dir = Quaternion.Euler(0, 225f, 0);
        //        break;
        //    default:
        //        Debug.Log("存在しない向きに回転しようとしています。");
        //        break;
        //}
        //transform.rotation = dir;//回転角度を反映
        #endregion

        #region 滑らかな回転
        Vector3 test = new Vector3(velocity.x, 0, velocity.z) * Time.deltaTime / 2;
        transform.localRotation = Quaternion.LookRotation(test, Vector3.up);
        #endregion
    }

    /// <summary>
    /// ねじチェンジ
    /// キーの入力で処理を分岐させる。
    /// </summary>
    void TwistedChange()
    {
        //解放中なら処理しない
        if (isRelease || isNockBack) return;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("joystick button 5") || GetKeyDown(Keys.R_Trigger))
        {
            //ねじってる音を鳴らす
            audioSource.PlayOneShot(twistedSE, 1f);
            TwistedAccumulate();//ねじねじ
        }
        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp("joystick button 5") || GetKeyUP(Keys.R_Trigger))//離したら解放する
        {
            TwistedRelease();//解放
        }
    }

    /// <summary>
    /// ねじチャージ 
    /// ねじりフラグをTrueにする。
    /// </summary>
    void TwistedAccumulate()
    {
        animator.enabled = false;
        transform.localScale = Vector3.one;
        isTwisted = true;
        velocity = Vector3.zero;
        rigid.velocity = Vector3.zero;//ねじり中は移動量を無くす
    }

    /// <summary>
    /// ねじキャンセル
    /// Zキーを押したら初期化メソッドが呼ばれる。
    /// </summary>
    void TwistedCancel()
    {
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown("joystick button 4") || GetKeyDown(Keys.L_Trigger))
        {
            //リセットする前に一旦流れている音を止める
            audioSource.Stop();

            isReset = true;
            currentHp = saveValue;
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
        if (!isTwisted) return;//ねじり中じゃなかったら解放しない

        debugTwistedCount++;

        //解放する前に一旦流れている音を止める
        audioSource.Stop();

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
        #region 旧弾解放の処理
        //for (int angle = 0; angle < 360; angle += count)//360で割った個数分出てくる
        //{
        //    GameObject fragment = fragmentPool.GetObject();//生きているオブジェクトを代入

        //    if (fragment != null)
        //    {
        //        //この実装は、UpdateでGetComponentしているので良くない
        //        fragment.GetComponent<Fragment>().Initialize(angle, transform.position, deleteCount);
        //    }
        //}
        #endregion

        //周囲に弾を解放する
        for (int i = 0; i < bulletNum; i++)
        {
            #region 弾解放Tips
            //①現在の向き(軸)を取得し、値を正規化して0～1の値にする。
            //②360を球数で割って、角度(i)をかけることで、発射角度を計算することができる。
            //Quaternion.Eulerは引数にVector3を使用し、その軸を何度回転させるかという関数
            //→Quaternion.Euler(0,90,0) = Y軸を90度回転させる。
            //③角度に、軸をかけることで、軸を基準にした角度(Vector3型)がもとまる。
            //→(0,1,0)の軸に90°回転をかけると、Y軸が90°回転する(0,90,0)
            #endregion
            Vector3 axis = transform.forward;
            axis.Normalize();
            axis = Quaternion.Euler(0, (360 / bulletNum) * i, 0) * axis;

            GameObject fragment = fragmentPool.GetActiveObject();//生きているオブジェクトを代入
            if (fragment != null)
            {
                //この実装は、UpdateでGetComponentしているので良くない
                fragment.GetComponent<Fragment>().Initialize(axis, transform.position, deleteCount, fragmentSpeed);
            }
        }

        //解放した音を鳴らす
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
            TwistedCancel();//いつでもキャンセルできるように

            //最大までねじったらアニメーションをする。
            if (testBool)
            {
                //                                 はやさ            揺れ幅
                float sin = Mathf.Sin(2 * Mathf.PI * 5 * Time.time) * 0.05f;
                this.transform.localScale = new Vector3(sin + 1f, this.transform.localScale.y, sin + 1f);
            }

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

            //最大まで伸びきった時に1度だけ呼ばれる
            if (myScale.y > maxNobiLength)
            {
                myScale.y = maxNobiLength;           
                audioSource.Stop();       //音を止めたい
                testBool = true;
            }
        }
        else
        {
            //解放中でなければ処理をしない
            if (!isRelease) return;

            testBool = false;

            //体を縮めていく
            myScale += new Vector3(0, -shrinkSpeed, 0);

            //赤ゲージをなめらかに現在の体力値まで減らす。
            saveValue -= 0.1f;

            //大きさが一定以下になったら
            if (myScale.y <= 1.0f)
            {
                //解放終了とみなす
                myScale.y = 1.0f;
                isRelease = false;
                Initialize();//元の大きさに戻ったら初期化
            }
        }

        //大きさを反映
        transform.localScale = myScale;
    }

    /// <summary>
    /// カウントによるレベルの変化
    /// </summary>
    void ChangeLevel()
    {
        //↓常に向きを反映させる場合、この処理はいらなくなる
        //if (neziCount > levelCount[2]) return;//これ以上は処理しない

        //ねじカウントの値によるレベルの変化
        if (neziCount >= levelCount[0])
        {
            //memo : 現状なんか納得いかない感じの書き方になってしまっている。
            //レベルが上がった瞬間に1度だけ呼び出せるようにしたい。

            #region 予測線に1度だけ向きを反映させる処理
            //if (neziCount == levelCount[2])
            //{
            //    neziLevel = 3;
            //    InitPredictionLine(fragmentCount[2]);
            //}
            //else if (neziCount == levelCount[1])
            //{
            //    neziLevel = 2;
            //    InitPredictionLine(fragmentCount[1]);
            //}
            //else if (neziCount == levelCount[0])
            //{
            //    neziLevel = 1;
            //    InitPredictionLine(fragmentCount[0]);
            //}
            #endregion

            #region 予測線に常に向きを反映させる処理
            if (neziCount >= levelCount[2])
            {
                neziLevel = 3;
                InitPredictionLine(fragmentCount[2], deleteCount[2]);
            }
            else if (neziCount >= levelCount[1])
            {
                neziLevel = 2;
                InitPredictionLine(fragmentCount[1], deleteCount[1]);
            }
            else
            {
                neziLevel = 1;
                InitPredictionLine(fragmentCount[0], deleteCount[0]);
            }
            #endregion
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
    void InitPredictionLine(int count, float deleteCount)
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
                //速度(速さ)と生存時間(時間)から、予測線の長さ(距離)を計算する。
                float distance = fragmentSpeed * deleteCount;
                predictionLine.GetComponent<PredictionLine>().Initialize(angle, transform.position, distance);
            }
        }
    }

    /// <summary>
    /// 回復 
    /// </summary>
    /// <param name="healBall">オブジェクトのスクリプトを取得</param>
    private void Heal(GameObject healBall)
    {
        //当たったオブジェクトのレベルを取得
        int healAmounst = healBall.GetComponent<HealBall>().GetHealLevel();

        //回復の音
        audioSource.PlayOneShot(healSE);

        //受け取ったレベルによって回復量を変化させる。
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
            currentHp = maxHp;
        }
        //赤ゲージも最大以上にならない
        if (saveValue >= maxHp)
        {
            saveValue = maxHp;
        }
    }

    /// <summary>
    /// ダメージを受ける
    /// </summary>
    /// <param name="damage">ダメージ量</param>
    private void Damage(int damage, GameObject other)
    {
        if (isDamage) return;

        //音を止めたい
        audioSource.Stop();

        NockBack(other, 50);//ノックバックの移動をする
        animator.SetTrigger("Trigger");

        if (currentHp > 0)
        {
            currentHp -= damage;
            saveValue -= damage;

            //ダメージを受けたとき、ねじれ状態などを解除する。
            isReset = true;
            Initialize();

            debugDamageCount++;

            isDamage = true;
        }

        if (currentHp < 1.0f)
        {
            //死んだらシーン遷移
            //memo : ここも死んだらフラグをtrueにしてそれをどっかに伝えるようにした方がいい
            SceneManager.LoadScene("GameOver");
        }
    }


    /// <summary>
    /// 無敵時間
    /// </summary>
    /// <param name="time">何秒無敵にするか</param>
    private void InvincibleTime(float time)
    {
        if (!isDamage) return;

        //無敵時間計測開始
        alphaTimer += Time.deltaTime;
        if (alphaTimer < 1.0f)//ノックバックアニメションの時間
        {
            velocity = Vector3.zero;
            isNockBack = true;
        }
        else//ノックバックアニメーションが終了したら以下の処理に移る。
        {
            isNockBack = false;

            //memo : 点滅の処理をもっとわかりやすく直す
            alphaCount++;

            if (alphaCount > 0)
            {
                meshRenderer.material.color = Color.red;
            }
            if (alphaCount > 4)
            {
                meshRenderer.material.color = Color.white;
            }
            if (alphaCount > 8)
            {
                alphaCount = 0;
            }
        }

        //無敵時間の終了
        if (time < alphaTimer)
        {
            alphaTimer = 0;
            isDamage = false;
        }
    }

    /// <summary>
    /// 各ボールに触れたときの処理
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("HealBall"))
        {
            Heal(other.gameObject);   //回復
            Destroy(other.gameObject);//回復玉を消す
        }
        else if (other.gameObject.CompareTag("PoisonBall"))
        {
            Damage(1, other.gameObject);//ダメージ
            Destroy(other.gameObject);  //毒玉を消す
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            Damage(1, other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //memo : 持続ダメージの処理は仮
        if (other.gameObject.CompareTag("kanban"))
        {
            currentHp -= 0.01f;
            saveValue -= 0.01f;
            alphaCount++;

            if (alphaCount > 0)
            {
                meshRenderer.material.color = Color.red;
            }
            if (alphaCount > 4)
            {
                meshRenderer.material.color = Color.white;
            }
            if (alphaCount > 8)
            {
                alphaCount = 0;
            }
        }
    }

    /// <summary>
    /// ノックバック処理
    /// </summary>
    /// <param name="other">当たったオブジェクト</param>
    /// <param name="velocity">ノックバックの移動量</param>
    void NockBack(GameObject other, float velocity)
    {
        //当たった敵の角度を取得して
        //Wuaternionに変換しつつ正面ベクトル(0, 0 ,1)とかけて
        //その方向に移動させたらノックバック完成
        Vector3 angles = other.transform.localEulerAngles;
        Vector3 directions = Quaternion.Euler(angles) * Vector3.forward;

        transform.position += directions * velocity * Time.deltaTime;
    }

    ///// <summary>
    ///// 当たっている間の処理
    ///// </summary>
    ///// <param name="collision"></param>
    //private void OnCollisionStay(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Enemy"))
    //    {
    //        Damage(1, collision.gameObject);
    //    }
    //}

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
    /// 入力された瞬間
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    private bool GetAxisDown(Keys key)
    {
        if (preTrigger[(int)key] == 0)
        {
            if (nowTrigger[(int)key] > 0 || nowTrigger[(int)key] < 0)
            {
                return true;
            }
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
    /// ねじねじしているか
    /// </summary>
    /// <returns></returns>
    public bool GetTwisted()
    {
        return isTwisted;
    }

    /// <summary>
    /// 吸い込んでいるか
    /// </summary>
    /// <returns></returns>
    public bool GetInhale()
    {
        return isInhale;
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

    /// <summary>
    /// 現在のねじレベルを取得
    /// </summary>
    /// <returns></returns>
    public int GetNeziLevel()
    {
        return neziLevel;
    }
}
