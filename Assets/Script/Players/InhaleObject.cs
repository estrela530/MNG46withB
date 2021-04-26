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

    protected Player player;
    protected Vector3 playerPosition;
    protected float speed;
    protected float moveDistance;
    protected int playerLevel;
    protected int moveState;
    protected bool isTwisted;

    // Start is called before the first frame update
    void Start()
    {
        moveState = 0;
        isTwisted = false;

        speed = inhaleSpeed;

        player = GameObject.Find("SlimePlayer").GetComponent<Player>();
    }

    private void FixedUpdate()
    {
        Move();

        //Debug.Log("私は親" +player);
    }

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
                    //指定した範囲内にじぶんがいたら
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
                    //Debug.Log(speed);
                    //移動
                    transform.position += direction * speed * Time.deltaTime;
                    break;
                default:
                    Debug.Log("回復玉が存在しない移動状態になっています。");
                    break;
            }
        }
        else
        {
            moveState = 0;
        }
    }
}
