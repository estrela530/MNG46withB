using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class lifeDroplife : MonoBehaviour
{
    private GameObject Target;//追尾する相手

    private float dis;//プレイヤーとの距離

    Rigidbody rigid;
    [SerializeField, Tooltip("オブジェクトが飛んでいく力")]
    private float jumpPower = 18.0f;
    [SerializeField, Tooltip("オブジェクトの最大到達地点")]
    private float topHeightPoint = 5;
    [SerializeField, Tooltip("オブジェクトが沈んでいく力")]
    private float downPower = 3.0f;
    [SerializeField, Tooltip("オブジェクトの最低到達地点")]
    private float topLowPoint = 7f;

    public float stateTime;

    public int moveState;

    // Start is called before the first frame update
    void Start()
    {
        Target = GameObject.FindGameObjectWithTag("Player");
        rigid = GetComponent<Rigidbody>();
        moveState = 0;

        dis = Vector3.Distance(transform.position, Target.transform.position);//二つの距離を計算して一定以下になれば追尾
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        dis = Vector3.Distance(transform.position, Target.transform.position);//二つの距離を計算して一定以下になれば追尾
        switch (moveState)
        {
            case 0:
                //Y軸にも動けるようにした後、上に移動する
                rigid.constraints = RigidbodyConstraints.None;
                rigid.constraints = RigidbodyConstraints.FreezeRotation;
                rigid.AddForce(Vector3.up * jumpPower*3);
               

                if (this.transform.position.y > topHeightPoint)
                {
                    this.transform.position = new Vector3(this.transform.position.x, topHeightPoint, this.transform.position.z);
                }

                stateTime += Time.deltaTime;
                if (stateTime > 1)
                {
                    stateTime = 0;
                    moveState = 1;
                }

                break;

            case 1:
                // Y軸にも動けるようにした後、上に移動する
                

                if (this.transform.position.y <= topLowPoint)
                {
                    this.transform.position = new Vector3(this.transform.position.x, topLowPoint, this.transform.position.z);
                }

                stateTime += Time.deltaTime;
                if (stateTime > 3)
                {
                    stateTime = 0;
                    moveState = 2;
                }
                if(transform.position.y >=2 )
                {
                    rigid.constraints = RigidbodyConstraints.None;
                    rigid.constraints = RigidbodyConstraints.FreezeRotation;
                    rigid.AddForce(Vector3.down * downPower * 3);
                }

                break;

            case 2:
                transform.position = new Vector3(transform.position.x, 1, transform.position.z);
                break;
                
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene("Epilogue");
        }
        
    }
}
