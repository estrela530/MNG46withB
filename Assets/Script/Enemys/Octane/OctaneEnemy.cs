﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;

//[RequireComponent(typeof(BoxCollider))]

public class OctaneEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject Target;//追尾する相手
    private float dis;//プレイヤーとの距離
    // public float area;//この数値以下になったら追う
    StageMove1 stageMove1;

    [Header("体力")]public float enemyHP = 5;

    Rigidbody rigid;

    private float workeAria1 = 1;//
    private float workeAria2 = 1;//

    private float Rspeed;

    private float ww;
    private float ww2;
    Player player;
    Color color;
    //Player player;
    
    int workNumber = 1;
    
    [Header("発見時のスピード")]
    public float speedLoc;

    [Header("この数値まで進む")] public float social;//この数値まで進む
    private GameObject Enemy;

    [Header("追う時")]
    public bool MoveFlag = false;//追う
    //public bool lookFlag = false;

    [SerializeField, Header("何秒止まるか")]
    public float freezeTime;
    public float lookTime;

    Ray ray;
    RaycastHit hitRay;
    LineRenderer lineRenderer;

    Vector3 playerPos;
    Vector3 EnemyPos;
    Vector3 velocity = Vector3.zero;

    public int moveState;

    // Start is called before the first frame update
    void Start()
    {
        moveState = 0;
        stageMove1 = GetComponent<StageMove1>();
        Target = GameObject.FindGameObjectWithTag("Player");
        rigid = GetComponent<Rigidbody>();
        color = GetComponent<Renderer>().material.color;

        ray = new Ray();
        lineRenderer = this.gameObject.GetComponent<LineRenderer>();

        //lineRenderer.SetPosition(0, this.transform.position);
        lineRenderer.enabled = false;
        ray.origin = this.transform.position;//自分の位置のレイ

        //ラインレンダラーの色
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.green;//初めの色
        lineRenderer.endColor = Color.green;//終わりの色

        lineRenderer.startWidth = 0.5f;
        lineRenderer.endWidth = 0.5f;

        //変えるかも?
        ray.direction = transform.forward;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rigid.angularVelocity = Vector3.zero;
        rigid.velocity = Vector3.zero;

        if(stageMove1.GetComponent<StageMove1>().nowFlag == true)
        {
            MoveFlag = false;
            moveState = 0;
        }

        switch(moveState)
        {
            case 0:
                
                if(MoveFlag)
                {
                    moveState = 1;
                }
                break;

            case 1://見てる時
                lookTime += Time.deltaTime;


                

                if (lookTime <= freezeTime)
                {
                    this.transform.LookAt(new Vector3(Target.transform.position.x, this.transform.position.y, Target.transform.position.z));//ターゲットにむく

                    //レイの処理
                    if (Physics.Raycast(ray, out hitRay, 15))
                    {
                        if (hitRay.collider.gameObject.CompareTag("Player"))
                        {
                            lineRenderer.enabled = true;

                            lineRenderer.SetPosition(1, hitRay.point);
                            //hitRay.point;
                        }

                    }
                }

                if(lookTime >= freezeTime - 0.5f)
                {
                    lineRenderer.startColor = Color.red;//初めの色
                    lineRenderer.endColor = Color.red;//終わりの色
                }

                if (lookTime >= freezeTime)
                {
                    moveState = 2;
                }
                 break;

            case 2://位置

                playerPos = Target.transform.position;
                //this.transform.LookAt(new Vector3(playerPos.x, this.transform.position.y, playerPos.z));//ターゲットにむく
                moveState = 3;
                break;

            case 3:

                lineRenderer.startColor = Color.green;//初めの色
                lineRenderer.endColor = Color.green;//終わりの色

                transform.position = Vector3.MoveTowards(transform.position, new Vector3(playerPos.x,this.transform.position.y,playerPos.z), speedLoc * Time.deltaTime);

                lineRenderer.enabled = false;//(弾が間にいると点滅みたいになる)

                lookTime = 0;

                if(playerPos.x == transform.position.x
                    && playerPos.z == transform.position.z)
                {
                    moveState = 0;
                }
                //moveState = 0;
                break;

        }

        if (enemyHP <= 0)
        {
            gameObject.SetActive(false);//非表示
        }

        dis = Vector3.Distance(transform.position, Target.transform.position);//二つの距離を計算して一定以下になれば追尾
        
        //レイ
        lineRenderer.SetPosition(0, this.transform.position);

        ray.origin = this.transform.position;//自分の位置のレイ

        ray.direction = transform.forward;//自分の向きのレイ

        Debug.DrawRay(ray.origin, ray.direction * 10, Color.red, 0.1f);
        
    }


    public float HpGet()
    {
        return enemyHP;
    }

    //(仮)指定されたtagに当たると消える
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Fragment"))
        {
            enemyHP = enemyHP - 1;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player")|| 
            other.gameObject.CompareTag("Wall"))
        {
            MoveFlag = false;
            moveState = 0;
        }

    }

    private void OnDestroy()
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        DestroyImmediate(renderer.material); //マテリアルのメモリーを消す
    }
}
