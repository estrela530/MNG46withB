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

    Player player;            //シーン上のプレイヤーを取得
    TestManager manager;      //回復玉管理リストを取得
    MeshRenderer meshRenderer;//色変え用

    Vector3 hitPosition = Vector3.zero;//回復玉同士が当たった位置
    Vector3 playerPos = Vector3.zero;  //プレイヤーの現在位置

    bool hitFlag = false;  //回復玉同士が当たったか
    bool isTwisted = false;//プレイヤーがねじっているか
    bool moveFlag = false; //false = ターゲット取得：true = 移動中

    int levelCount; //レベルアップ用カウント
    int deleteCount;//消滅用カウント
    int healLevel;  //回復レベル

    private float speed;

    //ParticleSystem particleSystem;

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

    // Start is called before the first frame update
    void Start()
    {
        //60かけて秒にする。
        for(int i = 0;i<levelUpTime.Length;i++)
        {
            levelUpTime[i] *= 60.0f;
        }

        levelCount = 0;
        deleteCount = 0;
        healLevel = 0;

        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.color = Color.yellow;

        //manager = GameObject.Find("Manager").GetComponent<TestManager>();
        //manager.AddList(this);//リストに自分を追加

        //タグがPlayerのオブジェクトを取得
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        //particleSystem = transform.GetChild(0).GetComponent<ParticleSystem>();
        //particleSystem.gameObject.SetActive(false);
    }

    void FixedUpdate()
    {
        Move();       //移動
        ChangeState();//状態カウント
        SetAction();  //行動変化
    }

    //IEnumerator ParticleWorks()
    //{
    //    yield return new WaitWhile(() => particleSystem.IsAlive(true));
    //    particleSystem.gameObject.SetActive(false);
    //}

    /// <summary>
    /// プレイヤーがねじっているとき移動する。
    /// </summary>
    void Move()
    {
        //プレイヤーがねじっているかを取得
        isTwisted = player.GetTwisted();

        if (isTwisted)
        {
            if (moveFlag == false)
            {
                //ターゲットの位置を1度だけ取得
                playerPos = player.GetPosition();
                moveFlag = true;
            }
            else
            {
                //ターゲットへの向きを取得
                Vector3 direction = playerPos - this.transform.position;

                //正規化
                direction.Normalize();

                this.transform.position += direction * speed * Time.deltaTime;
            }
        }
        else moveFlag = false;
    }

    /// <summary>
    /// カウントに応じて状態を変える
    /// </summary>
    void ChangeState()
    {
        //レベルが3以上になったら死へのカウントダウンを開始
        if (healLevel == 3)
        {
            deleteCount++;

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
        if (isTwisted || levelCount > levelUpTime[1]) return;
        
        levelCount++;//値を増やし続ける～

        if (levelCount >= levelUpTime[0])
        {
            if (levelCount >= levelUpTime[1]) state = State.Level3;
            else state = State.Level2;
        }
        else state = State.Level1;

        //particleSystem.gameObject.SetActive(true);
    }

    /// <summary>
    /// 状態によって色：大きさ：レベル：点滅：削除を変更
    /// </summary>
    void SetAction()
    {
        switch (state)
        {
            case State.Level1:
                Actions(1, Color.yellow, new Vector3(0.5f, 0.5f, 0.5f), inhaleSpeed[0]);
                break;
            case State.Level2:
                Actions(2, Color.green, new Vector3(1, 1, 1), inhaleSpeed[1]);
                break;
            case State.Level3:
                Actions(3, Color.black, new Vector3(1.5f, 1.5f, 1.5f), inhaleSpeed[2]);
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
        healLevel = level;
        meshRenderer.material.color = color;
        transform.localScale = scale;
        this.speed = speed;
        //---------------旧処理(各レベルごとに書く)------------
        //healLevel = 1;
        //meshRenderer.material.color = Color.yellow;
        //transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        //speed = inhaleSpeed[0];
        //-----------------------------------------------------
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
        //System.GC.Collect();
        //Resources.UnloadUnusedAssets();
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
            //Debug.Log("回復同士がぶつかった");
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
