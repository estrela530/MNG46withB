using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 吸収されるオブジェクトにアタタッチしてください
/// </summary>
public class InhaleObject : MonoBehaviour
{
    //【Tips】回転時の吸収速度
    //基本的に吸収時の速度の2倍に設定しています。
    //変更したい場合は連絡お願いします。

    [SerializeField, Header("吸収時の移動速度")]
    protected float inhaleSpeed = 3f;
    [SerializeField, Header("ねじレベルによる吸収範囲")]
    protected int[] inhaleRange = new int[3];

    protected Player player;　　　　　//プレイヤー格納用
    protected Vector3 playerPosition; //プレイヤーの位置保存用
    protected float speed;            //移動速度
    protected float moveDistance;     //プレイヤーとの距離(オブジェクトの移動距離)
    protected int playerLevel;        //プレイヤーのレベル取得用
    protected int moveState;          //オブジェクトの移動状態
    protected bool isTwisted;         //プレイヤーがねじっているかどうか

    protected GameObject findObject;    //プレイヤーを検索して保存する

    // Start is called before the first frame update
    void Start()
    {
        //まず最初にプレイヤーを探す。
        findObject = GameObject.FindGameObjectWithTag("Player");
        player = findObject.GetComponent<Player>();

        speed = inhaleSpeed;
        moveState = 0;
        isTwisted = false;    
    }

    private void FixedUpdate()
    {
        Move();//移動
    }

    /// <summary>
    /// 移動
    /// </summary>
    public virtual void Move()
    {
        if (player == null) return;

        //プレイヤーがねじっているかを取得
        isTwisted = player.GetTwisted();

        if (isTwisted == true)
        {
            switch (moveState)
            {
                case 0:
                    //まずは位置を取得(1度だけ)
                    playerPosition = player.GetPosition();
                    //ここでプレイヤーとの距離を計算
                    moveDistance = Vector3.Distance(playerPosition, transform.position);
                    //計算が終わったら次に進む
                    moveState = 1;
                    break;
                case 1:
                    //プレイヤーのレベルを取得
                    playerLevel = player.GetNeziLevel();
                    //指定した範囲内にじぶんがいたら次に進む
                    if (playerLevel == 3 && moveDistance < inhaleRange[2])
                    {
                        moveState = 2;
                    }
                    else if (playerLevel == 2 && moveDistance < inhaleRange[1])
                    {
                        moveState = 2;
                    }
                    break;
                case 2:
                    //ターゲットへの向きを取得
                    Vector3 direction = playerPosition - transform.position;
                    //正規化
                    direction.Normalize();
                    direction.y = 0.0f;
                    //移動
                    transform.position += direction * speed * Time.deltaTime;
                    break;
                default:
                    Debug.Log("回復玉が存在しない移動状態になっています。");
                    break;
            }
        }
        else moveState = 0;
    }
}
