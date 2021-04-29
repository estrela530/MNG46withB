using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectButton : MonoBehaviour
{
    [SerializeField] private Button[] buttons;
    [SerializeField] private int firstSelectButton;
    [SerializeField]
    //　ポーズした時に表示するUIのプレハブ
    private GameObject PauseUIPrefab;
    Pause pause;

    //一度だけ呼び出す用のbool
    public bool callSet;

    // Start is called before the first frame update
    void Start()
    {
        buttons[firstSelectButton].Select();
        pause = PauseUIPrefab.GetComponent<Pause>();

        callSet = false;
    }

    void Update()
    {

    }

    //わっしょいするメソッド
   　public void Wasshoi()
    {
        buttons[firstSelectButton].Select();
    }
}
