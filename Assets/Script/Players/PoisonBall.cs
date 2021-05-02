using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonBall : InhaleObject
{
    [SerializeField,Header("毒ガスを生成するまでの時間")]
    float deleteTime = 5;

    GameObject poisonSmoke;   //エフェクト用オブジェクト
    MeshRenderer meshRenderer;//色変え用
    Animator animator;        //アニメーション用

    float smokeCount = 0;//生成時間計測用  
    bool smokFlag;       //毒ガス生成状態

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        animator = GetComponent<Animator>();

        meshRenderer.material.color = new Color(1, 0, 1, 0);//色変更

        //エフェクト用オブジェクトを取得
        poisonSmoke = transform.GetChild(0).gameObject;
        poisonSmoke.SetActive(false);//待機状態
        smokFlag = false;
    }

    private void FixedUpdate()
    {
        Smoke();//毒ガスの時間計測
    }

    /// <summary>
    /// 毒ガスを生成
    /// </summary>
    void Smoke()
    {
        //一定時間経過で毒ガスを生成する
        //毒玉本体は「コア」として残る。
        //コアが破壊されると、毒ガスも消滅する。
        //毒ガス生成時、破壊しやすいようにコアを拡大する。

        smokeCount += Time.deltaTime;

        if (smokeCount > deleteTime)
        {
            smokFlag = true;

        }

        if (smokFlag)
        {
            poisonSmoke.SetActive(true);
            animator.SetTrigger("Smoke");
            this.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    /// <summary>
    /// このオブジェクトがDestroyされたときに呼ばれる。
    /// </summary>
    private void OnDestroy()
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        DestroyImmediate(renderer.material);//マテリアルのメモリを削除
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Fragment"))
        {
            Destroy(this.gameObject);
        }
    }
}
