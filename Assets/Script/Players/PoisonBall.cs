using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonBall : MonoBehaviour
{
    [SerializeField, Tooltip("吸収時の移動速度")]
    private float speed = 1.0f;
    Player player;//シーン上のプレイヤーを取得

    Vector3 playerPos = Vector3.zero;  //プレイヤーの現在位置

    bool isTwisted = false;//プレイヤーがねじっているか
    bool moveFlag = false; //false = ターゲット取得：true = 移動中

    void Start()
    {
        //色変更
        GetComponent<Renderer>().material.color = new Color(1, 0, 1, 0);

        //プレイヤーを取得
        //player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        player = GameObject.Find("SlimePlayer").GetComponent<Player>();
    }
    
    void Update()
    {
        Suction();
    }

    /// <summary>
    /// 吸収
    /// </summary>
    void Suction()
    {
        //プレイヤーがねじっているかを取得
        isTwisted = player.GetTwisted();

        if (isTwisted)
        {
            if (moveFlag == false)
            {
                //ターゲットの位置を取得
                playerPos = player.GetPosition();
                moveFlag = true;
            }
            else
            {
                //ターゲットへの向きを取得
                Vector3 direction = playerPos - this.transform.position;

                //正規化
                direction.Normalize();

                this.transform.position += direction * speed * Time.deltaTime;
            }
        }
        else moveFlag = false;
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
        if(other.gameObject.CompareTag("Fragment"))
        {
            Destroy(this.gameObject);
        }
    }
}
