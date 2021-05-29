using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kanban : MonoBehaviour
{
    [SerializeField]
    //　ポーズした時に表示するUIのプレハブ
    private GameObject kanbanPrefab;

    public bool kanbanHit;

    // Start is called before the first frame update
    void Start()
    {
        kanbanHit = false;
        kanbanPrefab.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if (kanbanHit == true)
        {
            kanbanPrefab.SetActive(true);    
        }
        else if (kanbanHit == false)
        {
            kanbanPrefab.SetActive(false);
        }

    }

    //Playerが当たると説明表示
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            kanbanHit = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            kanbanHit = false;
        }

    }
}
