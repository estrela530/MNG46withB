using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EpilogTest : MonoBehaviour
{
    [SerializeField]
    GameObject[] scenes;

    enum SceneState
    {
        Scene1 = 0,
        Scene2,
        Scene3,
        Scene4,
        Scene5,
        Scene6
    }SceneState sceneState;

    // Start is called before the first frame update
    void Start()
    {
        ResetScene();

        sceneState = SceneState.Scene1;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("現在のシーンは" + sceneState);

        switch (sceneState)
        {
            case SceneState.Scene1:
                ResetScene();
                scenes[(int)SceneState.Scene1].SetActive(true);

                if(Input.GetKeyDown(KeyCode.Space))
                {
                    sceneState = SceneState.Scene2;
                }
                break;
            case SceneState.Scene2:

                ResetScene();
                scenes[(int)SceneState.Scene2].SetActive(true);

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    sceneState = SceneState.Scene1;
                }
                break;
            case SceneState.Scene3:
                break;
            case SceneState.Scene4:
                break;
            case SceneState.Scene5:
                break;
            case SceneState.Scene6:
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 全てのシーンの表示状態をリセットする
    /// </summary>
    void ResetScene()
    {
        for (int i = 0; i < scenes.Length; i++)
        {
            scenes[i].SetActive(false);
        }
    }
}
