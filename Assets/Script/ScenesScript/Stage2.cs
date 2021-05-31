using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SceneNumberData.numberData.referer = "Game2";
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("number"+SceneNumberData.numberData.referer);
    }
}
