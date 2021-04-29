using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneGR : MonoBehaviour
{
    public bool changeOKFlag = true;

    // Start is called before the first frame update
    void Start()
    {
        //changeOKFlag = false;
        changeOKFlag = true;
    }

    // Update is called once per frame
    void Update()
    {
        changeOKFlag = true;
        SceneChange();
    }

    //シーン切り替え用
    void SceneChange()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            changeOKFlag = true;
            SceneManager.LoadScene("GameClear");
            Debug.Log(changeOKFlag);

        }


    }

}
