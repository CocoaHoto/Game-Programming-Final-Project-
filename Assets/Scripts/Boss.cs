using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Boss : MonoBehaviour
{
    public TMP_Text BossLivesText;
    public TMP_Text BossText;
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;
    public Sprite[] upSprites;
    public Sprite[] downSprites;
    public Sprite[] leftSprites;
    public Sprite[] rightSprites;

    AudioSource src;
    public AudioClip fireSound;

    float speed = 2.0f;
    public float threshold = 50.0f;
    private float lastSpriteChangeTime;
    private float lastFireChangeTime;
    private float spriteChangeDelay = 0.2f; // Half-second delay between sprite changes
    private float fireDelayTime = 6f;
    AIHelper.State state = AIHelper.State.IDLE;
    GameObject player;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastSpriteChangeTime = Time.time;
        lastFireChangeTime = Time.time;
        player = GameObject.FindGameObjectWithTag("Player");
        GameData.bossLives = 6;
        src = GetComponent<AudioSource>();
    }

    public Tilemap tilemap;
    public Tilemap tilemap2;
    public Tilemap tilemap3;

    // Update is called once per frame
    void Update()
    {
        if(state == AIHelper.State.IDLE){
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if(distance < threshold){
                BossLivesText.color = Color.red;
                BossText.color = Color.red;

                // Set the tilemap material color to a redder shade based on the current red value
                Color newColor = new Color( tilemap.color.r / 2 , tilemap.color.g / 2,  tilemap.color.b * 2);
                tilemap.color = newColor;
                tilemap2.color = newColor;
                tilemap3.color = newColor;

                
                state = AIHelper.State.CHASE;
            }
        } else if(state == AIHelper.State.CHASE) {
            var direction = player.transform.position - transform.position;
            direction = direction.normalized;

            if(Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) {
                if(direction.x > 0) {
                    ChangeSprite(rightSprites);
                }
                else if(direction.x < 0) {
                    ChangeSprite(leftSprites);
                }

                var change = new Vector3(direction.x, 0, 0);
                rb.velocity = change * speed;
            } 
            else if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y)) {
                if(direction.y > 0) {
                    ChangeSprite(upSprites);
                }
                else if(direction.y < 0) {
                    ChangeSprite(downSprites);
                }

                var change = new Vector3(0, direction.y, 0);
                rb.velocity = change * speed;
            }

            float distance = Vector3.Distance(transform.position, player.transform.position);
            if(distance < 6){
                if (Time.time - lastFireChangeTime > fireDelayTime ) {
                    if(!GameObject.FindGameObjectWithTag("Fire")) {
                        fireAttack();
                    }
                    lastFireChangeTime = Time.time;
                }
            }

        } 
        
    }

    void ChangeSprite(Sprite[] sprites)
    {
        if (Time.time - lastSpriteChangeTime > spriteChangeDelay ) {
            // Change sprite based on the movement direction
            int spriteIndex = Random.Range(0, sprites.Length);
            spriteRenderer.sprite = sprites[spriteIndex];
            lastSpriteChangeTime = Time.time;

        }
    }

    public GameObject Fire;
    void fireAttack() {
        src.PlayOneShot(fireSound);
        Instantiate(Fire, transform.position, Quaternion.identity);
    }
}
