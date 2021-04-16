using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    
    Slider slider;
    private GameObject HpUI;
    float currHp;

    [SerializeField] BossMove enemy;


    // Start is called before the first frame update
    void Start()
    {
        slider = this.gameObject.GetComponent<Slider>();

        //enemy = GameObject.Find("Enemy").GetComponent<BossMove>();
        enemy.GetComponent<EnemyMove>();

        slider.maxValue = slider.value = enemy.HpGet();

        currHp = enemy.HpGet();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        slider.value = currHp;

        //transform.rotation = Camera.main.transform.rotation;
        //Debug.Log(currHp);
        currHp = enemy.HpGet();

    }
}
