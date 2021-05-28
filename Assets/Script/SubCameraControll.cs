using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubCameraControll : MonoBehaviour
{
    [SerializeField] GameObject mainCamera;
    [SerializeField] GameObject subCamera1;
    [SerializeField] GameObject subCamera2;
    [SerializeField] GameObject subCamera3;
    [SerializeField] GameObject Boss;
    private BossMove BossEnemy;
    private OctaneEnemy Octane;
    private ScorpionBoss Scorpion;
    float ObjHp;
    [SerializeField] float speedLoc = 5;
    [SerializeField] int cameraState;
    [SerializeField] float transitionTime1;
    [SerializeField] float transitionTime2;


    // Start is called before the first frame update
    void Start()
    {
        //mainCamera = GameObject.Find("Main Camera");
        //subCamera = GameObject.Find("SubCamera");

        //オクタン
        if (this.Boss.GetComponent<OctaneEnemy>())
        {
            Octane = this.Boss.GetComponent<OctaneEnemy>();
            ObjHp = Octane.HpGet();
        }
        //ボス
        if (this.Boss.GetComponent<BossMove>())
        {
            BossEnemy = this.Boss.GetComponent<BossMove>();
            ObjHp = BossEnemy.HpGet();
        }

        //スコーピオン
        if (this.Boss.GetComponent<ScorpionBoss>())
        {
            Scorpion = this.Boss.GetComponent<ScorpionBoss>();
            ObjHp = Scorpion.HpGet();
        }
        mainCamera.SetActive(true);
        cameraState = 0;
        subCamera1.SetActive(false);
        subCamera2.SetActive(false);
        subCamera3.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //召喚
        if (ObjHp <= 0)
        {
            CameraMove();
        }

        //オクタン
        if (this.Boss.GetComponent<OctaneEnemy>())
        {
            ObjHp = Octane.HpGet();
        }
        //ボス
        else if (this.Boss.GetComponent<BossMove>())
        {
            ObjHp = BossEnemy.HpGet();
        }
        //
        else if (this.Boss.GetComponent<ScorpionBoss>())
        {
            ObjHp = Scorpion.HpGet();
        }

    }

    void CameraMove()
    {
        mainCamera.SetActive(false);
        switch (cameraState)
        {
            case 0:
                cameraState = 1;
                break;

            case 1:
                subCamera1.SetActive(true);
                transitionTime1 -= Time.deltaTime;

                subCamera1.transform.LookAt(Boss.transform.position);
                subCamera1.transform.position += transform.forward * speedLoc * Time.deltaTime;

                if(transitionTime1<=0)
                {
                    cameraState = 2;
                }
                
                break;

            case 2:
                transitionTime2 -= Time.deltaTime;
                subCamera1.SetActive(false);
                subCamera2.SetActive(true);

                subCamera2.transform.LookAt(Boss.transform.position);
                subCamera2.transform.position += transform.forward * speedLoc * Time.deltaTime;
                if (transitionTime2 <= 0)
                {
                    cameraState = 3;
                }

                break;

            case 3:
                subCamera2.SetActive(false);
                subCamera3.SetActive(true);

                subCamera3.transform.LookAt(Boss.transform.position);
                subCamera3.transform.position += transform.forward * speedLoc * Time.deltaTime;
                break;
        }
       
    }
}
