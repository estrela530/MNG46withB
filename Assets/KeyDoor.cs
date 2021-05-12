using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDoor : MonoBehaviour
{
    Key key;
    bool isGetKeyFlag;
    float fadeCount;
    [SerializeField, Header("Key")]
    GameObject keyObj;
    int closeTime = 5000;

    // Start is called before the first frame update 
    void Start()
    {
        key = keyObj.GetComponent<Key>();
        //this.isGetKeyFlag = key.GetKey();
        isGetKeyFlag = key.GetComponent<Key>().getKeyFlag;
        fadeCount = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //this.isGetKeyFlag = key.GetKey();
        isGetKeyFlag = key.getKeyFlag;

        if (!isGetKeyFlag)
        {
            return;
        }
        else if (isGetKeyFlag)
        {
            fadeCount++;
            if (fadeCount <= 240)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y-fadeCount / closeTime , transform.position.z);
            }
            else
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}
