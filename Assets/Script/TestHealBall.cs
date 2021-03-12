using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHealBall : MonoBehaviour
{
    int test = 0;
    int level = 0;

    // Start is called before the first frame update
    void Start()
    {
        //テスト↓ : 色変え
        GetComponent<Renderer>().material.color = Color.yellow;
    }

    // Update is called once per frame
    void Update()
    {
        test++;

        if(test > 1)
        {
            test = 180;
            level = 50;
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.gameObject.CompareTag("Player"))
    //    {
    //        Destroy(this.gameObject);
    //    }
    //}

    public int GetLevel()
    {
        return level;
    }
}
