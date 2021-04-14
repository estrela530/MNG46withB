using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionManagerScript : MonoBehaviour
{
    [SerializeField, Header("ミッション1星")]
    GameObject Mission1;
    [SerializeField, Header("ミッション2星")]
    GameObject Mission2;
    [SerializeField, Header("ミッション3星")]
    GameObject Mission3;

    GameObject m1;
    GameObject m2;
    GameObject m3;

    float fadeCount;

    // Start is called before the first frame update
    void Start()
    {
        fadeCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        fadeCount++;

        if (fadeCount >= 120)
        {
            Mission1.GetComponent<Image>().color = Color.yellow;
        }
        if (fadeCount >= 300)
        {
            Mission2.GetComponent<Image>().color = Color.yellow;
        }
        if (fadeCount >= 480)
        {
            Mission3.GetComponent<Image>().color = Color.yellow;
        }       
    }
}
