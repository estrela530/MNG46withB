using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// プレイヤーの体力管理クラス
/// </summary>
public class PlayerHPGage : MonoBehaviour
{
    [SerializeField, Header("シーン内のプレイヤーを入れてね")]
    private Player player;
    [SerializeField, Tooltip("ダメージ時の揺らす強さ")]
    private int shakePower;

    private Slider redSlider;    //赤ゲージ
    private Slider greenSlider;  //緑ゲージ
    private Vector3 initPosition;//元の位置

    private float maxHp;    //最大体力
    private float currentHp;//現在の体力
    private float saveValue;//一時保存体力
   
    
    private bool isShake;//画面を揺らすかどうか

    // Start is called before the first frame update
    void Start()
    {
        redSlider = transform.GetChild(0).GetComponent<Slider>();
        greenSlider = transform.GetChild(1).GetComponent<Slider>();

        //なにか一つでもnullだったら処理しない
        if (!NullCheck()) return;

        maxHp = player.GetHp();
        redSlider.maxValue = greenSlider.maxValue = saveValue = currentHp = maxHp;

        //初期位置を保存
        initPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        SetHPGage();
        ShakeHpGage(shakePower);
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
    /// ダメージを受けた際、HPゲージを揺らします。
    /// </summary>
    /// <param name="shakePower"></param>
    void ShakeHpGage(int shakePower)
    {
        isShake = player.GetDamageFlag();

        transform.position = initPosition;

        if (!isShake) return;

        //ランダムに揺らす
        transform.position = initPosition + Random.insideUnitSphere * shakePower;
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
