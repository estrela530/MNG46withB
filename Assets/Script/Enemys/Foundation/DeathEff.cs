using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathEff : MonoBehaviour
{
    private float DeathTime;
    // Start is called before the first frame update
    void Start()
    {
        DeathTime = 1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        DeathTime -= Time.deltaTime;
        if(DeathTime<=0)
        {
            Destroy(this.gameObject);
            DeathTime = 0;
        }
    }
}
