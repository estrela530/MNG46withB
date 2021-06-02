using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Houti : MonoBehaviour
{
    float houtiGoTitleCount;
    // Start is called before the first frame update
    void Start()
    {
        houtiGoTitleCount = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        houtiGoTitleCount += Time.deltaTime;

        if (houtiGoTitleCount >= 30)
        {
            SceneManager.LoadScene("Prologue");
        }
    }
}
