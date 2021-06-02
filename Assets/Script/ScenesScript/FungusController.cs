using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class FungusController : MonoBehaviour
{
    public GameObject obj;

    private void Start()
    {
        DontDestroyOnLoad(obj);
    }

    void LoadScene()
    {
        SceneManager.LoadScene("GameClear");
    }
}
