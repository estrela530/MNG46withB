using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkipGage : MonoBehaviour
{
    Image puni;
    [SerializeField, Header("スキップするまでのゲージ")]
    float zouka;

    // Start is called before the first frame update
    void Start()
    {
        puni = this.gameObject.GetComponent<Image>();
        puni.fillAmount = 0;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey("joystick button 4") || Input.GetKey(KeyCode.Z))
        {
            puni.fillAmount += zouka;
        }
        else
        {
            puni.fillAmount = 0;
        }
    }
}
