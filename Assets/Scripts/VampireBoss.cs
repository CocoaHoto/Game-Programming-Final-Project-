using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class VampireBoss : MonoBehaviour
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
    public AudioClip ligthingSound;
    public AudioClip bloodSound;

    float speed = 3.5f;
    public float threshold = 20.0f;
    private float lastSpriteChangeTime;
    private float lastFireChangeTime;
    private float spriteChangeDelay = 0.2f; // Half-second delay between sprite changes
    private float fireDelayTime = 1.5f;
    AIHelper.State state = AIHelper.State.IDLE;
    GameObject player;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastSpriteChangeTime = Time.time;
        lastFireChangeTime = Time.time;
        player = GameObject.FindGameObjectWithTag("Player");
        GameData.bossLives = 10;
        src = GetComponent<AudioSource>();
    }

    public Tilemap tilemap;
    public Tilemap tilemap2;
    public Tilemap tilemap3;

    public GameObject lightning;
    void callLightning() {
        Instantiate(lightning, transform.position, Quaternion.identity);
    }


    // Update is called once per frame
    void Update()
    {
        if(GameData.bossLives < 2 && GameData.revive > 0) {
            //make it stop
            var change = new Vector3(0, 0, 0);
            rb.velocity = change * speed;

            //lighning

            src.PlayOneShot(ligthingSound);

            callLightning();

            GameData.bossLives = 10;

            GameData.revive--;

            BossLivesText.text = "Boss Health points: " + GameData.bossLives.ToString();

            Debug.Log("Current BOSS HP: " + GameData.bossLives);

            //change state to idle
            state = AIHelper.State.IDLE;
        }
        if(state == AIHelper.State.IDLE){
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if(distance < threshold){
                BossLivesText.color = Color.cyan;
                BossText.color = Color.cyan;

                // Set the tilemap material color to a redder shade based on the current red value
                Color newColor = new Color( tilemap.color.r * 3 , tilemap.color.g / 2,  tilemap.color.b / 2);
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
                    src.PlayOneShot(bloodSound);
                    bloodLine();
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

    public GameObject Blood;
    void bloodLine() {
        Instantiate(Blood, transform.position, Quaternion.identity);
    }
}
