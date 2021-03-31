using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 予測線オブジェクトプール
/// </summary>
public class PredictionLinePool : MonoBehaviour
{
    private List<GameObject> predictionLinePool;
    private GameObject predictionLine;

    /// <summary>
    /// 予測線プールの作成
    /// </summary>
    /// <param name="line">生成するオブジェクト</param>
    /// <param name="createCount">初期生成個数</param>
    public void CreatePredictionLinePool(GameObject line,int createCount)
    {
        predictionLinePool = new List<GameObject>();
        predictionLine = line;

        for(int i = 0;i < createCount;i++)
        {
            GameObject obj = Instantiate(predictionLine);
            obj.name = predictionLine.name + (predictionLinePool.Count + 1);
            obj.SetActive(false);
            predictionLinePool.Add(obj);
        }
    }

    public GameObject GetActiveObject()
    {
        foreach (var list in predictionLinePool)
        {
            if (list.activeSelf == false)
            {
                list.SetActive(true);
                return list;
            }
        }

        GameObject obj = Instantiate(predictionLine);
        obj.name = predictionLine.name + (predictionLinePool.Count + 1);
        obj.SetActive(true);
        predictionLinePool.Add(obj);
        return obj;
    }

    /// <summary>
    /// リスト内のオブジェクトを全て非アクティブにする
    /// </summary>
    public void ResetActive()
    {
        foreach(var list in predictionLinePool)
        {
            if(list.activeSelf == true)
            {
                list.SetActive(false);
            }
        }
    }

}
