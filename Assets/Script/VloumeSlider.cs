using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VloumeSlider : MonoBehaviour
{
    [SerializeField, Header("BGMスライダー")]
    private Slider bgmSlider;
    
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        //コンポーネントゲッツ！
        audioSource = GetComponent<AudioSource>();
        bgmSlider = GetComponent<Slider>();
    }

   　public void VolumeMove()
    {
        //audioSource.volume = bgmSlider.value;
    }

    // Update is called once per frame
    void Update()
    {
        //audioSource.volume = bgmSlider.value;
    }
}
