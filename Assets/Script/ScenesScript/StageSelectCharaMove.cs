using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectCharaMove : MonoBehaviour
{

    [SerializeField, Header("ねじりん")]
    GameObject nejirin;

    Vector3 nejirinnStage1Pos = new Vector3(240,280, 0);
    Vector3 nejirinnStage2Pos = new Vector3(500, 280, 0);

    // Start is called before the first frame update
    void Start()
    {
        nejirin.transform.position = nejirinnStage1Pos;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            nejirin.transform.position = nejirinnStage1Pos;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            nejirin.transform.position = nejirinnStage2Pos;
        }

    }
}
