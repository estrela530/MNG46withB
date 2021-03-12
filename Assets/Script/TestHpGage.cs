using UnityEngine;
using UnityEngine.UI;

public class TestHpGage : MonoBehaviour
{
    Slider redSlider;
    Slider greenSlider;
    float saveValue;
    int maxHp = 10; //これはプレイヤーから持ってくる。
    float currentHp;//これもプレイヤーから持ってくる

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
