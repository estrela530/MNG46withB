using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 生成した欠片を溜めるリストを管理する
/// </summary>
public class FragmentPool : MonoBehaviour
{
    private List<GameObject> poolFragmentList;//オブジェクトを入れておくリスト
    private GameObject poolFragment;          //リストに入れるオブジェクト

    /// <summary>
    /// オブジェクトプールを作成
    /// </summary>
    /// <param name="fragment">生成するオブジェクト</param>
    /// <param name="firstCreateCount">最初に生成する個数</param>
    public void CreatePool(GameObject fragment, int firstCreateCount)
    {
        poolFragment = fragment;
        poolFragmentList = new List<GameObject>();

        for (int i = 0; i < firstCreateCount; i++)
        {
            GameObject obj = CreateNewObject();//オブジェクト生成
            obj.SetActive(false);              //生成時は非表示にする
            poolFragmentList.Add(obj);         //リストに入れる
        }
    }

    /// <summary>
    /// 使用可能なオブジェクトを探す
    /// </summary>
    /// <returns>待機状態のオブジェクトを返す</returns>
    public GameObject GetObject()
    {
        //使用中でないものを探す
        foreach (var pFList in poolFragmentList)
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
        poolFragmentList.Add(obj);

        return obj;
    }

    /// <summary>
    /// 足りなかった場合新しく生成
    /// </summary>
    /// <returns>名前を付けたオブジェクトを返す</returns>
    private GameObject CreateNewObject()
    {
        GameObject obj = Instantiate(poolFragment);
        obj.name = poolFragment.name + (poolFragmentList.Count + 1);

        return obj;
    }
}
