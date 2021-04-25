using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestChildInhale : TestParentInhale
{
    [SerializeField, Header("レベルアップに必要な時間(成長2回、最大になってから消えるまでの時間)")]
    private float[] levelUpTime = new float[3];//(2,8,5
    [SerializeField, Tooltip("吸収時の移動速度")]
    private float[] mInhaleSpeed = new float[3];
    
    MeshRenderer meshRenderer;//色変え用

    float levelCount;  //レベルアップ用カウント
    float deleteCount; //消滅用カウント
    float testSpeed;
    float[] twiceSpeed = new float[3];

    int healLevel;  //回復レベル

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
    }
    State state = State.Level1;

    private void Start()
    {
        levelCount = 0;
        deleteCount = 0;
        healLevel = 1;

        //二倍の値を代入
        for (int i = 0; i < mInhaleSpeed.Length; i++)
        {
            twiceSpeed[i] = mInhaleSpeed[i] * 2;
        }

        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        meshRenderer = GetComponent<MeshRenderer>();

        player = GameObject.Find("SlimePlayer").GetComponent<Player>();
    }

    private void FixedUpdate()
    {
        ChangeState();//状態カウント
        SetAction();  //行動変化
        base.Move();       //移動

        //Debug.Log("私は子ども" + base.inhaleRange[2]);
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
        if (moveState == 2 || levelCount > levelUpTime[1]) return;

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
                if (player.GetInhale()) testSpeed = twiceSpeed[0];
                else testSpeed = mInhaleSpeed[0];

                Actions(1, Color.yellow, new Vector3(0.5f, 0.5f, 0.5f), testSpeed);
                break;
            case State.Level2:
                //吸い込み中かどうかを調べる
                if (player.GetInhale()) testSpeed = twiceSpeed[1];
                else testSpeed = mInhaleSpeed[1];

                Actions(2, Color.green, new Vector3(1, 1, 1), testSpeed);
                break;
            case State.Level3:
                //吸い込み中かどうかを調べる
                if (player.GetInhale()) testSpeed = twiceSpeed[2];
                else testSpeed = mInhaleSpeed[2];

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
    void Actions(int level, Color color, Vector3 scale, float mSpeed)
    {
        //値を反映指せる
        healLevel = level;
        meshRenderer.material.color = color;
        transform.localScale = scale;
        base.speed = mSpeed;
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
        Renderer renderer = gameObject.GetComponent<Renderer>();
        DestroyImmediate(renderer.material);//マテリアルのメモリを削除
    }

    /// <summary>
    /// 回復レベルを取得
    /// </summary>
    /// <returns></returns>
    public int GetHealLevel()
    {
        return healLevel;
    }
}
