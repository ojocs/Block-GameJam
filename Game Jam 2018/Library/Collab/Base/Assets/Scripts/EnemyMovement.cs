﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

    //To look at/react to player?
    public Transform Player;

    //Threshold to remain idle
    [SerializeField] float IdleDistance = 20f;
   
    //Speed
    public int MovementSpeed = 1;
    
    // Use this for initialization
    void Start () {
        facingRight = true;
    }
	
	// Update is called once per frame
	void Update () {
        //To stop
        var distance = Vector3.Distance(transform.position, Player.position);

        //Keep moving towards player until you're at his position
        if (distance < IdleDistance)
        {
            Vector3 playerDirection = (Player.transform.position - transform.position).normalized;
            transform.Translate(playerDirection.x * Time.deltaTime * MovementSpeed, 0, 0);
            
        }

        //if enemy is outside of bounds
        Flip(((Vector3)(Player.transform.position - transform.position)).x);
	}

    private bool facingRight;

    private void Flip(float horizontal)
    {
        if(horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
        {
            facingRight = !facingRight;

            Vector3 theScale = transform.localScale; 
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }
}
