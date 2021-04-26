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
    private KraberEnemy Kraber;
    private PoisonEnemy Poison;
    private BossMove Boss;

    // Start is called before the first frame update
    void Start()
    {
        slider = this.gameObject.GetComponent<Slider>();
        
        //Octane = GameObject.Find("OctaneEnemy").GetComponent<OctaneEnemy>();

        if(this.Enemy.GetComponent<OctaneEnemy>())
        {
            Octane = this.Enemy.GetComponent<OctaneEnemy>();
            slider.maxValue = slider.value = Octane.HpGet();
            currHp = (int)Octane.HpGet();
        }
       
        if(this.Enemy.GetComponent<KraberEnemy>())
        {
            Kraber = this.Enemy.GetComponent<KraberEnemy>();
            slider.maxValue = slider.value = Kraber.HpGet();
            currHp = (int)Kraber.HpGet();
        }

        if (this.Enemy.GetComponent<EnemyMove>())
        {
            Normal = this.Enemy.GetComponent<EnemyMove>();
            slider.maxValue = slider.value = Normal.HpGet();
            currHp = (int)Kraber.HpGet();
        }

        if (this.Enemy.GetComponent<PoisonEnemy>())
        {
            Poison = this.Enemy.GetComponent<PoisonEnemy>();
            slider.maxValue = slider.value = Poison.HpGet();
            currHp = (int)Poison.HpGet();
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
        if(Enemy.GetComponent<OctaneEnemy>())
        {
            slider.value = Octane.HpGet();
            currHp = (int)Octane.HpGet();
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


        transform.rotation = Camera.main.transform.rotation;
        //Debug.Log(currHp);

        //currHp = enemy.HpGet();

    }
}
