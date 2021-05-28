using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    bool flag;
    bool flagB;

    //[SerializeField] BossMove enemy;
    [Header("アタッチしたエネミー")]
    public GameObject Enemy;

    private EnemyMove Normal;
    private OctaneEnemy Octane;
    private OctaneWand OctaneWand;
    private KraberEnemy Kraber;
    private PoisonEnemy Poison;
    private BossMove Boss;
    private PawnEnemy Pawn;
    private ScorpionBoss Scorpion;
    // Start is called before the first frame update
    void Start()
    {
        //スコーピオンボス
        if (this.Enemy.GetComponent<ScorpionBoss>())
        {
            Scorpion = this.Enemy.GetComponent<ScorpionBoss>();
            flagB =Scorpion.DamageGet();
        }

        //オクタン
        if (this.Enemy.GetComponent<OctaneEnemy>())
        {
            Octane = this.Enemy.GetComponent<OctaneEnemy>();
        }

        //
        if (this.Enemy.GetComponent<KraberEnemy>())
        {
            Kraber = this.Enemy.GetComponent<KraberEnemy>();
        }

        //ノーマル
        if (this.Enemy.GetComponent<EnemyMove>())
        {
            Normal = this.Enemy.GetComponent<EnemyMove>();
            flag = Normal.DamageGet();
        }

        //ポイズン
        if (this.Enemy.GetComponent<PoisonEnemy>())
        {
            Poison = this.Enemy.GetComponent<PoisonEnemy>();
            
        }

        //ボス
        if (this.Enemy.GetComponent<BossMove>())
        {
            Boss = this.Enemy.GetComponent<BossMove>();
        }

        //ザコ
        if (this.Enemy.GetComponent<PawnEnemy>())
        {
            Pawn = this.Enemy.GetComponent<PawnEnemy>();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       
        //オクタン
        if (this.Enemy.GetComponent<OctaneEnemy>())
        {
            
        }

        //クレーバー
        else if (this.Enemy.GetComponent<KraberEnemy>())
        {
           
        }
        //ノーマル
        else if (this.Enemy.GetComponent<ScorpionBoss>())
        {
            flagB = Scorpion.DamageGet();
        }
        //ノーマル
        else if (this.Enemy.GetComponent<EnemyMove>())
        {
            flag = Normal.DamageGet();
        }

        //ポイズン
        else if (this.Enemy.GetComponent<PoisonEnemy>())
        {
           
        }

        //ボス
        else if (this.Enemy.GetComponent<BossMove>())
        {
           
        }

        //ザコ
        else if (this.Enemy.GetComponent<PawnEnemy>())
        {
            
        }

        if (flag)
        {
            Eff();
        }

        if (flagB)
        {
            Eff2();
        }

    }

    void Eff()
    {
        float level = Mathf.Abs(Mathf.Sin(Time.time * 0.5f));
        gameObject.GetComponent<Renderer>().material.color = new Color(125f, 125f, 255f, level);
        StartCoroutine("WaitForIt");
    }

    void Eff2()
    {
        float level = Mathf.Abs(Mathf.Sin(Time.time * 3f));
        gameObject.GetComponent<Renderer>().material.color = new Color(125f, 125f, 255f, level);
        StartCoroutine("WaitForIt");
    }

    IEnumerator WaitForIt()
    {
        // 1秒間処理を止める
        yield return new WaitForSeconds(0);

        // １秒後ダメージフラグをfalseにして点滅を戻す
        flag = false;
        flagB = false;
        gameObject.GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f, 1f);
    }
}
