using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public bool isTouchPlayerFlag;
    //[SerializeField,Header("SpawnEnemys")]
    //private GameObject SpawnEnemys;
    GameObject[] enemys;
    int enemysCount;

    // Start is called before the first frame update
    void Start()
    {
        isTouchPlayerFlag = false;
        //SpawnEnemys.SetActive(false);
        enemysCount = gameObject.transform.childCount;
        enemys = new GameObject[enemysCount];
        Debug.Log(enemysCount);

        for (int i = 0; i < enemysCount; i++)
        {
            enemys[i] = gameObject.transform.GetChild(i).gameObject;
            enemys[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    }

    void OnTriggerEnter(Collider collision)
    {
        if (isTouchPlayerFlag)
        {
            return;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            //SpawnEnemys.SetActive(true);

            for (int i = 0; i < enemysCount; i++)
            {
                enemys[i].SetActive(true);
            }
            isTouchPlayerFlag = true;
            Debug.Log("触ったな…");
        }

    }

}
