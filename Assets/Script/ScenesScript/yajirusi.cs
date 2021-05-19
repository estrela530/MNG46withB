using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class yajirusi : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private int alphaCount = 0;//点滅用カウント
    Color alpha = new Color(0, 0, 0, 0.05f);

    bool alphaFlag = true;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = transform.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //alphaCount++;

        //if (alphaCount > 0)
        //{
        //    meshRenderer.material.color = Color.red;
        //}
        //if (alphaCount > 4)
        //{
        //    meshRenderer.material.color = Color.white;
        //}
        //if (alphaCount > 8)
        //{
        //    alphaCount = 0;
        //}

            float alpha = Mathf.Sin(Time.time * 5) / 2 + 0.5f;
            meshRenderer.material.color = new Color(255, 1, 1, alpha);
    }
}
