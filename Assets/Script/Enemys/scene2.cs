﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class scene2 : MonoBehaviour
{
    public GameObject Enemy1;
    // Start is called before the first frame update
    void Start()
    {
        Enemy1.GetComponent<EnemyMove>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Enemy1.GetComponent<EnemyMove>().HpGet() <= 0 )
        {
            //gameObject.SetActive(false);//非表示
            SceneManager.LoadScene("Boss");
        }
    }
}