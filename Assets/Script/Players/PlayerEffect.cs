using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffect : MonoBehaviour
{
    GameObject[] effects;//子オブジェクト
    GameObject parent;   //一番上の親を取得
    Player player;       //プレイヤー

    int level;      //ねじレベル
    int childCount; //子オブジェクトの数
    bool[] isActive;//各子オブジェクトの表示状態

    Vector3 defaultScale;

    // Start is called before the first frame update
    void Start()
    {
        //一番上の親オブジェクトから、プレイヤーを取得
        parent = transform.parent.gameObject;  
        player = parent.GetComponent<Player>();

        //自身の子どもの数を取得
        childCount = gameObject.transform.childCount;
        effects = new GameObject[childCount];
        isActive = new bool[childCount];

        //各子どもオブジェクトを取得して代入
        for (int i = 0; i < childCount; i++)
        {
            effects[i] = gameObject.transform.GetChild(i).gameObject;
            effects[i].SetActive(false);
        }




        defaultScale = transform.lossyScale;
    }

    // Update is called once per frame
    void Update()
    {
        //親オブジェクトが回転しても、反映されないようにする。
        gameObject.transform.rotation = Quaternion.Euler(Vector3.forward);

        //プレイヤーのねじレべえるを取得
        level = player.GetNeziLevel();

        switch (level)
        {
            case 0:
                //ねじっていないとき、
                //エフェクトを非表示にする。
                for (int i = 0; i < childCount; i++)
                {
                    effects[i].SetActive(false);
                    isActive[i] = false;
                }
                break;
            case 1://レベル1
                Test(0);
                break;
            case 2://レベル2
                Test(1);
                break;
            case 3://レベル3
                Test(2);
                break;
            default:
                Debug.Log("存在しないねじレベルに達しています。");
                break;
        }
    }

    void Test(int count)
    {
        //各フラグがfalseの時、
        //エフェクトを一旦非表示にし、
        //その後指定されたエフェクトを表示する。
        //処理は一度しか実行されない。

        if (isActive[count]) return;

        for (int i = 0; i < effects.Length; i++)
        {
            effects[i].SetActive(false);
        }
        effects[count].SetActive(true);
        isActive[count] = true;
    }
}
