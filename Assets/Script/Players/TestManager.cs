using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    [SerializeField,Tooltip("新たに生成する回復玉")]
    private GameObject pre;

    bool initFlag = false;         //生成フラグ
    List<HealBall> objectList;     //回復玉の管理リスト
    List<HealBall> healBalls;      //リスト二つ目
    Vector3 initPos = Vector3.zero;//生成位置

    // Start is called before the first frame update
    void Start()
    {
        //リスト初期化
        objectList = new List<HealBall>();
        healBalls = new List<HealBall>();
    }

    /// <summary>
    /// リストに追加
    /// </summary>
    /// <param name="obj">追加するオブジェクト</param>
    public void AddList(HealBall obj)
    {
        objectList.Add(obj);
        healBalls.Add(obj);
    }

    /// <summary>
    /// リストから削除
    /// </summary>
    /// <param name="obj">削除するオブジェクト</param>
    public void RemoveList(HealBall obj)
    {
        objectList.Remove(obj);
        healBalls.Remove(obj);
    }

    // Update is called once per frame
    void Update()
    {
        //ぶつかり合った2つを、両方をこちら側で削除したい。

        //これでいちおう両方消えている
        for(int i = 0;i < objectList.Count;i++)
        {        
            for(int j = 0;j < healBalls.Count;j++)
            {
                if(objectList[i].GetHitFlag() && healBalls[j].GetHitFlag())
                {
                    //Destroy(objectList[i].gameObject);

                    //二つの位置を取得(保存変数はここで作る)
                    //二つの位置の中点を計算
                    //その位置を保存
                    //フラグをtrueにする

                    Vector3 pos1 = objectList[i].GetHitPos();
                    Vector3 pos2 = healBalls[j].GetHitPos();

                    Debug.Log("ObjectList : " + pos1);
                    Debug.Log("HealBalls : " + pos2);

                    objectList[i].gameObject.SetActive(false);
                    healBalls[j].gameObject.SetActive(false);
                }
            }

            ////このifは2回呼ばれているから両方消える。
            //if (objectList[i].GetHitFlag())
            //{
            //    //※当たった二つのうちどちらかの位置が入る。
            //    initPos = objectList[i].GetHitPos();
            //    //ふたつとも削除
            //    Destroy(objectList[i].gameObject);
            //    initFlag = true;

            //    //Debug.Log("InitPosition : "+initPos);
            //}

        }

        //一度だけオブジェクトを生成
        if(initFlag)
        {
            GameObject test = Instantiate(pre, initPos, Quaternion.identity) as GameObject;
            initFlag = false;
        }


        //Debug.Log("リスト内のオブジェクトの数 : " + objectList.Count);
    }
}
