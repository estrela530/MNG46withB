using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public bool getKeyFlag;
    [SerializeField, Header("ドア")]
    GameObject door;

    // Start is called before the first frame update
    void Start()
    {
        getKeyFlag = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    }

    void OnTriggerEnter(Collider collision)
    {
        if (getKeyFlag)
        {
            return;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            getKeyFlag = true;
            this.gameObject.SetActive(false);
        }
    }


    public bool GetKey()
    {
        return getKeyFlag;
    }
}
