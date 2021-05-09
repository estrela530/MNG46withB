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

        for (int i = 0; i < enemysCount; i++)
        {
            enemys[i] = gameObject.transform.GetChild(i).gameObject;
            enemys[i].SetActive(false);


        }
        enemys[1].SetActive(true);
        countTest = enemys[1].GetComponent<CountTest>();

        this.allDeathFlag = countTest.allDeathFlag;
        allDeathFlag = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.allDeathFlag = countTest.allDeathFlag;
        Debug.Log(allDeathFlag);
        if (allDeathFlag)
        {
            door.SetActive(false);
        }
        else 
        {
            door.SetActive(true);
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
            isTouchPlayerFlag = true;
            Debug.Log("触ったな…");
        }

    }

}
