using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class αScore : MonoBehaviour
{
    //被弾回数入れるよう変数
    int damageCount;
    //解放回数入れるよう変数
    int releaseCount;
    //スコア表示用テキスト
    public Text scoreText;


    // Start is called before the first frame update
    void Start()
    {
        scoreText = GetComponent<Text>();

        damageCount = 3;
        releaseCount = 12;
        //くぼしょ～の協力　被弾回数の取得
        //damageCount = Player.aaa;
        //くぼしょ～の協力　解放回数の取得
        //releaseCount = Player.bbb;
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "" + damageCount + "." + releaseCount;
    }
}
