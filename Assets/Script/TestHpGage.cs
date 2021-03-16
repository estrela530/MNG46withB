using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// プレイヤーからHPを受け取り、ゲージに反映する。
/// </summary>
public class TestHpGage : MonoBehaviour
{
    Slider redSlider;  //赤ゲージ
    Slider greenSlider;//緑ゲージ
    float saveValue;   //一次保存用体力
    int maxHp = 10;    //これはプレイヤーから持ってくる。
    float currentHp;   //これもプレイヤーから持ってくる

    Player player;

    // Start is called before the first frame update
    void Start()
    {
        redSlider   = transform.GetChild(0).GetComponent<Slider>();
        greenSlider = transform.GetChild(1).GetComponent<Slider>();

        player = GameObject.Find("Player").GetComponent<Player>();

        redSlider.maxValue = greenSlider.maxValue = saveValue = maxHp;
        currentHp = player.GetHp();
    }

    // Update is called once per frame
    void Update()
    {
        currentHp = player.GetHp();

        greenSlider.value = currentHp;
        redSlider.value = currentHp;
    }
}
