using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Made by Cañadas Ortega, Fernando
 * 2º Desarrollo de aplicaciones multiplataformas, San José
 */

/// <summary>
/// This class is in charge of manage the player character size when the character move through hills
/// </summary>
public class PlayerHillHeight : MonoBehaviour
{
    private float xPastPosition;
    private float yPastPosition;
    public bool isMovingX;
    public float scaleChange;

    private BoxCollider2D walkArea;
    private Vector2 minWalkPoint;
    private Vector2 maxWalkPoint;
    private Camera mainCamera;
    public GameObject targetMap;

    /// <summary>
    /// Start is called before the first frame update. Get from the scene the BoxCollider of the hill, the boundaries of the BoxCollider and also the main camera of the scene
    /// </summary>
    private void Start()
    {
        walkArea = GetComponent<BoxCollider2D>();

        minWalkPoint = walkArea.bounds.min;
        maxWalkPoint = walkArea.bounds.max;

        mainCamera = Camera.main;
    }

    /// <summary>
    /// Change the character and camera size as you walk through the hill
    /// </summary>
    /// <param name="collision">Gameobject that enter the hill area</param>
    private void OnTriggerStay2D(Collider2D collision)
    {
        // Hills where you move from left to right and vice versa
        if (isMovingX)
        {
            float xCurrentPosition = collision.transform.position.x;
            // If you go from left to right
            if (xCurrentPosition > xPastPosition)
            {
                xPastPosition = xCurrentPosition;
                collision.transform.localScale += new Vector3(-scaleChange, -scaleChange, -scaleChange);
            }
            // If you go from right to left
            else if (xCurrentPosition < xPastPosition)
            {
                xPastPosition = xCurrentPosition;
                collision.transform.localScale += new Vector3(scaleChange, scaleChange, scaleChange);
            }
        }
        // Hills where you move from above to below and vice versa
        else
        {
            float yCurrentPosition = collision.transform.position.y;
            // If you go from below to above
            if (yCurrentPosition > yPastPosition)
            {
                yPastPosition = yCurrentPosition;
                collision.transform.localScale += new Vector3(scaleChange, scaleChange, scaleChange);
                mainCamera.orthographicSize += (scaleChange / 1.7f);

            }
            // If you go from above to below
            else if (yCurrentPosition < yPastPosition)
            {
                yPastPosition = yCurrentPosition;
                collision.transform.localScale += new Vector3(-scaleChange, -scaleChange, -scaleChange);
                mainCamera.orthographicSize -= (scaleChange / 1.8f);
            }

        }
    }

    /// <summary>
    /// Set a fixed size when you exit the hill and resize the camera boundaries
    /// </summary>
    /// <param name="collision">Gameobject that enter the hill area</param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Hills where you move from left to right and vice versa
        if (isMovingX)
        {
            xPastPosition = 0f;
            if (collision.transform.position.x > maxWalkPoint.x)
            {
                collision.transform.localScale = new Vector3(1.5249f, 1.5249f, 1.5249f);
            }
            else if (collision.transform.position.x < maxWalkPoint.x)
            {
                collision.transform.localScale = new Vector3(1.73269f, 1.73269f, 1.73269f);
            }
        }
        // Hills where you move from above to below and vice versa
        else
        {
            yPastPosition = 0f;

            if (collision.transform.position.y > maxWalkPoint.y)
            {
                collision.transform.localScale = new Vector3(3.654238f, 3.654238f, 3.654238f);
                mainCamera.orthographicSize = 10.2f;
            }
            else if (collision.transform.position.y < maxWalkPoint.y)
            {
                collision.transform.localScale = new Vector3(1.73269f, 1.73269f, 1.73269f);
                mainCamera.orthographicSize = 9;
            }
        }

        mainCamera.GetComponent<CameraMovement>().setCameraBounds(targetMap);
    }
}
