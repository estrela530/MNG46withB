using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestParticles : MonoBehaviour
{
    //パーティクルの宣言(特殊)
    ParticleSystem.MainModule particle;


    // Start is called before the first frame update
    void Start()
    {
        //パーティクルの取得(特殊)
        particle = GetComponent<ParticleSystem>().main;

        //レベルごとに変わっているところを書こうぜ
        particle.duration = 1;
        particle.startLifetime = 1;
        particle.startSpeed = -3;
        //particle.startSize;
        //    particle.startSize.constantMax = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
