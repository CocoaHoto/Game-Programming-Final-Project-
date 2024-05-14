using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public Sprite[] fireSpites;
    private float lastSpriteChangeTime;
    private float spriteChangeDelay = 0.1f; // Half-second delay between sprite changes
    private int spriteIndex = 0;
    GameObject boss;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastSpriteChangeTime = Time.time;
        boss = GameObject.FindGameObjectWithTag("Boss");
    }

    // Update is called once per frame
    void Update()
    {
        var bossPOS = boss.transform.position;
        var cameraPOS = transform.position;

        cameraPOS.x = bossPOS.x;
        cameraPOS.y = bossPOS.y;
        transform.position = cameraPOS;

        if (Time.time - lastSpriteChangeTime > spriteChangeDelay ) {
            spriteRenderer.sprite = fireSpites[spriteIndex];
            lastSpriteChangeTime = Time.time;
            spriteIndex++;

            if(spriteIndex == fireSpites.Length) {
                Destroy(gameObject);
            }
        }
    }
}
