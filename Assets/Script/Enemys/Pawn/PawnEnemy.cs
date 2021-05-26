using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;

public class PawnEnemy : MonoBehaviour
{
    private GameObject Target;//追尾する相手
    private float dis;//プレイヤーとの距離
    //public float area;//この数値以下になったら追う

    Rigidbody rigid;

    private GameObject Enemy;

    [SerializeField, Header("体力")] float enemyHP = 2;

    [SerializeField, Header("スピード")]
    float speedLoc;

    [Header("追う時のフラグ")]
    public bool MoveFlag;//追う

    [SerializeField, Header("死んだ時のエフェクト")]
    private GameObject DeathEffect;
    private ParticleSystem DeathParticle;   //ダメージのパーティクル

    [SerializeField, Header("死ぬエフェがでるまでの時間")]
    float DeathEffectTime = 0.5f;

    private int deathState;

    //[SerializeField] Animation anime;
    private string animeName;
    [SerializeField] Animator anime;
    [SerializeField] private float DeathTime = 0;

    GameObject stageMove1;

    GameObject[] enemyParts;
    int partsCount;

    //Renderer renderComponent;

    void Start()
    {
        DeathParticle = DeathEffect.GetComponent<ParticleSystem>();
        stageMove1 = GameObject.FindGameObjectWithTag("StageMove");
        stageMove1.GetComponent<StageMove1>();
        //Target = GameObject.Find("Player");//追尾させたいオブジェクトを書く
        Target = GameObject.FindGameObjectWithTag("Player");
        rigid = GetComponent<Rigidbody>();
        MoveFlag = true;

        deathState = 0;

        //renderComponent = GetComponent<Renderer>();

        //anime = gameObject.GetComponent<Animation>();
        //foreach(AnimationState Astate in anime)
        //{
        //    animeName = Astate.name;
        //}
        //anime[animeName].normalizedTime = 0f;


        anime = GetComponent<Animator>();

        partsCount = gameObject.transform.childCount;
        enemyParts = new GameObject[partsCount];
        anime.enabled = false;
        for (int i = 0; i < partsCount; i++)
        {
            enemyParts[i] = gameObject.transform.GetChild(i).gameObject;
            //enemyParts[i].SetActive(false);
        }
    }
    //優先度 1,エネミー達の死亡エフェクト(アニメーション) 2,ボスの死亡時のカメラズーム 3,ザコ敵召喚された時のエフェクト

    //死亡時 = アニメーション→メッシュ消し→パーティクルを消す
    //自分のセットアクティブがoffからonになった瞬間
    private void OnEnable()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //anime[animeName].normalizedTime = 1.0f;
        //anime[animeName].normalizedSpeed = 1
        rigid.angularVelocity = Vector3.zero;
        rigid.velocity = Vector3.zero;
        switch (deathState)
        {
            case 0:
                //if (enemyHP <= 0)
                //{

                //    //Destroy(transform.parent);
                //    var sum = Instantiate(DeathEffect,
                //                  this.transform.position,
                //                  Quaternion.identity);
                //    Destroy(this.gameObject);
                //}

                if (enemyHP <= 0)
                {
                    deathState = 1;
                }

                break;

            case 1:
                //アニメーション再生
                anime.enabled = true;
                anime.SetTrigger("Death");
                //anime.Play();
                //rigid.constraints = 
                //  RigidbodyConstraints.FreezePositionX|
                //  RigidbodyConstraints.FreezePositionZ;
                //Debug.Log("再生ーーーーーーー");
                DeathTime += Time.deltaTime;
                if (DeathTime > 1)
                {

                    DeathTime = 0;

                    deathState = 2;
                }
                break;

            case 2:
                for (int i = 0; i < partsCount; i++)
                {
                    enemyParts[i] = gameObject.transform.GetChild(i).gameObject;
                    enemyParts[i].SetActive(false);
                }

               // DeathEffectTime -= Time.deltaTime;
                var sum = Instantiate(DeathEffect,
                          this.transform.position,
                          Quaternion.identity);
                deathState = 3;
                //if (DeathEffectTime <= 0)
                //{
                    
                //}
                
                break;

            case 3:
                if (!stageMove1.GetComponent<StageMove1>().bossNow)
                {
                    TimerScript.enemyCounter += 1;
                }
                Destroy(this.gameObject);
                //gameObject.SetActive(false);//非表示
                break;

        }
       


        dis = Vector3.Distance(transform.position, Target.transform.position);//二つの距離を計算して一定以下になれば追尾

        //追いかける
        if (MoveFlag)
        {
            this.transform.LookAt(new Vector3(Target.transform.position.x, this.transform.position.y, Target.transform.position.z));//ターゲットにむく
            if (dis >= 1)
            {
                transform.position += transform.forward * speedLoc * Time.deltaTime;//前進(スピードが変わる)
            }
        }

        if (stageMove1.GetComponent<StageMove1>().nowFlag == true)
        {
            MoveFlag = false;
        }
        else if (!stageMove1.GetComponent<StageMove1>().nowFlag)
        {
            MoveFlag = true;
        }

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
    private void OnDestroy()
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        DestroyImmediate(renderer.material); //マテリアルのメモリーを消す
    }

}
