using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blood : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public Sprite[] bloodSpites;
    private float lastSpriteChangeTime;
    private float spriteChangeDelay = 20.0f;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastSpriteChangeTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastSpriteChangeTime > spriteChangeDelay ) {
            Destroy(gameObject);
        }
    }
}
