using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectCharaMove : MonoBehaviour
{

    [SerializeField, Header("ねじりん")]
    GameObject nejirin;
    [SerializeField, Header("ステージ1")]
    GameObject s1Image;
    [SerializeField, Header("ステージ2")]
    GameObject s2Image;


    Vector3 nejirinnStage1Pos = new Vector3(-40, -23, 80);
    Vector3 nejirinnStage2Pos = new Vector3(5, -9, 80);
    bool stage1nowFlag;
    bool stage2NowFlag;

    public bool isMissionImageFlag;

    // Start is called before the first frame update
    void Start()
    {
        nejirin.transform.position = nejirinnStage1Pos;
        stage1nowFlag = true;
        stage2NowFlag = false;
        isMissionImageFlag = true;
        s2Image.SetActive(false);
        s1Image.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        //L Stick
        float lsh = Input.GetAxisRaw("Horizontal");

        Debug.Log(lsh);

        if (Input.GetKeyDown(KeyCode.LeftArrow) || lsh < 0)
        {
            stage1nowFlag = true;
            s2Image.SetActive(false);
            s1Image.SetActive(true);
            nejirin.transform.position = nejirinnStage1Pos;
            stage1nowFlag = false;
            isMissionImageFlag = true;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || lsh > 0)
        {
            stage2NowFlag = true;
            s2Image.SetActive(true);
            s1Image.SetActive(false);
            nejirin.transform.position = nejirinnStage2Pos;
            stage2NowFlag = false;
            isMissionImageFlag = true;
        }

    }
}
