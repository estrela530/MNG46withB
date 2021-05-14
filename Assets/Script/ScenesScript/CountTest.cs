using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountTest : MonoBehaviour
{
    List<GameObject> enemys = new List<GameObject>();

    public int allEnemyCount;
    public bool allDeathFlag;

    //GameObject door;

    // Start is called before the first frame update
    void Start()
    {
        allDeathFlag = false;
        //door = transform.GetChild(0).gameObject;
        //door.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //foreach (var item in enemys.ToArray())
        //{
        //    if (item.gameObject == null)
        //    {
        //        enemys.Remove(item);
        //    }
        //}

        for (int i = 0; i < enemys.Count; i++)
        {
            if (enemys[i].gameObject == null)
            {
                enemys.Remove(enemys[i]);
            }
        }

        if (enemys.Count <= 0)
        {
            allDeathFlag = true;
        }
        else
        {
            allDeathFlag = false;
        }

        Debug.Log(enemys.Count);
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            enemys.Add(other.gameObject);
        }
    }

}
