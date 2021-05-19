using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveIcon : CameraMove
{
    [SerializeField,Header("移動速度(要調整)")]
    private float time;

    Player playerObj; //プレイヤーを取得
    Vector3 startPos; //初期位置
    Vector3 offset;   //最大で引いた位置
    Vector3 endPos;   //レベル3の位置
    Vector3 direction;//向き

    bool istwisted = false;//ねじっているか
    int neziLevel = 0;     //ねじレベル

    // Start is called before the first frame update
    void Start()
    {
        playerObj = player.GetComponent<Player>();
        istwisted = playerObj.GetTwisted();
        neziLevel = playerObj.GetNeziLevel();

        //位置の初期化
        startPos = player.transform.position + offsetPos;
        endPos = new Vector3(0.8f, 6.5f, -1.5f);
        offset = player.transform.position + endPos;
    }

    private void FixedUpdate()
    {
        base.SetAngle();
        SetPosision();
    }

    public override void SetPosision()
    {
        istwisted = playerObj.GetTwisted();
        if (istwisted)
        {
            neziLevel = playerObj.GetNeziLevel();

            if (neziLevel < 3)
            {
                //ねじっている&レベルが3未満だったらカメラを移動させる
                //移動位置は、レベル3の時の固定位置
                //時間の調整が結構大変

                direction = offset - startPos;
                direction.Normalize();
                this.transform.position += direction * Time.deltaTime * time;
            }
            else
            {
                //レベル3に達したら指定の位置で固定

                position = player.transform.position + endPos;
                this.transform.position = position;

            }
        }
        else
        {
            //ねじっていないときは元の位置
            base.SetPosision();
        }

    }
}
