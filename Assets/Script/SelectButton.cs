using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectButton : MonoBehaviour
{
    [SerializeField] private Button[] buttons;
    [SerializeField] private int firstSelectButton;

    // Start is called before the first frame update
    void Start()
    {
        buttons[firstSelectButton].Select();
    }
}
