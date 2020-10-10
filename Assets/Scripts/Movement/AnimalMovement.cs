using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Made by Cañadas Ortega, Fernando
 * 2º Desarrollo de aplicaciones multiplataformas, San José
 */

/// <summary>
/// This class is in charge of manage the movement and the animation of the animals
/// </summary>
public class AnimalMovement : MonoBehaviour
{
    public float speedMovement = 2f;
    private Rigidbody2D animal;
    private new Animator animation;

    private bool isWalking;
    public float timeToWalk;
    private float timeWalkCounter;

    private bool isEating;
    public float timeToEat;
    private float timeEatCounter;

    public float timeIdle;
    private float timeIdleCounter;

    public string currentAnimal;

    private int actionSelected;
    private int lastAction = -1;

    /// <summary>
    /// Method that is called before the first frame update. Get the rigid body and the animator of the animal and select an action
    /// </summary>
    void Start()
    {
        animal = GetComponent<Rigidbody2D>();
        animation = GetComponent<Animator>();

        // Set the counters values
        timeWalkCounter = timeToWalk;
        timeEatCounter = timeToEat;
        timeIdleCounter = timeIdle;

        SelectAction();
    }

    /// <summary>
    /// This method is executed every frame, is used in game physics. Move the animal in a direction, wait an idle time or make the animal eat
    /// according to the selected action
    /// </summary>
    void FixedUpdate()
    {
        // Move the animal
        if (isWalking)
        {
            timeWalkCounter -= Time.deltaTime;

            switch (actionSelected)
            {
                case 0:
                    animal.velocity = new Vector2(0, speedMovement);
                    animation.SetFloat("movementX", 0);
                    animation.SetFloat("movementY", 1);
                    break;

                case 1:
                    animal.velocity = new Vector2(speedMovement, 0);
                    animation.SetFloat("movementX", 1);
                    animation.SetFloat("movementY", 0);
                    break;

                case 2:
                    animal.velocity = new Vector2(0, -speedMovement);
                    animation.SetFloat("movementX", 0);
                    animation.SetFloat("movementY", -1);
                    break;

                case 3:
                    animal.velocity = new Vector2(-speedMovement, 0);
                    animation.SetFloat("movementX", -1);
                    animation.SetFloat("movementY", 0);
                    break;
            }

            // Select another action when time is up
            if (timeWalkCounter < 0)
            {
                isWalking = false;
                animation.SetBool("isWalking", false);
                timeIdleCounter = timeIdle;
            }
        }

        // Make the animal eat
        else if (isEating)
        {
            timeEatCounter -= Time.deltaTime;

            // Select another action when time is up
            if (timeEatCounter < 0)
            {
                isEating = false;
                animation.SetBool("isEating", false);
                timeIdleCounter = timeIdle;
            }
        }

        // Make the animal stand still
        else
        {
            timeIdleCounter -= Time.deltaTime;
            animal.velocity = Vector2.zero;

            // Select another action when time is up
            if (timeIdleCounter < 0)
            {
                SelectAction();
            }
        }
    }

    /// <summary>
    /// Select an action, the current action can not be same as the last action. Also modify the box collider of the animal according to the direction the 
    /// animal is looking
    /// </summary>
    private void SelectAction()
    {
        if (lastAction == -1)
        {
            actionSelected = Random.Range(0, 4);
        }
        else
        {
            actionSelected = Random.Range(0, 5);
        }

        if (actionSelected == lastAction)
        {
            SelectAction();
        }

        lastAction = actionSelected;

        // Enter if the action is eat
        if (actionSelected == 4)
        {
            isEating = true;
            animation.SetBool("isEating", true);
            timeEatCounter = timeToEat;
        }

        // Enter if the action is move
        else if (actionSelected < 4)
        {
            if (currentAnimal.Equals("cow"))
            {
                // Cow looking up
                if (actionSelected == 0)
                {
                    gameObject.GetComponent<BoxCollider2D>().offset = new Vector2((float)-0.004600048, (float)-0.00500679);
                    gameObject.GetComponent<BoxCollider2D>().size = new Vector2((float)0.7833929, (float)1.954651);
                }
                // Cow looking down
                else if (actionSelected == 2)
                {
                    gameObject.GetComponent<BoxCollider2D>().offset = new Vector2((float)-0.004600048, (float)-0.01672459);
                    gameObject.GetComponent<BoxCollider2D>().size = new Vector2((float)0.7833929, (float)1.5912);
                }
                // Cow looking right or left
                else if (actionSelected == 1 || actionSelected == 3)
                {
                    gameObject.GetComponent<BoxCollider2D>().offset = new Vector2((float)-0.004600286, (float)-0.01017261);
                    gameObject.GetComponent<BoxCollider2D>().size = new Vector2((float)1.941573, (float)1.221071);
                }
            }

            else if (currentAnimal.Equals("sheep"))
            {
                // Sheep looking up or down
                if (actionSelected == 0 || actionSelected == 2)
                {
                    gameObject.GetComponent<BoxCollider2D>().offset = new Vector2((float)0.000535965, (float)0.008228779);
                    gameObject.GetComponent<BoxCollider2D>().size = new Vector2((float)0.7930355, (float)1.35366);
                }
                // Sheep looking right or left
                else if (actionSelected == 1 || actionSelected == 3)
                {
                    gameObject.GetComponent<BoxCollider2D>().offset = new Vector2((float)-0.01107508, (float)-0.003372312);
                    gameObject.GetComponent<BoxCollider2D>().size = new Vector2((float)1.373578, (float)1.09844);
                }
            }

            else if (currentAnimal.Equals("pig"))
            {
                // Pig looking up or down
                if (actionSelected == 0 || actionSelected == 2)
                {
                    gameObject.GetComponent<BoxCollider2D>().offset = new Vector2((float)-0.0005071163, (float)0.006098747);
                    gameObject.GetComponent<BoxCollider2D>().size = new Vector2((float)0.591022, (float)1.305269);
                }
                // Pig looking right or left
                else if (actionSelected == 1 || actionSelected == 3)
                {
                    gameObject.GetComponent<BoxCollider2D>().offset = new Vector2((float)-0.0005073547, (float)0.00609827);
                    gameObject.GetComponent<BoxCollider2D>().size = new Vector2((float)1.549158, (float)0.8266077);
                }
            }

            else if (currentAnimal.Equals("chicken"))
            {
                // Chicken looking up or down
                if (actionSelected == 0 || actionSelected == 2)
                {
                    gameObject.GetComponent<BoxCollider2D>().offset = new Vector2((float)-0.001365662, (float)-0.001221001);
                    gameObject.GetComponent<BoxCollider2D>().size = new Vector2((float)0.5481577, (float)0.7307752);
                }
                // Chicken looking right or left
                else if (actionSelected == 1 || actionSelected == 3)
                {
                    gameObject.GetComponent<BoxCollider2D>().offset = new Vector2((float)0.002295494, (float)-0.001221001);
                    gameObject.GetComponent<BoxCollider2D>().size = new Vector2((float)0.8923054, (float)0.7307752);
                }
            }

            isWalking = true;
            animation.SetBool("isWalking", true);
            timeWalkCounter = timeToWalk;
        }
    }
}