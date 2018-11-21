using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillCode : MonoBehaviour {

    //kill anything out of bounds
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(other.gameObject);
            other.gameObject.GetComponent<PlayerMovement>().gameOver();

        }
        else if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Block"))
        {
            Destroy(other.gameObject);
        }
    }
}
