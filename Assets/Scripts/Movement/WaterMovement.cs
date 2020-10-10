using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Made by Cañadas Ortega, Fernando
 * 2º Desarrollo de aplicaciones multiplataformas, San José
 */

/// <summary>
/// This class is in charge of manage the water animation
/// </summary>
public class WaterMovement : MonoBehaviour
{
    private new Animator animation;
    public int maxAnimations;

    /// <summary>
    /// Function that is called right after the scene is loaded and set an animation to the water block according to the number of water animations
    /// </summary>
    void Awake()
    {
        animation = GetComponent<Animator>();
        
        if (maxAnimations == 5)
        {
            animation.SetInteger("waterMovement", Random.Range(1, 6));
        }
        else if (maxAnimations == 8)
        {
            animation.SetInteger("waterMovement", Random.Range(1, 9));
        }
    }
}
