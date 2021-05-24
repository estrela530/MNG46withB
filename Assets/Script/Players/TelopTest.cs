using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;


public class TelopTest : MonoBehaviour
{
    public Fungus.Flowchart flowchart;
    public string text = "";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            flowchart.SendFungusMessage(text);
        }
    }
}
