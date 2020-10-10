using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Made by Cañadas Ortega, Fernando
 * 2º Desarrollo de aplicaciones multiplataformas, San José
 */

/// <summary>
/// This class is in charge of manage the movement of the water lilies
/// </summary>
public class Water_Lilies_Movement : MonoBehaviour
{
    public float speedMovement;
    private Rigidbody2D water_Lily;

    private bool isWandering;

    public float timeToWander;
    private float timeWanderCounter;

    public float timeIdle;
    private float timeIdleCounter;

    private int actionSelected;
    private int lastAction = -1;

    /// <summary>
    /// Method that is called before the first frame update. Get the rigid body of the water lily and select an action
    /// </summary>
    void Start()
    {
        water_Lily = GetComponent<Rigidbody2D>();

        timeWanderCounter = timeToWander;
        timeIdleCounter = timeIdle;

        SelectAction();
    }

    /// <summary>
    /// This method is executed every frame, is used in game physics. Move the water lily in a direction or wait an idle time according to the selected action
    /// </summary>
    private void FixedUpdate()
    {
        // Move the water lily
        if (isWandering)
        {
            timeWanderCounter -= Time.deltaTime;

            switch (actionSelected)
            {
                case 0:
                    water_Lily.velocity = new Vector2(0, speedMovement);
                    break;

                case 1:
                    water_Lily.velocity = new Vector2(speedMovement, 0);
                    break;

                case 2:
                    water_Lily.velocity = new Vector2(0, -speedMovement);
                    break;

                case 3:
                    water_Lily.velocity = new Vector2(-speedMovement, 0);
                    break;
            }

            // Select another action when time is up
            if (timeWanderCounter < 0)
            {
                SelectAction();
            }
        }

        // Make the water lily stand still
        else
        {
            timeIdleCounter -= Time.deltaTime;
            water_Lily.velocity = Vector2.zero;

            // Select another action when time is up
            if (timeIdleCounter < 0)
            {
                SelectAction();
            }
        }
    }

    /// <summary>
    /// Select an action, the current action can not be same as the last action
    /// </summary>
    private void SelectAction()
    {
        actionSelected = Random.Range(0, 5);

        if (actionSelected == lastAction)
        {
            SelectAction();
        }

        lastAction = actionSelected;

        if (actionSelected != 4)
        {
            isWandering = true;
            timeWanderCounter = timeToWander;
        }
        else
        {
            isWandering = false;
            timeIdleCounter = timeIdle;
        }
    }
}
