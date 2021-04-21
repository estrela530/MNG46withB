using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 見た目変更のテストクラス
/// </summary>
public class TestTest : MonoBehaviour
{
    MeshFilter meshFilter;//メッシュフィルター
    Vector3[] vertices;//頂点データ


    // Start is called before the first frame update
    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();

        vertices = meshFilter.mesh.vertices;//頂点データを代入

        for(int i = 0; i < vertices.Length;i++)
        {


            //   -0.5-------------0.5
            //       |           |
            //       |           |
            //       |    〇     |←座標(0,0,0)が中心位置
            //       |           |
            //       |           |
            //   -0.5-------------0.5

            ////Y軸が+の頂点だけ移動
            //if (vertices[i].y > 0)
            //{
            //    vertices[i] += new Vector3(0.3f, 0.3f, 0.3f);
            //}

            ////中心位置から絶対値0.2の四角を作って上に伸ばす
            //if(Mathf.Abs(vertices[i].x) < 0.2f && Mathf.Abs(vertices[i].z) < 0.2f)
            //{
            //    vertices[i] += new Vector3(0, 3, 0);
            //}

            if (vertices[i].y > 0)
            {
                vertices[i] = Quaternion.Euler(45, 0, 0) * vertices[i];
            }


            meshFilter.mesh.vertices = vertices;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
