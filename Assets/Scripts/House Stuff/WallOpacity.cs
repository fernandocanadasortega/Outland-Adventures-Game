using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Made by Cañadas Ortega, Fernando
 * 2º Desarrollo de aplicaciones multiplataformas, San José
 */

/// <summary>
/// This class is in charge of load fade the house walls when you come near to them and make the opaque when you go away
/// </summary>
public class WallOpacity : MonoBehaviour
{
    public GameObject wall;
    public GameObject windows;
    public GameObject windows2;

    /// <summary>
    /// Fade away the wall
    /// </summary>
    /// <param name="collision">Gameobject that enter the house wall area</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        wall.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, .3f);
        if (windows != null && windows2 != null)
        {
            windows.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, .7f);
            windows2.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, .7f);
        }
    }

    /// <summary>
    /// Reappear the wall
    /// </summary>
    /// <param name="collision">Gameobject that enter the house wall area</param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        wall.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        if (windows != null && windows2 != null)
        {
            windows.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
            windows2.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        }
    }
}
