﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalFlag : MonoBehaviour
{
    StageMove stageMove;
    [SerializeField]
    private GameObject StageMovePrefab;


    // Start is called before the first frame update
    void Start()
    {

        stageMove = StageMovePrefab.GetComponent<StageMove>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("ハタハタ");
            stageMove. nowFlag = true;

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}