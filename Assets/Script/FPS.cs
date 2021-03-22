using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPS : MonoBehaviour
{

    //FPS
    int frameCount;
    float prevTime;
    [SerializeField]
    float fps;

    // Start is called before the first frame update
    void Start()
    {
        //FPS
        frameCount = 0;
        prevTime = 0.0f;

    }

    // Update is called once per frame
    void Update()
    {
        //FPS 表示
        frameCount++;
        float time = Time.realtimeSinceStartup - prevTime;

        if (time >= 0.5f)
        {
            fps = frameCount / time;
            frameCount = 0;
            prevTime = Time.realtimeSinceStartup;
        }

    }

    //表示処理
    private void OnGUI()
    {
        GUI.skin.label.fontSize = 50;
        GUILayout.Label(fps.ToString());
    }

}

//using System.Collections;
//using System.Threading;
//using UnityEngine;

//public class FPS : MonoBehaviour
//{
//    public float Rate = 50.0f;
//    float currentFrameTime;

//    void Start()
//    {
//        QualitySettings.vSyncCount = 0;
//        Application.targetFrameRate = 9999;
//        currentFrameTime = Time.realtimeSinceStartup;
//        StartCoroutine("WaitForNextFrame");
//    }

//    IEnumerator WaitForNextFrame()
//    {
//        while (true)
//        {
//            yield return new WaitForEndOfFrame();
//            currentFrameTime += 1.0f / Rate;
//            var t = Time.realtimeSinceStartup;
//            var sleepTime = currentFrameTime - t - 0.01f;
//            if (sleepTime > 0)
//                Thread.Sleep((int)(sleepTime * 1000));
//            while (t < currentFrameTime)
//                t = Time.realtimeSinceStartup;
//        }
//    }
//}