using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{

    private Rigidbody2D rd2d;

    public float speed;
    public AudioClip winMusic;
    public AudioClip bgMusic;
    public AudioSource musicSource; 

    private bool gameOver;
    private bool facingRight = true;
    private bool isOnGround;
    public Transform groundcheck;
    public float checkRadius;
    public LayerMask allGround;

    public Text score;
    public Text winText;
    public Text livesText;
    

    Animator anim;

    private int scoreValue = 0;
    private int livesValue = 3;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        score.text = "Score: " + scoreValue.ToString();
        winText.text = "";
        livesText.text = "Lives: " + livesValue.ToString();
        gameOver = false;
        musicSource.clip = bgMusic;
        musicSource.loop = true;
        musicSource.Play();
        anim = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            anim.SetBool("run", true);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            anim.SetBool("run", false);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            anim.SetBool("run", true);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            anim.SetBool("run", false);
        }
        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);
        if (Input.GetKey(KeyCode.W) && isOnGround ==false)
        {
            anim.SetBool("jump", true);
        }
 
    }
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float verMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, verMovement * speed));
        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }
    }
    
    void Flip()
    {
     facingRight = !facingRight;
     Vector2 Scaler = transform.localScale;
     Scaler.x = Scaler.x * -1;
     transform.localScale = Scaler;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = "Score: " + scoreValue.ToString();
            Destroy(collision.collider.gameObject);
            
            if (scoreValue == 4)
            {
                transform.position = new Vector3(118.0f, 27.0f, 0.0f);
                livesValue = 3;
                livesText.text = "Lives: " + livesValue.ToString();
            }
            if (scoreValue >= 8)
            {
                gameOver = true;
            }
            if (gameOver == true)
            {
                winText.text = "You win! Game created by Kelcie Rushing";
                musicSource.Stop();
                musicSource.loop = false;
                musicSource.clip = winMusic;
                musicSource.Play();
            }
        }
        
        else if(collision.collider.tag == "Enemy")
        {
            livesValue = livesValue - 1;
            livesText.text = "Lives: " + livesValue.ToString();
            Destroy(collision.collider.gameObject);
            if (livesValue == 0)
            {
                winText.text = "Sorry! You Lose!";
                Destroy(this);
            }
        }
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.tag == "Ground" && isOnGround)
        {
            anim.SetBool("jump", false);
            if(Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
            }
        }
    }
}
