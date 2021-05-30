using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageOnOff : MonoBehaviour
{
    [SerializeField, Header("点滅速度")]
    public float blinkSpeed = 5.0f;

    SpriteRenderer sprite;
    private float time;
    private float alpha;

    // Start is called before the first frame update
    void Start()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        time = Time.time * blinkSpeed;
        sprite.color = new Color(1, 1, 1, Mathf.Sin(time) * 0.5f + 0.5f);
    }
}
