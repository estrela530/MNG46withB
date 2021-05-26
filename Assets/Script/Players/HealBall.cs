using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 回復玉クラス
/// </summary>
public class HealBall : InhaleObject
{
    [SerializeField, Header("壁にくっついた時のSE")]
    private AudioClip clingSE;
    [SerializeField, Header("レベルアップに必要な時間(成長2回、最大になってから消えるまでの時間)")]
    private float[] levelUpTime = new float[3];//(2,8,5
    [SerializeField, Tooltip("吸収時の移動速度")]
    private float[] mInhaleSpeed = new float[3];

    //GameObject findObject;    //プレイヤーを検索して保存する
    GameObject growEffect;    //成長エフェクト用オブジェクトを保存する。
    MeshRenderer meshRenderer;//色変え用
    Animator animator;        //アニメーション用

    Vector3 playerPos = Vector3.zero;//プレイヤーの現在位置

    int healLevel;       //回復レベル

    float mSpeed;        //移動速度保存用
    float levelCount;    //レベルアップ用カウント
    float deleteCount;   //消滅用カウント
    //ねじれ吸収の速度保存用
    float[] twiceSpeed = new float[3];

    private AudioSource audioSource;

    /// <summary>
    /// 成長状態
    /// </summary>
    private enum State
    {
        Level1,  //初期状態
        Level2,  //1回成長
        Level3,  //2回成長
        Blinking,//点滅
        Death    //死亡
    }
    State state = State.Level1;

    private void Awake()
    {
        //まず最初にプレイヤーを探す。
        findObject = GameObject.FindGameObjectWithTag("Player");
        player = findObject.GetComponent<Player>();
    }

    private void Start()
    {
        meshRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
        animator = GetComponent<Animator>();

        //生成時に音を鳴らす
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(clingSE,0.2f);

        //エフェクト用オブジェクトを取得
        growEffect = transform.GetChild(1).gameObject;
        growEffect.SetActive(false);//エフェクトを非表示
        //初期スケールの設定
        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        levelCount = 0.0f;
        deleteCount = 0.0f;
        healLevel = 1;//初期状態はレベル1

        //ねじれ吸収の速度は、通常の吸収速度の二倍とする。
        for (int i = 0; i < mInhaleSpeed.Length; i++)
        {
            twiceSpeed[i] = mInhaleSpeed[i] * 2;
        }
    }

    private void FixedUpdate()
    {
        if (player == null) meshRenderer.material.color = Color.gray;

        ChangeState(); //状態遷移
        ChangeAction();//行動変化
        base.Move();   //移動(親クラスの移動を使う)
    }

    /// <summary>
    /// レベルに応じて成長状態を遷移する
    /// </summary>
    void ChangeState()
    {
        //レベルが3以上になったら死へのカウントダウンを開始
        if (healLevel == 3)
        {
            //エフェクトを消す
            growEffect.SetActive(false);

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
        if (moveState == 2 || levelCount > levelUpTime[1]) return;

        //エフェクトを表示
        growEffect.SetActive(true);

        levelCount += Time.deltaTime; ;//値を増やし続ける～

        if (levelCount >= levelUpTime[0])
        {
            if (levelCount >= levelUpTime[1]) state = State.Level3;
            else state = State.Level2;
        }
        else state = State.Level1;
    }

    /// <summary>
    /// 成長状態を確認して、行動を変化させる
    /// </summary>
    void ChangeAction()
    {
        //成長状態に応じて、
        //回復レベル・色・スケール・吸収速度
        //を変化させる。
        //レベルが変化した時大きさが変更されるため、
        //アニメーションもその都度変更する。

        switch (state)
        {
            case State.Level1:
                //ねじれ吸収中なら2倍速、通常なら等速を速度に代入
                if (player.GetInhale()) mSpeed = twiceSpeed[0];
                else mSpeed = mInhaleSpeed[0];
                //回復レベル・色・スケール・吸収速度
                Actions(1, Color.yellow, new Vector3(0.5f, 0.5f, 0.5f), mSpeed);               
                break;
            case State.Level2:
                //ねじれ吸収中なら2倍速、通常なら等速
                if (player.GetInhale()) mSpeed = twiceSpeed[1];
                else mSpeed = mInhaleSpeed[1];
                //アニメーションの変更
                animator.SetInteger("HealLevel", 2);
                Actions(2, Color.red, new Vector3(1, 1, 1), mSpeed);
                break;
            case State.Level3:
                if (player.GetInhale()) mSpeed = twiceSpeed[2];
                else mSpeed = mInhaleSpeed[2];
                animator.SetInteger("HealLevel", 3);
                Actions(3, Color.white, new Vector3(1.5f, 1.5f, 1.5f), mSpeed);
                break;
            case State.Blinking:
                Blinking(10.0f);//点滅(点滅の速さ)
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
    void Actions(int level, Color color, Vector3 scale, float speed)
    {
        //値を反映指せる
        //速度(speed)は親クラスのものに代入
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
        meshRenderer.material.color = new Color(255, 1, 1, alpha);
    }

    /// <summary>
    /// このオブジェクトがDestroyされたときに呼ばれる。
    /// </summary>
    private void OnDestroy()
    {
        Renderer renderer = transform.GetChild(0).gameObject.GetComponent<Renderer>();
        DestroyImmediate(renderer.material);//マテリアルのメモリを削除
    }

    /// <summary>
    /// 回復レベルを取得
    /// </summary>
    /// <returns></returns>
    public int GetHealLevel()
    {
        //Playerで回復量を指定するために使用
        return healLevel;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Destroy(this.gameObject);
        }
    }
}
