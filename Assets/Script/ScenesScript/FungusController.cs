using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class FungusController : MonoBehaviour
{
    void LoadScene()
    {
        SceneManager.LoadScene("GameClear");
    }
}
