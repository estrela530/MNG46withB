using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private GameObject player;
    [SerializeField]
    private Vector3 offsetPos = new Vector3(2.5f, 10f, -5);
    [SerializeField]
    private Vector3 offsetRot = new Vector3(60, -30, 0);

    Vector3 position;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");

        position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        position = transform.position;

        position = player.transform.position + offsetPos;

        position.y = Mathf.Clamp(position.y, -10, 15);

        //ここに値を入れておく
        transform.position = position;

        //回転角度はこれで固定かな...
        this.transform.eulerAngles = offsetRot;
    }
}
