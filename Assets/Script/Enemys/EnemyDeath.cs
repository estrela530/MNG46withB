using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class EnemyDeath : MonoBehaviour
{
    //public bool enemyDeath;
    //string Death = "Player";
   

    //public GameObject enemay;
    void Start()
    {
       
    }

    //すべてのカメラに写ってない時に呼ばれる関数
    ////画面外の時
    //private void OnBecameInvisible()
    //{
    //    //GameObject.Destroy(this.gameObject);
    //    gameObject.SetActive(false);//非表示
    //}
    ////画面内の時
    //private void OnBecameVisible()
    //{
    //    gameObject.SetActive(true);//表示
    //}

    // Update is called once per frame
    void Update()
    {
        
    }

    //(仮)指定されたtagに当たると消える
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(this.gameObject);
        }
    }
}
