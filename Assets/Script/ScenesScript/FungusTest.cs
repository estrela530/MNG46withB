using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FungusTest : MonoBehaviour
{
    public Fungus.Flowchart flowchart = null;

    // Start is called before the first frame update
    void Start()
    {

        flowchart.SendFungusMessage("Koppe");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            flowchart.SendFungusMessage("Koppe");
        }
    }
}
