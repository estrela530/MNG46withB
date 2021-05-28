﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShot : MonoBehaviour
{
    public GameObject BossBullet;

    public GameObject Move;

    public int intarval;
    public float min = -100;
    public float max = 100;

    public float shotTime;
    public int shotCount;//撃った回数
    private float ss;

    private float dis;//プレイヤーとの距離
    private GameObject Target;//追尾する相手

    public bool shotFlag;

    //レイ関連
    Ray ray;
    RaycastHit hitRay;
    LineRenderer lineRenderer;

    int enemyNumber = (1 << 13 | 1 << 8);

    // Start is called before the first frame update
    void Start()
    {
        ss = 1;
        Target = GameObject.FindGameObjectWithTag("Player");//追尾させたいオブジェクトを書く
        Move.GetComponent<BossMove>();

        ray = new Ray();
        lineRenderer = this.gameObject.GetComponent<LineRenderer>();

        //lineRenderer.SetPosition(0, this.transform.position);
        lineRenderer.enabled = true ;
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
        ss += Time.deltaTime;
        dis = Vector3.Distance(transform.position, Target.transform.position);//二つの距離を計算して一定以下になれば追尾

        Random.Range(min, max);
        
        Ray();
    }

    void Ray()
    {
        lineRenderer.SetPosition(0, this.transform.position);

        if(!Physics.Raycast(ray, out hitRay, 30, enemyNumber))
        {
            lineRenderer.enabled = false;
        }

        //レイの処理
        if (Physics.Raycast(ray, out hitRay, 30, enemyNumber))
        {
            if (hitRay.collider.gameObject.CompareTag("Player"))
            {
                lineRenderer.enabled = true;
               
                lineRenderer.SetPosition(1, hitRay.point);
                shotFlag = true;
                if (Move.GetComponent<BossMove>().AttackFlag == true)
                {
                    ss += Time.deltaTime;
                    if (ss >= intarval)
                    {
                        Shot();
                        ss = 0;

                    }
                    //攻撃する前に色を変える
                    if (ss >= intarval - 1)
                    {
                        //ラインレンダラーの色
                        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                        lineRenderer.startColor = Color.red;//初めの色
                        lineRenderer.endColor = Color.red;//終わりの色

                    }
                }
                
            }
            else
            {
                //lineRenderer.enabled = false;//(弾が間にいると点滅みたいになる)
                shotFlag = false;
            }
            if (!hitRay.collider.gameObject.CompareTag("Player"))
            {
                shotFlag = false;
                ss = 0;
                //ラインレンダラーの色
                lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                lineRenderer.startColor = Color.green;//初めの色
                lineRenderer.endColor = Color.green;//終わりの色
            }
        }

        ray.origin = this.transform.position;//自分の位置のレイ

        ray.direction = transform.forward;//自分の向きのレイ

        //Debug.DrawRay(ray.origin, ray.direction * 10, Color.red, 0.1f);
    }


    void Shot()
    {
        shotCount = shotCount + 1;

        if (shotFlag)
        {
            Vector3 ff = new Vector3(dis + Random.Range(min, max), 0, dis);
            GameObject shot = Instantiate(BossBullet, transform.position, transform.rotation);
            Rigidbody rigidbody = shot.GetComponent<Rigidbody>();
            //rigidbody.AddForce(transform.forward * shotTime);
            rigidbody.AddForce(ff * shotTime);
        }

    }
}
