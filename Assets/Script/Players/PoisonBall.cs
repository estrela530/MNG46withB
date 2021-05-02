using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonBall : InhaleObject
{
    GameObject findObject;

    MeshRenderer meshRenderer;//色変え用
                              //Animator animator;//アニメーション用

    float deleteCount = 0;
    float deleteTime = 5;

    GameObject child;
    bool smokFlag = false;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        //animator = GetComponent<Animator>();

        meshRenderer.material.color = new Color(1, 0, 1, 0);

        child = transform.GetChild(0).gameObject;
        child.SetActive(false);
        smokFlag = false;
    }

    private void FixedUpdate()
    {
        deleteCount += Time.deltaTime;

        if(deleteCount > deleteTime)
        {
            smokFlag = true;
            
        }

        if(smokFlag)
        {
            child.SetActive(true);
            this.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            child.SetActive(false);
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
