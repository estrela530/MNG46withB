using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VloumeSlider : MonoBehaviour
{
    [SerializeField, Header("BGMスライダー")]
    public Slider bgmSlider;

    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        //コンポーネントゲッツ！
        audioSource = GetComponent<AudioSource>();
    }

   　public void VolumeMove()
    {
        //audioSource.volume = bgmSlider.value;
        
    }

    // Update is called once per frame
    void Update()
    {
        audioSource.volume = bgmSlider.GetComponent<Slider>().normalizedValue;
    }
}
