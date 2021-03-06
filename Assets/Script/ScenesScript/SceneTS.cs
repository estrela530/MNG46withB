using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTS : MonoBehaviour
{
    int fadeCount;
    int fadeMax;
    bool isSceneChangeFlag;
    float houtiGoTitleCount;

    [SerializeField, Header("フェードPrefab")]
    GameObject fadeManager;

    // Start is called before the first frame update
    void Start()
    {
        fadeCount = 0;
        fadeMax = 90;
        isSceneChangeFlag = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //SceneChange();
        if (Input.GetKeyDown(KeyCode.JoystickButton0) || (Input.GetKeyDown(KeyCode.Return)))
        {
            isSceneChangeFlag = true;
            //Debug.Log(isSceneChangeFlag);
        }
        if (isSceneChangeFlag)
        {
            SceneChange();
        }

        houtiGoTitleCount += Time.deltaTime;
        if (houtiGoTitleCount >= 60)
        {
            SceneManager.LoadScene("Prologue");
        }

    }

    //シーン切り替え用
    void SceneChange()
    {
        fadeManager.SetActive(true);
        fadeCount++;
        if (fadeCount >= fadeMax)
        {
            SceneManager.LoadScene("StageSelect");
        }
    }

}


//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class SceneTS : MonoBehaviour
//{
//    Start is called before the first frame update
//    void Start()
//    {

//    }

//    Update is called once per frame
//    void Update()
//    {
//        SceneChange();
//    }

//    シーン切り替え用
//    void SceneChange()
//    {
//        if (Input.GetKeyDown(KeyCode.Space))
//            if (Input.GetKeyDown(KeyCode.JoystickButton0) || (Input.GetKeyDown(KeyCode.Space)))
//            {
//                SceneManager.LoadScene("StageSelect");
//            }


//    }

//}