using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DOTweenTest : MonoBehaviour
{
    Transform trans;
    Sequence sequence;

    public float time = 0;

    // Start is called before the first frame update
    void Start()
    {
        trans = this.transform;
        sequence = DOTween.Sequence();


        //trans.DOLocalJump(
        //    trans.position,
        //    2,
        //    1,
        //    1
        //    ).SetDelay(0.5f).SetLoops(-1);

        //縦に伸びるようになる
        sequence.Append(
            trans.DOScaleX(1.0f, time)
            );
        sequence.Join(
            trans.DOScaleY(1.2f, time)
            );
        sequence.Join(
            trans.DOScaleZ(1.0f, time)
            );

        //横につぶれたようになる。
        sequence.Append(
            trans.DOScaleX(1.2f, time)
            );
        sequence.Join(
            trans.DOScaleY(1.0f, time)
            );
        sequence.Join(
            trans.DOScaleZ(1.2f, time)
            );

        sequence.SetLoops(-1);

    }

    /// <summary>
    /// 移動っぽい動き(どちらかというと心臓の鼓動とかリズムのってるみたい)
    /// </summary>
    void MoveSequence()
    {
        //大きくする
        sequence.Append(
            trans.DOScale(1.2f, 0.1f)
            );
        //大きさを戻す
        sequence.Append(
            trans.DOScale(1.0f, 0.1f)
            );
        //ちょっと待つ
        sequence.SetDelay(0.5f);
        //ループさせる
        sequence.SetLoops(-1);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
