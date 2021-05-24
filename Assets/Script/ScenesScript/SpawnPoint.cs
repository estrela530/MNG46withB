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
    GameObject door;
    GameObject door2;
    CountTest countTest;
    bool allDeathFlag;

    
    // Start is called before the first frame update
    void Start()
    {
        isTouchPlayerFlag = false;
        //SpawnEnemys.SetActive(false);
        enemysCount = gameObject.transform.childCount;
        enemys = new GameObject[enemysCount];
        door = transform.GetChild(0).gameObject;
        door.SetActive(false);
        door2 = transform.GetChild(1).gameObject;
        door2.SetActive(false);

        for (int i = 0; i < enemysCount; i++)
        {
            enemys[i] = gameObject.transform.GetChild(i).gameObject;
            enemys[i].SetActive(false);

        }
        //enemys[2].SetActive(true);
        countTest = enemys[2].GetComponent<CountTest>();

        this.allDeathFlag = countTest.allDeathFlag;
        allDeathFlag = false;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isTouchPlayerFlag)
        {
            return;
        }
        this.allDeathFlag = countTest.allDeathFlag;
        //Debug.Log(allDeathFlag);
        if (allDeathFlag)
        {
            door.SetActive(false);
            door2.SetActive(false);
        }
        else 
        {
            door.SetActive(true);
            door2.SetActive(true);
        }
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
            enemys[2].SetActive(true);
            isTouchPlayerFlag = true;
            //Debug.Log("触ったな…");
        }

    }

}
