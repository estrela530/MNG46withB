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

    private EnemyMove Normal;//ノーマル

    private OctaneEnemy Octane;//ボスオクタン

    private OctaneNormal OctaneNormal;//ノーマルオクタン

    private OctaneWand OctaneWand;//徘徊オクタン

    private KraberEnemy Kraber;//クレーバー(亀)

    private ToriteiEnemy Toritei;//トリテ(サボテン)

    private PoisonEnemy Poison;//毒

    private BossMove Boss;//ノーマルボス

    private ScorpionBoss Scorpion;//スコーピオンボス

    private PawnEnemy Pawn;//ザコ(ひよこ)

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
            flag = Octane.DamageGet();
        }

        //ノーマルオクタン
        if (this.Enemy.GetComponent<OctaneNormal>())
        {
            OctaneNormal = this.Enemy.GetComponent<OctaneNormal>();
            flag = OctaneNormal.DamageGet();
        }

        //徘徊オクタン
        if (this.Enemy.GetComponent<OctaneWand>())
        {
            OctaneWand = this.Enemy.GetComponent<OctaneWand>();
            flag = OctaneWand.DamageGet();
        }

        //クレーバー
        if (this.Enemy.GetComponent<KraberEnemy>())
        {
            Kraber = this.Enemy.GetComponent<KraberEnemy>();
            flag = Kraber.DamageGet();
        }

        //トリテ
        if (this.Enemy.GetComponent<ToriteiEnemy>())
        {
            Toritei = this.Enemy.GetComponent<ToriteiEnemy>();
            flag = Toritei.DamageGet();
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
            flag = Poison.DamageGet();

        }

        //ボス
        if (this.Enemy.GetComponent<BossMove>())
        {
            Boss = this.Enemy.GetComponent<BossMove>();
            flag = Boss.DamageGet();
        }

        //ザコ(ひよこ)
        if (this.Enemy.GetComponent<PawnEnemy>())
        {
            Pawn = this.Enemy.GetComponent<PawnEnemy>();
            //flag = Pawn.DamageGet();
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
       
        //オクタン
        if (this.Enemy.GetComponent<OctaneEnemy>())
        {
            flag = Octane.DamageGet();
        }

        //ノーマルオクタン
        else if (this.Enemy.GetComponent<OctaneNormal>())
        {
            flag = OctaneNormal.DamageGet();
        }

        //徘徊オクタン
        else if (this.Enemy.GetComponent<OctaneWand>())
        {
            flag = OctaneWand.DamageGet();
        }

        //クレーバー
        else if (this.Enemy.GetComponent<KraberEnemy>())
        {
            flag = Kraber.DamageGet();
        }
        //スコーピオンボス
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
            flag = Poison.DamageGet();
        }

        //トリテ
        if (this.Enemy.GetComponent<ToriteiEnemy>())
        {
            flag = Toritei.DamageGet();
        }

        //ボス
        else if (this.Enemy.GetComponent<BossMove>())
        {
            flag = Boss.DamageGet();
        }

        //ザコ(ひよこ)
        else if (this.Enemy.GetComponent<PawnEnemy>())
        {
            //flag = Pawn.DamageGet();
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
