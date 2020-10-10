using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Made by Cañadas Ortega, Fernando
 * 2º Desarrollo de aplicaciones multiplataformas, San José
 */

/// <summary>
/// This class is in charge of manage the player character movement and animations
/// </summary>
public class player : MonoBehaviour
{
    public float velocity = 4f;
    private new Animator animation;
    private Rigidbody2D rb2D;
    private Vector2 movement;
    public bool isTalking_or_isReading;
    public bool gamePaused;

    /// <summary>
    /// Start is called before the first frame update. Get from the scene the character rigidbody, animator and start some variables
    /// </summary>
    void Start()
    {
        isTalking_or_isReading = true;
        gamePaused = false;
        animation = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Update is called once per frame. Check if the player has pressed any movement key (W, A, S, D), if he has then change the character animation sprite,
    /// if the player is not pressing any movement key then the character stay still
    /// </summary>
    void Update()
    {
        if (!isTalking_or_isReading && !gamePaused)
        {
            movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            if (movement != Vector2.zero)
            {
                animation.SetFloat("movementX", movement.x);
                animation.SetFloat("movementY", movement.y);
                animation.SetBool("walking", true);
            }
            else
            {
                animation.SetBool("walking", false);
            }
        }
        else
        {
            animation.SetBool("walking", false);
            movement = Vector2.zero;
        }
    }

    /// <summary>
    /// Similar to update, FixedUpdate is used when you manage game physics. Check if the player has pressed any movement key (W, A, S, D), if he has then
    /// move the character in the desired direction
    /// </summary>
    void FixedUpdate()
    {
        rb2D.MovePosition(rb2D.position + (movement * velocity * Time.deltaTime));
    }
}
