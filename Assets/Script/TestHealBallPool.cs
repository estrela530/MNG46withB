using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 生成した回復玉を溜めるリストを管理する。
/// </summary>
public class TestHealBallPool : MonoBehaviour
{
    private List<GameObject> poolHealBallList;//オブジェクトを入れておくリスト
    private GameObject poolFragment;          //リストに入れるオブジェクト

    private static TestHealBallPool instance;
    private TestHealBallPool() { }

    public static TestHealBallPool GetInstance()
    {
        if(instance == null)
        {
            //ここで一度だけ作られる。
            instance = new TestHealBallPool();
        }
        return instance;
    }

    /// <summary>
    /// オブジェクトプールを作成
    /// </summary>
    /// <param name="fragment">生成するオブジェクト</param>
    /// <param name="firstCreateCount">最初に生成する個数</param>
    public void CreatePool(GameObject fragment, int firstCreateCount)
    {
        poolFragment = fragment;
        poolHealBallList = new List<GameObject>();

        for (int i = 0; i < firstCreateCount; i++)
        {
            GameObject obj = CreateNewObject();//オブジェクト生成
            obj.SetActive(false);              //生成時は非表示にする
            poolHealBallList.Add(obj);         //リストに入れる
        }
    }

    /// <summary>
    /// 使用可能なオブジェクトを探す
    /// </summary>
    /// <returns>待機状態のオブジェクトを返す</returns>
    public GameObject GetObject()
    {
        //使用中でないものを探す
        foreach (var pFList in poolHealBallList)
        {
            if (pFList.activeSelf == false)
            {
                pFList.SetActive(true);//使用中にして返す
                return pFList;
            }
        }

        //全て使用中だったら、新しく作る
        GameObject obj = CreateNewObject();
        obj.SetActive(true);//使用中にして返す

        //使用中にしてリストに加える
        poolHealBallList.Add(obj);

        return obj;
    }

    /// <summary>
    /// 足りなかった場合新しく生成
    /// </summary>
    /// <returns>名前を付けたオブジェクトを返す</returns>
    private GameObject CreateNewObject()
    {
        GameObject obj = Instantiate(poolFragment);
        obj.name = poolFragment.name + (poolHealBallList.Count + 1);

        return obj;
    }
}
