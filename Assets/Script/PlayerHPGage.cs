using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// プレイヤーの体力管理クラス
/// </summary>
public class PlayerHPGage : MonoBehaviour
{
    [SerializeField, Tooltip("シーン内のプレイヤーを入れてね")]
    private Player player;

    private Slider redSlider;  //赤ゲージ
    private Slider greenSlider;//緑ゲージ

    private float maxHp;    //最大体力
    private float currentHp;//現在の体力
    private float saveValue;//一時保存体力

    // Start is called before the first frame update
    void Start()
    {
        redSlider = transform.GetChild(0).GetComponent<Slider>();
        greenSlider = transform.GetChild(1).GetComponent<Slider>();

        //なにか一つでもnullだったら処理しない
        if (!NullCheck()) return;

        maxHp = player.GetHp();
        redSlider.maxValue = greenSlider.maxValue = saveValue = maxHp;
        currentHp = player.GetHp();
    }

    // Update is called once per frame
    void Update()
    {
        SetHPGage();
    }

    /// <summary>
    /// 現在の体力をHPゲージに反映する
    /// </summary>
    void SetHPGage()
    {
        currentHp = player.GetHp();
        saveValue = player.GetSavevalue();
        greenSlider.value = currentHp;//現在の体力を入れる
        redSlider.value = saveValue;  //保存しておいた体力を入れる
    }

    /// <summary>
    /// nullじゃないかどうかをチェックする
    /// </summary>
    bool NullCheck()
    {
        if (redSlider == null)
        {
            Debug.Log("RedSliderの参照がありません");
            return false;
        }

        if (greenSlider == null)
        {
            Debug.Log("GreenSliderの参照がありません");
            return false;
        }

        if (player == null)
        {
            Debug.Log("Playerの参照がありません");
            return false;
        }

        return true;
    }
}
