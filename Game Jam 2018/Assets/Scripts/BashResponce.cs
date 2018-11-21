using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BashResponce : MonoBehaviour {
    public GameObject player;
    public float bashSpeed;
    private PlayerMovement pm;
    public AudioClip hit;

    void Start()
    {
        pm = player.GetComponent<PlayerMovement>();

        GetComponent<AudioSource>().playOnAwake = false;
        GetComponent<AudioSource>().clip = hit;
    }
    // Use this for initialization
    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.CompareTag("Enemy") && pm.getBash())
        {
            Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector3(pm.getDirection() * bashSpeed, 0, 0);

            //Sound
            GetComponent<AudioSource> ().Play();

        }
	}
}
