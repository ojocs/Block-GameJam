using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BashAttack : MonoBehaviour
{

    private Rigidbody2D rb;
    public float bashSpeed;
    private float bashTime;
    public float startBashTime;
    private bool bashCharge = true;
    private float bashCooldown;
    public float startBashCooldown;
    private float direction;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bashTime = startBashTime;
        direction = 0;
    }

    void FixedUpdate()
    {
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
            bashTime = startBashTime;
            bashCooldown = startBashCooldown;
        }
        else if (bashTime < startBashTime || Input.GetKeyDown("space") && bashCharge)
        {
            Debug.Log("space " + direction + " " + direction * bashSpeed);
            bashTime -= Time.deltaTime;
            rb.velocity = new Vector3(direction * bashSpeed, 0, 0);
            bashCharge = false;
        }
    }
}