using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon : MonoBehaviour
{
    float ObjHp;

    [Header("アタッチしたエネミー")]
    public GameObject Enemy;

    private EnemyMove Normal;//ノーマル
    private OctaneEnemy Octane;//オクタン
    private KraberEnemy Kraber;//クレーバー
    private PoisonEnemy Poison;//ポイズン
    private BossMove Boss;//ボス
    private PawnEnemy Pawn;//ザコ

    [SerializeField, Header("召喚したエネミーの数")]
    int EnemyCount;//プレハブの出現数

    [SerializeField, Header("この数値以下になったら召喚する")]
    int SummonHP;
    
    [SerializeField, Header("召喚するオブジェクト")]
    GameObject SummonEnemy;

    [SerializeField, Header("次からの生成時間")]
    float ResetTime;

    [SerializeField, Header("生成までの時間")]
    float PawnTime;
    
    [SerializeField, Header("召喚するエネミーの上限")]
    int MaxEnemyCount;//プレハブの出現数

    // Start is called before the first frame update
    void Start()
    {
        //オクタン
        if (this.Enemy.GetComponent<OctaneEnemy>())
        {
            Octane = this.Enemy.GetComponent<OctaneEnemy>();
            ObjHp = Octane.HpGet();
        }

        //オクタン
        if (this.Enemy.GetComponent<KraberEnemy>())
        {
            Kraber = this.Enemy.GetComponent<KraberEnemy>();
            ObjHp = Kraber.HpGet();
        }

        //ノーマル
        if (this.Enemy.GetComponent<EnemyMove>())
        {
            Normal = this.Enemy.GetComponent<EnemyMove>();
            
            ObjHp = Normal.HpGet();
        }

        //ポイズン
        if (this.Enemy.GetComponent<PoisonEnemy>())
        {
            Poison = this.Enemy.GetComponent<PoisonEnemy>();

            ObjHp = Poison.HpGet();
        }

        //ボス
        if (this.Enemy.GetComponent<BossMove>())
        {
            Boss = this.Enemy.GetComponent<BossMove>();
            ObjHp = Boss.HpGet();
        }

        //ザコ
        if (this.Enemy.GetComponent<PawnEnemy>())
        {
            Pawn = this.Enemy.GetComponent<PawnEnemy>();
            ObjHp = Pawn.HpGet();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //召喚
        if (ObjHp <= SummonHP)
        {
            Debug.Log("召喚ー");
            Summons();
        }

        //オクタン
        if (this.Enemy.GetComponent<OctaneEnemy>())
        {
            ObjHp = Octane.HpGet();
        }

        //クレーバー
        else if (this.Enemy.GetComponent<KraberEnemy>())
        {
            ObjHp = Kraber.HpGet();
        }

        //ノーマル
        else if(this.Enemy.GetComponent<EnemyMove>())
        {
            
            ObjHp = Normal.HpGet();
        }

        //ポイズン
        else if (this.Enemy.GetComponent<PoisonEnemy>())
        {
            ObjHp = Poison.HpGet();
        }

        //ボス
        else if(this.Enemy.GetComponent<BossMove>())
        {
            ObjHp = Boss.HpGet();
        }

        //ザコ
        else if(this.Enemy.GetComponent<PawnEnemy>())
        {
            ObjHp = Pawn.HpGet();
        }
    }

     //召喚する処理
    void Summons()
    {
        //カウントの値まで生成
        if (EnemyCount < MaxEnemyCount)
        {
            PawnTime -= Time.deltaTime;
            if(PawnTime <= 0.0f)
            {
                PawnTime = ResetTime;//1秒沖に生成
                var sum = Instantiate(SummonEnemy,
                    new Vector3(transform.position.x,transform.position.y,transform.position.z+3),
                    Quaternion.identity);
                EnemyCount++;
            }
        }
    }
}
