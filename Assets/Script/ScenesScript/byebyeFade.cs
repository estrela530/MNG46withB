using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class byebyeFade : MonoBehaviour
{
    //[SerializeField,Header("FadeManagerInをいれる")]
    private GameObject fadeManagerIn;
    public int disappearCount;

    // Start is called before the first frame update
    void Start()
    {
        fadeManagerIn = transform.GetChild(0).gameObject;
        disappearCount = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ByebyeFade();
    }

    //fadeManagerInを消すメソッド
    void ByebyeFade()
    {
        if (disappearCount <= 240)
        {
            disappearCount++;
        }
        else if (disappearCount >= 240)
        {
            fadeManagerIn.SetActive(false);
            disappearCount = 250;
        }
    }
}
