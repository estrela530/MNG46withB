using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    
    Slider slider;
    private GameObject HpUI;
    int currHp;

    //[SerializeField] BossMove enemy;
    public GameObject Enemy;

    private EnemyMove Normal;
    private OctaneEnemy Octane;
    private OctaneNormal OctaneNormal;
    private OctaneWand OctaneWand;
    private KraberEnemy Kraber;
    private KraberBoss KraberBoss;
    private PoisonEnemy Poison;
    private PoisonBoss PoisonBoss;
    private BossMove Boss;
    private PawnEnemy Pawn;
    private ScorpionBoss Scorpion;
    private ToriteiEnemy Toritei;
    private ToriteiBoss ToriteiBoss;
    private KeyEnemy Key;


    // Start is called before the first frame update
    void Start()
    {
        slider = this.gameObject.GetComponent<Slider>();

        //Octane = GameObject.Find("OctaneEnemy").GetComponent<OctaneEnemy>();
        if (this.Enemy.GetComponent<ScorpionBoss>())
        {
            Scorpion = this.Enemy.GetComponent<ScorpionBoss>();
            slider.maxValue = slider.value = Scorpion.HpGet();
            currHp = (int)Scorpion.HpGet();
        }

        if (this.Enemy.GetComponent<ToriteiEnemy>())
        {
            Toritei = this.Enemy.GetComponent<ToriteiEnemy>();
            slider.maxValue = slider.value = Toritei.HpGet();
            currHp = (int)Toritei.HpGet();
        }

        if (this.Enemy.GetComponent<ToriteiBoss>())
        {
            ToriteiBoss = this.Enemy.GetComponent<ToriteiBoss>();
            slider.maxValue = slider.value = ToriteiBoss.HpGet();
            currHp = (int)ToriteiBoss.HpGet();
        }

        if (this.Enemy.GetComponent<OctaneNormal>())
        {
            OctaneNormal = this.Enemy.GetComponent<OctaneNormal>();
            slider.maxValue = slider.value = OctaneNormal.HpGet();
            currHp = (int)OctaneNormal.HpGet();
        }

        if (this.Enemy.GetComponent<OctaneWand>())
        {
            OctaneWand = this.Enemy.GetComponent<OctaneWand>();
            slider.maxValue = slider.value = OctaneWand.HpGet();
            currHp = (int)OctaneWand.HpGet();
        }

        if (this.Enemy.GetComponent<OctaneEnemy>())
        {
            Octane = this.Enemy.GetComponent<OctaneEnemy>();
            slider.maxValue = slider.value = Octane.HpGet();
            currHp = (int)Octane.HpGet();
        }

        if (this.Enemy.GetComponent<KraberEnemy>())
        {
            Kraber = this.Enemy.GetComponent<KraberEnemy>();
            slider.maxValue = slider.value = Kraber.HpGet();
            currHp = (int)Kraber.HpGet();
        }

        if (this.Enemy.GetComponent<EnemyMove>())
        {
            Normal = this.Enemy.GetComponent<EnemyMove>();
            slider.maxValue = slider.value = Normal.HpGet();
            currHp = (int)Normal.HpGet();
        }

        if (this.Enemy.GetComponent<PoisonEnemy>())
        {
            Poison = this.Enemy.GetComponent<PoisonEnemy>();
            slider.maxValue = slider.value = Poison.HpGet();
            currHp = (int)Poison.HpGet();
        }

        if (this.Enemy.GetComponent<BossMove>())
        {
            Boss = this.Enemy.GetComponent<BossMove>();
            slider.maxValue = slider.value = Boss.HpGet();
            currHp = (int)Boss.HpGet();
        }

        if (this.Enemy.GetComponent<PawnEnemy>())
        {
            Pawn = this.Enemy.GetComponent<PawnEnemy>();
            slider.maxValue = slider.value = Pawn.HpGet();
            currHp = (int)Pawn.HpGet();
        }

        if (this.Enemy.GetComponent<KeyEnemy>())
        {
            Key = this.Enemy.GetComponent<KeyEnemy>();
            slider.maxValue = slider.value = Key.HpGet();
            currHp = (int)Key.HpGet();
        }

        if (this.Enemy.GetComponent<KraberBoss>())
        {
            KraberBoss = this.Enemy.GetComponent<KraberBoss>();
            slider.maxValue = slider.value = KraberBoss.HpGet();
            currHp = (int)KraberBoss.HpGet();
        }

        if (this.Enemy.GetComponent<PoisonBoss>())
        {
            PoisonBoss = this.Enemy.GetComponent<PoisonBoss>();
            slider.maxValue = slider.value = PoisonBoss.HpGet();
            currHp = (int)PoisonBoss.HpGet();
        }

        //enemy = GameObject.Find("Enemy").GetComponent<BossMove>();

        //slider.maxValue = slider.value = enemy.HpGet();

        //slider.maxValue = slider.value = Octane.GetComponent<OctaneEnemy>().HpGet();

        //currHp = Enemy.HpGet();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        slider.value = currHp;

        if (Enemy.GetComponent<OctaneWand>())
        {
            slider.value = OctaneWand.HpGet();
            currHp = (int)OctaneWand.HpGet();
        }
        else if (Enemy.GetComponent<OctaneEnemy>())
        {
            slider.value = Octane.HpGet();
            currHp = (int)Octane.HpGet();
        }
        else if (Enemy.GetComponent<OctaneNormal>())
        {
            slider.value = OctaneNormal.HpGet();
            currHp = (int)OctaneNormal.HpGet();
        }
        else if(Enemy.GetComponent<KraberEnemy>())
        {
            slider.value = Kraber.HpGet();
            currHp = (int)Kraber.HpGet();
        }

        else if (Enemy.GetComponent<EnemyMove>())
        {
            slider.value = Normal.HpGet();
            currHp = (int)Normal.HpGet();
        }

        else if (Enemy.GetComponent<PoisonEnemy>())
        {
            slider.value = Poison.HpGet();
            currHp = (int)Poison.HpGet();
        }

        else if (Enemy.GetComponent<BossMove>())
        {
            slider.value = Boss.HpGet();
            currHp = (int)Boss.HpGet();
        }

        else if (Enemy.GetComponent<PawnEnemy>())
        {
            slider.value = Pawn.HpGet();
            currHp = (int)Pawn.HpGet();
        }

        else if (Enemy.GetComponent<ScorpionBoss>())
        {
            slider.value = Scorpion.HpGet();
            currHp = (int)Scorpion.HpGet();
        }

        else if (Enemy.GetComponent<ToriteiEnemy>())
        {
            slider.value = Toritei.HpGet();
            currHp = (int)Toritei.HpGet();
        }

        else if (Enemy.GetComponent<ToriteiBoss>())
        {
            slider.value = ToriteiBoss.HpGet();
            currHp = (int)ToriteiBoss.HpGet();
        }

        else if (Enemy.GetComponent<KeyEnemy>())
        {
            slider.value = Key.HpGet();
            currHp = (int)Key.HpGet();
        }
        else if (Enemy.GetComponent<PoisonBoss>())
        {
            slider.value = PoisonBoss.HpGet();
            currHp = (int)PoisonBoss.HpGet();
        }
        transform.rotation = Camera.main.transform.rotation;
        //Debug.Log(currHp);

        //currHp = enemy.HpGet();

    }
}
