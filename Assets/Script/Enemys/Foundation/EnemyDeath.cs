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
