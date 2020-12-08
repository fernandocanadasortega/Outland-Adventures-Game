using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is in charge of manage the movement and the animation of the villagers
/// </summary>
public class VillagerMovement : MonoBehaviour
{
    public float speedMovement;
    private Rigidbody2D villager;
    private Animator animator;
    public Collider2D walkArea;

    private bool isWalking;
    public bool isTalking;
    private bool isIdle;
    public bool isAboutToTalk;
    public int talkingDirection;

    public float timeToWalk;
    public float timeWalkingCounter;

    public float timeIdle;
    private float timeIdleCounter;

    private int actionSelected;
    private int lastAction = -1;

    private Vector2 minWalkPoint;
    private Vector2 maxWalkPoint;
    private bool hasWalkArea;

    /// <summary>
    /// Method that is called before the first frame update. Get the rigid body and the animator of the villager and select an action
    /// </summary>
    void Start()
    {
        villager = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        timeWalkingCounter = timeToWalk;
        timeIdleCounter = timeIdle;

        if (walkArea != null)
        {
            minWalkPoint = walkArea.bounds.min;
            maxWalkPoint = walkArea.bounds.max;
            hasWalkArea = true;
        }

        SelectAction(7);
    }

    /// <summary>
    /// This method is executed every frame, is used in game physics. Move the villager in a direction or wait an idle time
    /// according to the selected action
    /// </summary>
    private void FixedUpdate()
    {
        if (isWalking)
        {
            timeWalkingCounter -= Time.deltaTime;

            switch (actionSelected)
            {
                case 0:
                    villager.velocity = new Vector2(0, speedMovement);
                    animator.SetFloat("movementX", 0);
                    animator.SetFloat("movementY", 1);

                    if (hasWalkArea && transform.position.y > maxWalkPoint.y)
                    {
                        timeWalkingCounter = 0;
                    }
                    break;

                case 1:
                    villager.velocity = new Vector2(speedMovement, 0);
                    animator.SetFloat("movementX", 1);
                    animator.SetFloat("movementY", 0);

                    if (hasWalkArea && transform.position.x > maxWalkPoint.x)
                    {
                        timeWalkingCounter = 0;
                    }
                    break;

                case 2:
                    villager.velocity = new Vector2(0, -speedMovement);
                    animator.SetFloat("movementX", 0);
                    animator.SetFloat("movementY", -1);

                    if (hasWalkArea && transform.position.y < minWalkPoint.y)
                    {
                        timeWalkingCounter = 0;
                    }
                    break;

                case 3:
                    villager.velocity = new Vector2(-speedMovement, 0);
                    animator.SetFloat("movementX", -1);
                    animator.SetFloat("movementY", 0);

                    if (hasWalkArea && transform.position.x < minWalkPoint.x)
                    {
                        timeWalkingCounter = 0;
                    }
                    break;
            }

            if (timeWalkingCounter <= 0)
            {
                isWalking = false;
                animator.SetBool("isWalking", false);
                SelectAction(7);
            }
        }
        else if (isIdle)
        {
            timeIdleCounter -= Time.deltaTime;
            villager.velocity = Vector2.zero;

            if (timeIdleCounter < 0)
            {
                SelectAction(7);
            }
        }
        else if (isTalking || isAboutToTalk)
        {
            villager.velocity = Vector2.zero;
            setTalkingDirection(talkingDirection);
        }
        else
        {
            SelectAction(5);
        }
    }

    /// <summary>
    /// Change the current walking direction of the villager
    /// </summary>
    public void changeDirection()
    {
        isWalking = false;
        animator.SetBool("isWalking", false);

        SelectAction(7);
    }

    /// <summary>
    /// Select an action, the current action can not be same as the last action
    /// </summary>
    public void SelectAction(int maxActions)
    {
        if (!isTalking)
        {
            actionSelected = Random.Range(0, maxActions);

            if (actionSelected == lastAction)
            {
                SelectAction(7);
            }
            else
            {
                lastAction = actionSelected;
            }

            if (actionSelected <= 3)
            {
                isWalking = true;
                isIdle = false;
                timeWalkingCounter = timeToWalk;
                animator.SetBool("isWalking", true);
            }
            else
            {
                isWalking = false;
                isIdle = true;
                timeIdleCounter = timeIdle;
                animator.SetBool("isWalking", false);
            }
        }
    }

    /// <summary>
    /// Start walking in the selected direction
    /// </summary>
    public void startTalking()
    {
        isWalking = false;
        isIdle = false;
        isAboutToTalk = true;
        animator.SetBool("isWalking", false);
    }

    /// <summary>
    /// Check if the villager is being pushed
    /// </summary>
    /// <param name="isPushed">Boolean, true if the villager is being pushed</param>
    public void isPushed(bool isPushed)
    {
        animator.SetBool("isPushed", isPushed);
    }

    private void setTalkingDirection(int talkingDirection)
    {
        switch (talkingDirection)
        {
            case 0:
                animator.SetFloat("movementX", 0);
                animator.SetFloat("movementY", 1);
                break;

            case 1:
                animator.SetFloat("movementX", 1);
                animator.SetFloat("movementY", 0);
                break;

            case 2:
                animator.SetFloat("movementX", 0);
                animator.SetFloat("movementY", -1);
                break;

            case 3:
                animator.SetFloat("movementX", -1);
                animator.SetFloat("movementY", 0);
                break;
        }
    }
}