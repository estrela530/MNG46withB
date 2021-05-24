using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearanceEff : MonoBehaviour
{
    [SerializeField] GameObject spawnPoint;

    [SerializeField, Header("出現実行フラグ")]
    bool AppearFlag;

    [SerializeField, Header("出現までの時間")]
   float AppearTime;

    [SerializeField, Header("召喚のエフェクト")]
    GameObject SummonEffect;

    private ParticleSystem SummonParticle;

    [SerializeField, Header("召喚のエフェクトの魔法陣")]
    GameObject MagicCircle;

    private int EffectCount;

    // Start is called before the first frame update
    void Start()
    {
        AppearFlag = false;
        spawnPoint.GetComponent<SpawnPoint>();
        SummonParticle = SummonEffect.GetComponent<ParticleSystem>();

        MagicCircle.transform.position = SummonEffect.transform.position = this.transform.position;


        EffectCount = 0;
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(spawnPoint.GetComponent<SpawnPoint>().isTouchPlayerFlag == true)
        {
            //出現までカウントスタート
            AppearTime -= Time.deltaTime;
            Debug.Log(AppearTime+"時間");
            //魔法陣を出す
            MagicCircle.SetActive(true);

            //エフェクトパーティクル
            var eff = Instantiate(SummonEffect,
                   this.transform.position,
                   Quaternion.identity);

            if (AppearTime<=0)
            {
                AppearFlag = true;
            }
        }

        if(AppearFlag)
        {
            SummonParticle.Stop();//パーティクルを消す
            this.gameObject.SetActive(true);
        }
    }
}
