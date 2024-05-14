using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public string nextScene;
    public TMP_Text BossLivesText;
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;
    AudioSource src;
    public AudioClip slashSound;

    public Sprite[] upSprites;
    public Sprite[] downSprites;
    public Sprite[] leftSprites;
    public Sprite[] rightSprites;

    public Sprite[] calmUp;
    public Sprite[] calmDown;
    public Sprite[] calmLeft;
    public Sprite[] calmRight;

    public Sprite[] attackUp;
    public Sprite[] attackDown;
    public Sprite[] attackLeft;
    public Sprite[] attackRight;
    float speed = 5.0f;
    
    private PlayerHelper.PlayerSide playerSide;
    private PlayerHelper.PlayerState playerState;
    private float lastSpriteChangeTime;
    private float spriteChangeDelay = 0.2f; // Half-second delay between sprite changes


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastSpriteChangeTime = Time.time;
        playerSide = PlayerHelper.PlayerSide.DOWN;
        playerState = PlayerHelper.PlayerState.IDLE;
        src = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.W)){
            rb.velocity = new Vector2(0, speed);
            ChangeSprite(upSprites);
            playerSide = PlayerHelper.PlayerSide.UP;
        }
        else if(Input.GetKey(KeyCode.S)){
            rb.velocity = new Vector2(0, -speed);
            ChangeSprite(downSprites);
            playerSide = PlayerHelper.PlayerSide.DOWN;
        }
        else if(Input.GetKey(KeyCode.A)){
            rb.velocity = new Vector2(-speed, 0);
            ChangeSprite(leftSprites);
            playerSide = PlayerHelper.PlayerSide.LEFT;
        }
        else if(Input.GetKey(KeyCode.D)){
            rb.velocity = new Vector2(speed, 0);
            ChangeSprite(rightSprites);
            playerSide = PlayerHelper.PlayerSide.RIGHT;
        }
        else if(Input.GetKey(KeyCode.Space)){
            playerState = PlayerHelper.PlayerState.ATTACK;
            switch(playerSide){
                case PlayerHelper.PlayerSide.UP:
                useAttack(attackUp);
                break;
                case PlayerHelper.PlayerSide.DOWN:
                useAttack(attackDown);
                break;
                case PlayerHelper.PlayerSide.LEFT:
                useAttack(attackLeft);
                break;
                case PlayerHelper.PlayerSide.RIGHT:
                useAttack(attackRight);
                break;
            }
        }
        else {
            rb.velocity = Vector2.zero;
            switch(playerSide){
                case PlayerHelper.PlayerSide.UP:
                ChangeSprite(calmUp);
                break;
                case PlayerHelper.PlayerSide.DOWN:
                ChangeSprite(calmDown);
                break;
                case PlayerHelper.PlayerSide.LEFT:
                ChangeSprite(calmLeft);
                break;
                case PlayerHelper.PlayerSide.RIGHT:
                ChangeSprite(calmRight);
                break;
            }
            playerState = PlayerHelper.PlayerState.IDLE;
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

    void useAttack(Sprite[] sprites)
    {
        src.PlayOneShot(slashSound);
        for(int spriteIndex = 0; spriteIndex < sprites.Length; spriteIndex++) {
            spriteRenderer.sprite = sprites[spriteIndex];
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("AI") && playerState == PlayerHelper.PlayerState.ATTACK){
            Destroy(collision.gameObject);
        } 
        else if (collision.CompareTag("AI") && playerState == PlayerHelper.PlayerState.IDLE) {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Lose");
        }

        if(collision.CompareTag("Fire") && playerState == PlayerHelper.PlayerState.IDLE) {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Lose");
        }
        
        
        if(collision.CompareTag("Boss") && playerState == PlayerHelper.PlayerState.ATTACK){
            GameData.bossLives--;

            BossLivesText.text = "Boss Health points: " + GameData.bossLives.ToString();

            Debug.Log("Current BOSS HP: " + GameData.bossLives);

            if(GameData.bossLives <= 0){
                UnityEngine.SceneManagement.SceneManager.LoadScene(nextScene);
            }
        }
    }
}
