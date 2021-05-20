using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;

//using UnityStandardAssets.Characters.ThirdPerson;
//[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
//[RequireComponent(typeof(BoxCollider))]

public class EnemyMove : MonoBehaviour
{
   
    // Start is called before the first frame update
    private GameObject Target;//追尾する相手
    private float dis;//プレイヤーとの距離
   // public float area;//この数値以下になったら追う

    [SerializeField,Header("体力")] float enemyHP;

    [SerializeField, Header("最大体力")] float MaxEnemyHP;


    Rigidbody rigid;

    //Color color;
    

    [Header("追う時と索敵のフラグ")]
    public bool MoveFlag = false;//追う
    public bool workFlag = true;//徘徊

   
    GameObject stageMove1;

    Renderer renderComponent;
    [SerializeField] float ColorInterval = 0.1f;
    [SerializeField] float DamageTime;
    [SerializeField, Header("ダメージ受けた時")]
    bool DamageFlag;

    [SerializeField, Header("死んだ時のエフェクト")]
    private GameObject DeathEffect;
    private ParticleSystem DeathParticle;   //ダメージのパーティクル

    void Start()
    {
        MaxEnemyHP = enemyHP ;
        DeathParticle = DeathEffect.GetComponent<ParticleSystem>();
        renderComponent = GetComponent<Renderer>();
        stageMove1 = GameObject.FindGameObjectWithTag("StageMove");
        stageMove1.GetComponent<StageMove1>();
        
        Target = GameObject.FindGameObjectWithTag("Player");
        rigid = GetComponent<Rigidbody>();
        //color = GetComponent<Renderer>().material.color;
        
    }
    IEnumerator Blink()
    {
        while (true)
        {
            renderComponent.enabled = !renderComponent.enabled;
            //何フレームとめる
            yield return new WaitForSeconds(ColorInterval);
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        rigid.angularVelocity = Vector3.zero;
        rigid.velocity = Vector3.zero;

        //最大体力以上にはならない。
        if (enemyHP >= MaxEnemyHP)
        {
            enemyHP = MaxEnemyHP ;
        }
        
        //ダメージ演出
        if (enemyHP > 0)
        {
            //ダメージ
            if (DamageFlag)
            {
                DamageTime += Time.deltaTime;
                StartCoroutine("Blink");
                if (DamageTime > 1)
                {
                    DamageTime = 0;
                    StopCoroutine("Blink");
                    renderComponent.enabled = true;
                    DamageFlag = false;
                }

                

            }
        }
        if (stageMove1.GetComponent<StageMove1>().nowFlag == true)
        {
            MoveFlag = false;
        }

        //常にターゲットにむく
        this.transform.LookAt(new Vector3(Target.transform.position.x, this.transform.position.y, Target.transform.position.z));

        if (enemyHP <= 0 && !stageMove1.GetComponent<StageMove1>().bossNow)
        {
            //gameObject.SetActive(false);//非表示
            TimerScript.enemyCounter += 1;
            var sum = Instantiate(DeathEffect,
                           this.transform.position,
                           Quaternion.identity);
            //Debug.Log(DeathParticle + "エフェクトーーーー");
            //SceneManager.LoadScene("Result");
            //SceneManager.LoadScene("GameClear");
            Destroy(this.gameObject);

        }

        dis = Vector3.Distance(transform.position, Target.transform.position);//二つの距離を計算して一定以下になれば追尾
        
        if (MoveFlag)
        {
           this.transform.LookAt(new Vector3(Target.transform.position.x, this.transform.position.y, Target.transform.position.z));//ターゲットにむく
            
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
            MoveFlag = true;
            DamageFlag = true;

        }
       
        //回復玉に当たったら回復する
        if (other.gameObject.CompareTag("HealBall"))
        {
            enemyHP = enemyHP + 1;
        }

    }

    //ノックバック処理
    void NockBack(GameObject other, float velocity)
    {
        Vector3 angles = other.transform.localEulerAngles;//当たったオブジェクトの角度
        Vector3 directions = Quaternion.Euler(angles) * Vector3.forward;//Wuaternionに変換しつつ正面ベクトル(0, 0 ,1)とかけて

        this.transform.position += directions * velocity * Time.deltaTime;
    }

    private void OnDestroy()
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        DestroyImmediate(renderer.material); //マテリアルのメモリーを消す
    }

    
}
