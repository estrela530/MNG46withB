using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーオブジェクトを探すときに使用したテスト用クラス
/// </summary>
public class TestPlayerSertch : MonoBehaviour
{
    GameObject findObject;
    GameObject findObjects;

    Player player1;
    Player player2;
    Player player3;

    TestParentInhale test;

    MeshRenderer meshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        findObject = GameObject.FindGameObjectWithTag("Player");
        if (findObject.name == "SlimePlayer")
        {
            meshRenderer.material.color = Color.red;
        }

        player1 = findObject.GetComponent<Player>();
        if (player1.GetHp() > 0)
        {
            meshRenderer.material.color = Color.gray;
        }

        //findObject = GameObject.FindGameObjectWithTag("Finish");
        //if(findObject.name == "Cube")
        //{
        //    meshRenderer.material.color = Color.red;
        //}

        //test = findObject.GetComponent<TestParentInhale>();

        //meshRenderer.material.color = Color.green;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
