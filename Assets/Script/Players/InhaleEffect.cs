using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ねじっているときの吸収のパーティクルを管理
/// </summary>
public class InhaleEffect : MonoBehaviour
{
    GameObject[] effects;//子オブジェクト
    GameObject parent;   //一番上の親を取得
    Player player;       //プレイヤー

    int level;      //ねじレベル
    int childCount; //子オブジェクトの数
    bool[] isActive;//各子オブジェクトの表示状態

    Vector3 lossyScale;

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

        lossyScale = transform.lossyScale;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ChangeLevel();
    }

    void ChangeLevel()
    {
        //親オブジェクトが回転しても、反映されないようにする。
        gameObject.transform.rotation = Quaternion.Euler(Vector3.forward);

        //transform.localScale = lossyScale;

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
                ChangeEffect(0);
                break;
            case 2://レベル2
                ChangeEffect(1);
                break;
            case 3://レベル3
                ChangeEffect(2);
                break;
            default:
                Debug.Log("存在しないねじレベルに達しています。");
                break;
        }
    }

    /// <summary>
    /// 表示エフェクトの変更
    /// </summary>
    /// <param name="count">表示するエフェクト番号</param>
    void ChangeEffect(int count)
    {
        //各フラグがfalseの時、
        //全てのエフェクトを一旦非表示にし、
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
