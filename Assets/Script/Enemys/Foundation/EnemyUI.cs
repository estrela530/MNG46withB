using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    
    Slider slider;
    private GameObject HpUI;
    float currHp;

    EnemyMove enemy;


    // Start is called before the first frame update
    void Start()
    {
        slider = this.gameObject.GetComponent<Slider>();

        enemy = GameObject.Find("Enemy").GetComponent<EnemyMove>();
        //enemy = GameObject.Find("Boss Enemy").GetComponent<EnemyMove>();

        slider.maxValue = slider.value = enemy.HpGet();

        currHp = enemy.HpGet();
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = currHp;

        transform.rotation = Camera.main.transform.rotation;
        //Debug.Log(currHp);
        currHp = enemy.HpGet();

    }
}
