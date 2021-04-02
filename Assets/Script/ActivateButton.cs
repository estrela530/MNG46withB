using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ActivateButton : MonoBehaviour
{

    //　最初にフォーカスするゲームオブジェクト
    [SerializeField]
    private GameObject firstSelect;

    public void ActivateOrNotActivate(bool flag)
    {
        for (var i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Button>().interactable = flag;
        }
        if (flag)
        {
            EventSystem.current.SetSelectedGameObject(firstSelect);
        }
    }
}
