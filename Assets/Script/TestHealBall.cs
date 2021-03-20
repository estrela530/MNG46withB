using System.Collections;
using UnityEngine;

public class TestHealBall : MonoBehaviour
{
    [SerializeField, Header("レベルアップに必要な時間(成長は2回,消えそう,消える)")]
    private float[] levelUpTime = new float[4];

    int count = 0;    //時間計測用
    int healLevel = 1;//回復レベル

    MeshRenderer meshRenderer;

    /// <summary>
    /// 状態
    /// </summary>
    private enum State
    {
        Level1,  //初期状態
        Level2,  //1回成長
        Level3,  //2回成長
        Blinking,//点滅
        Death    //死亡
    }State state = State.Level1;

    // Start is called before the first frame update
    void Start()
    {
        //60かけて秒にする。
        for(int i = 0;i<levelUpTime.Length;i++)
        {
            levelUpTime[i] *= 60.0f;
        }

        count = 0;
        healLevel = 0;

        meshRenderer = GetComponent<MeshRenderer>();

        ////テスト↓ : 色変え
        //GetComponent<Renderer>().material.color = Color.yellow;
    }

    void FixedUpdate()
    {
        ChangeState();
        SetAction();
    }

    void ChangeState()
    {
        count++;//値を増やし続ける～

        if (count >= levelUpTime[0])
        {
            if (count >= levelUpTime[3]) state = State.Death;
            else if (count >= levelUpTime[2]) state = State.Blinking;
            else if (count >= levelUpTime[1]) state = State.Level3;
            else state = State.Level2;
        }
        else state = State.Level1;
    }

    void SetAction()
    {
        switch (state)
        {
            case State.Level1:
                healLevel = 1;
                meshRenderer.material.color = Color.yellow;
                transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                break;
            case State.Level2:
                healLevel = 2;
                meshRenderer.material.color = Color.green;
                transform.localScale = new Vector3(1, 1, 1);
                break;
            case State.Level3:
                healLevel = 3;
                meshRenderer.material.color = Color.black;
                transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                break;
            case State.Blinking:
                //点滅する
                StartCoroutine("Blinking");
                break;
            case State.Death:
                Destroy(this.gameObject);
                break;
            default:
                Debug.Log("存在しない状態に切り替わっています。");
                break;
        }
    }

    IEnumerator Blinking()
    {
        while(true)
        {
            meshRenderer.material.color = Color.red;
            yield return new WaitForSeconds(2.0f);

            //yield return new WaitForEndOfFrame();
        }
    }

    /// <summary>
    /// このオブジェクトがDestroyされたときに呼ばれる。
    /// </summary>
    private void OnDestroy()
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        DestroyImmediate(renderer.material);//マテリアルのメモリを削除
        //System.GC.Collect();
        //Resources.UnloadUnusedAssets();
    }

    /// <summary>
    /// 回復レベルを取得
    /// </summary>
    /// <returns></returns>
    public int GetHealLevel()
    {
        return healLevel;
    }
}
