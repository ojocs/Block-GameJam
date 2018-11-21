using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //For animations
    Animator ani;
    float dirx;

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

    // Use this for initialization
    void Start()
    {
        //Animation
        ani = GetComponent<Animator>();

        // find the rigidbody component on the gameobject that has this script
        rb = GetComponent<Rigidbody2D>();

        bashTime = startBashTime;
        direction = 0;

        //initial direction
        facingRight = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //For walk animation, telling if Player is moving
        dirx = Input.GetAxisRaw("Horizontal") * velocity * Time.deltaTime;

        // set the player velocity to move the player
        // 		x will react to key presses because of Input.GetAxit()
        // 		y will be the previously set velocty
        //		z will be nothing since this is a 2D game
        rb.velocity = new Vector3(
            Input.GetAxis("Horizontal") * velocity, // x
            rb.velocity.y,                          // y
            0);                                     // z

        //Walk animation
        if (dirx != 0 && !ani.GetCurrentAnimatorStateInfo(0).IsName("jumping"))
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
            ani.SetBool("jumping", true);
            rb.AddForce(new Vector3(0, jumpStrength, 0));
            touchingGround = false;
        }
        else if(touchingGround)//So that jumping animation can last until Player touches ground
        {
            ani.SetBool("jumping", false);
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
            bash = true;
     //       Debug.Log("space " + direction + " " + direction * bashSpeed);
            bashTime -= Time.deltaTime;
            rb.velocity = new Vector3(direction * bashSpeed, 0, 0);
            bashCharge = false;
        }

        float horizontal = Input.GetAxis("Horizontal");
        Flip(horizontal);
    }

    public float getDirection()
    {
        return direction;
    }

    public bool getBash()
    {
        return bash;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        touchingGround = true; // reset jump
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

}