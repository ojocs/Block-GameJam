using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    //For animations
    Animator ani;
    float dirx;
    public GameObject shield;
    
    bool dead;
    
    // keep some reference to the rigidbody
    private Rigidbody2D rb;

    // velocity's value can be edited in the unity editor
    [SerializeField] float velocity = 10f;

    // same for jumping
    [SerializeField] float jumpStrength = 500f;

    // track if touching ground
    private bool touchingGround = true;

    public float bashSpeed;
    private float bashTime;
    public float startBashTime;
    private bool bashCharge = true;
    private float bashCooldown;
    public float startBashCooldown;
    private float direction;
    private bool bash = false;

    private bool facingRight;
    
    private float life = 2;
    public GameObject lifeBlock2;
    public GameObject lifeBlock1;
    private float iFrames;
    public float startIFrames;

    // Sound
    public AudioClip dash;
    public AudioClip jump;
    public AudioClip damage;

    // Use this for initialization
    void Start()
    {
        dead = false;

        //Animation
        ani = GetComponent<Animator>();

        // find the rigidbody component on the gameobject that has this script
        rb = GetComponent<Rigidbody2D>();

        bashTime = startBashTime;
        direction = 0;

        //initial direction
        facingRight = true;

        // Sound
        GetComponent<AudioSource>().playOnAwake = false;
        GetComponent<AudioSource>().clip = dash;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!dead)
        {
            dirx = Input.GetAxisRaw("Horizontal") * velocity * Time.deltaTime;

            // set the player velocity to move the player
            // 		x will react to key presses because of Input.GetAxit()
            // 		y will be the previously set velocty
            //		z will be nothing since this is a 2D game
            rb.velocity = new Vector3(
                Input.GetAxis("Horizontal") * velocity, // x
                rb.velocity.y,                          // y
                0);                                     // z

            //Dashing animation
            if (bash)
            {
                ani.SetBool("dashing", true);
            }
            else
            {
                ani.SetBool("dashing", false);
            }

            //Walk animation
            if (dirx != 0 && !ani.GetCurrentAnimatorStateInfo(0).IsName("dashing"))//Moving and not dashing
            {
                ani.SetBool("isWalking", true);
            }
            else
            {
                ani.SetBool("isWalking", false);
            }

            // jump only if on ground AND jump key pressed
            if (touchingGround && Input.GetAxis("Vertical") > 0)
            {
                rb.AddForce(new Vector3(0, jumpStrength, 0));
                touchingGround = false;

                // Sound
                GetComponent<AudioSource>().clip = jump;
                GetComponent<AudioSource>().Play();
            }


            // bash code
            if (Input.GetAxis("Horizontal") != 0 && bashTime == startBashTime)
            {
                if (Input.GetAxis("Horizontal") > 0)
                    direction = 1;
                else
                    direction = -1;
            }

            if (bashCooldown > 0)
            {
                bashCooldown -= Time.deltaTime;
                if (bashCooldown <= 0)
                    bashCharge = true;
            }
            else if (bashTime <= 0)
            {
                bash = false;
                bashTime = startBashTime;
                bashCooldown = startBashCooldown;
            }
            else if (bashTime < startBashTime || Input.GetKeyDown("space") && bashCharge)
            {
                //       Debug.Log("space " + direction + " " + direction * bashSpeed);
                bashTime -= Time.deltaTime;
                rb.velocity = new Vector3(direction * bashSpeed, 0, 0);
                bashCharge = false;

                // Sound
                if (!bash)
                {
                    GetComponent<AudioSource>().clip = dash;
                    GetComponent<AudioSource>().Play();
                }
                bash = true;
            }

            float horizontal = Input.GetAxis("Horizontal");
            Flip(horizontal);

            if (iFrames > 0)
                iFrames -= Time.deltaTime;
        }
    }

    public float getDirection()
    {
        return direction;
    }

    public bool getBash()
    {
        return bash;
    }

    void OnTriggerEnter2D(Collider2D col)
    {   //If Enemy hurts you
        if(col.gameObject.CompareTag("Enemy"))
        {//If full life
            if(life == 2)
            {//Take some life
                life--;
                Destroy(lifeBlock2);
                iFrames = startIFrames;
            }
            else if(iFrames <= 0)//If no life left
            {//Kill Player
                life--;
                Destroy(lifeBlock1);
                dead = true;
                deadAni();
                gameOver();
            }

            // Sound
            GetComponent<AudioSource>().clip = damage;
            GetComponent<AudioSource>().Play();
        }
        
        touchingGround = true; // reset jump
    }

    private void deadAni()
    {
        //Dying animation
        if (life == 0)
        {
            Destroy(shield);
            ani.SetBool("dying", true);
        }
        else
        {
            ani.SetBool("dying", false);
        }
    }

    private void Flip(float horizontal)
    {
        if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
        {
            facingRight = !facingRight;

            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }

    public void gameOver()
    {
        //      Debug.Log("GAME OVER KIDDO");
        //reloads scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}