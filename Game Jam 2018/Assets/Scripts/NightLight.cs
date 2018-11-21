using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightLight : MonoBehaviour
{
    Animator ani;

    SpriteRenderer render;
    public Sprite NightLightOn;
    void OnTriggerEnter2D(Collider2D other)
    {
        ani.SetBool("on", true);
    }
}