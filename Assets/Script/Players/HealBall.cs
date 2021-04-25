using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 回復玉クラス
/// </summary>
public class HealBall : MonoBehaviour
{
    [SerializeField, Header("レベルアップに必要な時間(成長2回、最大になってから消えるまでの時間)")]
    private float[] levelUpTime = new float[3];//(2,8,5
    [SerializeField, Tooltip("吸収時の移動速度")]
    private float[] inhaleSpeed = new float[3];
    [SerializeField, Tooltip("吸収範囲")]
    private int[] inhaleRange = new int[3];

    Player player;            //シーン上のプレイヤーを取得
    TestManager manager;      //回復玉管理リストを取得
    MeshRenderer meshRenderer;//色変え用

    Vector3 hitPosition = Vector3.zero;//回復玉同士が当たった位置
    Vector3 playerPos = Vector3.zero;  //プレイヤーの現在位置

    bool hitFlag = false;  //回復玉同士が当たったか
    bool isTwisted = false;//プレイヤーがねじっているか
    
    float levelCount;  //レベルアップ用カウント
    float deleteCount; //消滅用カウント
    float speed;       //速度の値受け取り用
    float testSpeed;
    float moveDistance;//プレイヤーとの距離
    float[] twiceSpeed = new float[3];

    int healLevel;  //回復レベル
    int playerLevel;//プレイヤーのねじレベル取得
    int moveState = 0;

    /// <summary>
    /// 状態
    /// </summary>
    private enum State
    {
        Level1,  //初期状態
        Level2,  //1回成長
        Level3,  //2回成長
        Blinking,//点滅
        Death    //死亡
    }State state = State.Level1;


    private void Awake()
    {
        levelCount = 0;
        deleteCount = 0;
        //healLevel = 0;
        healLevel = 1;

        //二倍の値を代入
        for(int i = 0; i < inhaleSpeed.Length;i++)
        {
            twiceSpeed[i] = inhaleSpeed[i] * 2;
        }

        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        meshRenderer = GetComponent<MeshRenderer>();

        //タグがPlayerのオブジェクトを取得
        //FindWithTagでできるようにしろ！
        //player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        player = GameObject.Find("SlimePlayer").GetComponent<Player>();
    }

    private void Start()
    {
        //スタートだったらfindwithtagで行けるかもしれないね
    }

    private void FixedUpdate()
    {
        if (player == null) meshRenderer.material.color = Color.gray;

        ChangeState();//状態カウント
        SetAction();  //行動変化
        Move();       //移動
    }

    /// <summary>
    /// プレイヤーがねじっているとき移動する。
    /// </summary>
    void Move()
    {
        if (player == null) return;

        //プレイヤーがねじっているかを取得
        isTwisted = player.GetTwisted();

        if(isTwisted == true)
        {
            switch(moveState)
            {
                case 0:
                    //まずは位置を取得(1度だけ)
                    playerPos = player.GetPosition();
                    //ここでプレイヤーとの距離を計算
                    moveDistance = Vector3.Distance(playerPos, this.transform.position);
                    //計算が終わったら次に進む
                    moveState = 1;
                    break;
                case 1:
                    //プレイヤーのレベルを取得
                    playerLevel = player.GetNeziLevel();
                    //指定した範囲内にじぶんがいたら
                    if (playerLevel == 3 && moveDistance < inhaleRange[2])
                    {
                        moveState = 2;
                    }
                    else if (playerLevel == 2 && moveDistance < inhaleRange[1])
                    {
                        moveState = 2;
                    }
                    break;
                case 2:

                    //ターゲットへの向きを取得
                    Vector3 direction = playerPos - this.transform.position;
                    //正規化
                    direction.Normalize();
                    //移動
                    transform.position += direction * speed * Time.deltaTime;
                    break;
                default:
                    break;
            }
        }
        else
        {
            moveState = 0;
        }
    }

    /// <summary>
    /// カウントに応じて状態を変える
    /// </summary>
    void ChangeState()
    {
        //レベルが3以上になったら死へのカウントダウンを開始
        if (healLevel == 3)
        {
            deleteCount += Time.deltaTime;

            //タイムアップで消滅
            if (deleteCount >= levelUpTime[2])
            {
                state = State.Death;
            }
            //カウントダウンの半刻で体が消えかける
            else if (deleteCount >= levelUpTime[2] * 0.5f)
            {
                state = State.Blinking;
            }

        }

        //吸い込み中はレベルアップしないようにする。
        if (moveState ==2 || levelCount > levelUpTime[1]) return;
        
        levelCount += Time.deltaTime; ;//値を増やし続ける～

        if (levelCount >= levelUpTime[0])
        {
            if (levelCount >= levelUpTime[1]) state = State.Level3;
            else state = State.Level2;
        }
        else state = State.Level1;
    }

    /// <summary>
    /// 状態によって色：大きさ：レベル：点滅：削除を変更
    /// </summary>
    void SetAction()
    {
        switch (state)
        {
            case State.Level1:
                //吸い込み中かどうかを調べる
                if (player.GetInhale())testSpeed = twiceSpeed[0];
                else testSpeed = inhaleSpeed[0];    
                
                Actions(1, Color.yellow, new Vector3(0.5f, 0.5f, 0.5f), testSpeed);
                break;
            case State.Level2:
                //吸い込み中かどうかを調べる
                if (player.GetInhale()) testSpeed = twiceSpeed[1];
                else testSpeed = inhaleSpeed[1];

                Actions(2, Color.green, new Vector3(1, 1, 1), testSpeed);
                break;
            case State.Level3:
                //吸い込み中かどうかを調べる
                if (player.GetInhale()) testSpeed = twiceSpeed[2];
                else testSpeed = inhaleSpeed[2];

                Actions(3, Color.black, new Vector3(1.5f, 1.5f, 1.5f), testSpeed);
                break;
            case State.Blinking:
                Blinking(10.0f);//点滅
                break;
            case State.Death:
                Destroy(this.gameObject);//削除
                break;
            default:
                Debug.Log("存在しない状態に切り替わっています。");
                break;
        }
    }

    /// <summary>
    /// 各レベルごとの変化
    /// </summary>
    /// <param name="level">回復レベル</param>
    /// <param name="color">回復玉の色</param>
    /// <param name="scale">大きさ</param>
    /// <param name="speed">吸収速度</param>
    void Actions(int level,Color color,Vector3 scale,float speed)
    {
        //値を反映指せる
        healLevel = level;
        meshRenderer.material.color = color;
        transform.localScale = scale;
        this.speed = speed;
    }

    /// <summary>
    /// 点滅
    /// </summary>
    /// <param name="time">点滅の速さ</param>
    void Blinking(float time)
    {
        float alpha = Mathf.Sin(Time.time * time) / 2 + 0.5f;
        meshRenderer.material.color = new Color(1, 1, 1, alpha);
    }

    /// <summary>
    /// このオブジェクトがDestroyされたときに呼ばれる。
    /// </summary>
    private void OnDestroy()
    {
        //manager.RemoveList(this);

        Renderer renderer = gameObject.GetComponent<Renderer>();
        DestroyImmediate(renderer.material);//マテリアルのメモリを削除
    }

    public void OnTriggerEnter(Collider other)
    {
        if (isTwisted) return;

        if(other.gameObject.CompareTag("HealBall"))
        {
            hitPosition = transform.position;//当たった位置保存

            //ここで相手の位置を保存して、マネージャーに渡せばいいのでは？
            //Other.Positionをマネージャーに渡す

            hitFlag = true;
        }
    }

    /// <summary>
    /// 回復レベルを取得
    /// </summary>
    /// <returns></returns>
    public int GetHealLevel()
    {
        return healLevel;
    }

    public bool GetHitFlag()
    {
        return hitFlag;
    }

    public Vector3 GetHitPos()
    {
        return hitPosition;
    }
}
