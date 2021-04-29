using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class TestJump : MonoBehaviour
{
    public bool isJump = false;//ジャンプ中か
    public bool isGround;

    public float jumpPower;
    public float time;
    public float gravity;

    float test;

    Rigidbody rigid;

    Transform myTrans;//自身の情報
    Vector3 position;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        myTrans = gameObject.transform;
        position = new Vector3(transform.position.x, 1, transform.position.z);
    }

    private void FixedUpdate()
    {
        Debug.Log("ジャンプしたか" + isJump);
        Debug.Log("重力" + test);
        //Jump();


        if(CheckGround())
        {
            transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);
            isJump = false;
        }
        else
        {
            //これが地面についていないとき


            rigid.AddForce(Vector3.down * gravity);
        }
    }

    bool CheckGround()
    {
        if (transform.position.y <= 0.0f)
        {
            transform.position = new Vector3(transform.position.x, 0.01f, transform.position.z);
            //地面についている状態とする。
            return true;
        }

        return false;
    }

    /// <summary>
    /// isJumpがtrueなら実行される処理
    /// </summary>
    void Jump()
    {
        if (isJump)
        {
            //position = new Vector3(transform.position.x, 1, transform.position.z);
            //myTrans.DOLocalJump(
            //    position,//終了時の位置
            //    jumpPower,//ジャンプ力
            //    1,//ジャンプ回数
            //    time//再生時間
            //).OnComplete(() =>
            //{
            //    isJump = false;
            //});

            //ジャンプしていたら下に下げる
            test = gravity;
            Debug.Log("ジャンプ中");

            if (transform.position.y < 0)
            {
                Debug.Log("1回しか呼ばれちゃダメな奴");
                transform.position = new Vector3(transform.position.x, 0, transform.position.z);
                
                isJump = false;
            }
        }
        else
        {
            test = 0;
        }

        rigid.AddForce(Vector3.down * test);
    }

    // Update is called once per frame
    void Update()
    {
        if (isJump == false)
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                rigid.AddForce(Vector3.up * jumpPower);
                isJump = true;
            }
        }
    }
}
