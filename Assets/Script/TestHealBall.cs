using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHealBall : MonoBehaviour
{
    int count = 0;
    Vector3 parentPos;
    int level = 1;

    // Start is called before the first frame update
    void Start()
    {
        count = 0;
        level = 0;
        //テスト↓ : 色変え
        GetComponent<Renderer>().material.color = Color.yellow;
    }

    public void Initialize(Vector3 position)
    {
        //位置初期化
        transform.position = position;
        parentPos = position;
    }

    // Update is called once per frame
    void Update()
    {
        count++;

        if(count >= 60)
        {
            if (count >= 180) level = 3;
            else level = 2;
        }
        else
        {
            level = 1;
        }

        switch (level)
        {
            case 1:
                //テスト↓ : 色変え
                GetComponent<Renderer>().material.color = Color.yellow;
                break;
            case 2:
                //テスト↓ : 色変え
                GetComponent<Renderer>().material.color = Color.green;
                break;
            case 3:
                //テスト↓ : 色変え
                GetComponent<Renderer>().material.color = Color.black;
                break;
            default:
                break;
        }
    }

    public int GetLevel()
    {
        return level;
    }
}
