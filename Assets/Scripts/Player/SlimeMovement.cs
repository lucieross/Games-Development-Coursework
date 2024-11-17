using UnityEngine;

public class CharacterController2D : MonoBehaviour {
    private Rigidbody2D rg;
    public float speed = 4.0f;
    public float jumpspeed = 6.0f;
    private bool isGrounded;
    public Transform groundCheck; // ground check
    public float groundCheckRadius = 0.2f; 
    public LayerMask groundLayer; 
    public Sprite normalSprite; // Original sprite
    public Sprite alternateSprite; // Sprite when hidden is held
    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer
    private Collider2D slimeCollider; 

    public Vector2 startingPosition; // starting position
    public GameObject upgradeMenu; 
    public GameObject StarParticles; // reference to star particles
    private bool isHidden; // Tracsk whether the player is hidden from enemies
    
    public bool isOver = false;

    void Start() {
        rg = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        slimeCollider = GetComponent<Collider2D>();
        startingPosition = transform.position;

        // Load jump and speed values
        jumpspeed = PlayerPrefs.GetFloat("PlayerJump", jumpspeed);
        speed = PlayerPrefs.GetFloat("PlayerSpeed", speed);

        
    }   

    void Update() {

        if (isOver) {
            rg.velocity = Vector2.zero; // Stop all movement
            return; 
        }
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        float straffe = Input.GetAxis("Horizontal") * speed;

        // Prevent moving left if at the starting position
        if (transform.position.x <= startingPosition.x && straffe < 0) {
            straffe = 0;
        }

        if ((Input.GetKey("s") || Input.GetKey(KeyCode.DownArrow)) && isGrounded) {
            isHidden = true; //makes hidden
            spriteRenderer.sprite = alternateSprite;
            rg.velocity = new Vector2(0, 0); // cant move while hiding
            slimeCollider.isTrigger = true; 
            rg.gravityScale = 0; // Prevent falling through floor
        } else {
            isHidden = false; //does the oposite
            spriteRenderer.sprite = normalSprite;
            slimeCollider.isTrigger = false; 
            rg.velocity = new Vector2(straffe, rg.velocity.y); // allow normal movement
            rg.gravityScale = 1; 
        }

        // Jump logic
        if (isGrounded && Input.GetButtonDown("Jump")) {
            rg.velocity = new Vector2(rg.velocity.x, jumpspeed);
        }
    }

    public bool IsHidden() {
        return isHidden; // Method to check hidden state
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("FallDetector")) {
            // Respawns at the starting position
            transform.position = startingPosition; 
            rg.velocity = Vector2.zero; 

            // Heart lost
            HeartManager.health--;
            if (HeartManager.health <= 0) {
                PlayerManager.isGameOver = true;
                gameObject.SetActive(false);
            }

            // Reset all falling platforms
            ResetAllPlatforms();
        }   

        if (other.CompareTag("Star")) {
            OpenUpgradeMenu();
            Destroy(other.gameObject); // Destroys the star after collecting it
        }
    }

    void OpenUpgradeMenu() {
        upgradeMenu.SetActive(true); // Show the upgrade menu
    }

    public void UpgradeJump() {
        jumpspeed += 0.2f; // Increases jump height by 0.2
        PlayerPrefs.SetFloat("PlayerJump", jumpspeed);
        CloseUpgradeMenu(); // Closes the menu after choosing
    }

    public void UpgradeSpeed() {
        speed += 0.2f; // Increase speed by 0.2
        PlayerPrefs.SetFloat("PlayerSpeed", speed);
        CloseUpgradeMenu(); // Close the menu
    }

    void CloseUpgradeMenu() {
        upgradeMenu.SetActive(false); // Hide the menus 
        Instantiate(StarParticles, transform.position, Quaternion.identity); // Particles
        AudioManager.instance.Play("Sparkle"); //sounds
    }

    private void ResetAllPlatforms() {
        FallingPlatform[] platforms = FindObjectsOfType<FallingPlatform>();
        foreach (FallingPlatform platform in platforms) {
            platform.ResetPlatform();
        }
    }
}
