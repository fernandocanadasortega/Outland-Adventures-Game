using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightPatrol : MonoBehaviour
{
    public float speedMovement;
    private Rigidbody2D knight;
    private Animator animator;
    public Collider2D walkArea;

    private bool isWalking;
    private bool isIdle;
    private float timeIdleCounter;

    private int actionSelected;

    private Vector2 minWalkPoint;
    private Vector2 maxWalkPoint;
    private bool hasWalkArea;

    // Start is called before the first frame update
    void Start()
    {
        knight = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        actionSelected = 0;

        if (walkArea != null)
        {
            minWalkPoint = walkArea.bounds.min;
            maxWalkPoint = walkArea.bounds.max;
            hasWalkArea = true;
        }

        SelectAction();
    }

    private void FixedUpdate()
    {
        if (isWalking)
        {
            switch (actionSelected)
            {
                case 0:
                    knight.velocity = new Vector2(0, speedMovement);
                    animator.SetFloat("movementX", 0);
                    animator.SetFloat("movementY", 1);

                    if (hasWalkArea && transform.position.y > maxWalkPoint.y)
                    {
                        startIdleState();
                    }
                    break;

                case 1:
                    knight.velocity = new Vector2(speedMovement, 0);
                    animator.SetFloat("movementX", 1);
                    animator.SetFloat("movementY", 0);

                    if (hasWalkArea && transform.position.x > maxWalkPoint.x)
                    {
                        startIdleState();
                    }
                    break;

                case 2:
                    knight.velocity = new Vector2(0, -speedMovement);
                    animator.SetFloat("movementX", 0);
                    animator.SetFloat("movementY", -1);

                    if (hasWalkArea && transform.position.y < minWalkPoint.y)
                    {
                        startIdleState();
                    }
                    break;

                case 3:
                    knight.velocity = new Vector2(-speedMovement, 0);
                    animator.SetFloat("movementX", -1);
                    animator.SetFloat("movementY", 0);

                    if (hasWalkArea && transform.position.x < minWalkPoint.x)
                    {
                        startIdleState();
                    }
                    break;
            }
        }
        else if (isIdle)
        {
            timeIdleCounter -= Time.deltaTime;
            knight.velocity = Vector2.zero;

            if (timeIdleCounter < 0)
            {
                SelectAction();
            }
        }
    }

    private void startIdleState()
    {
        isWalking = false;
        isIdle = true;
        timeIdleCounter = Random.Range(2, 4.5f);
        animator.SetBool("isWalking", false);
    }

    public void SelectAction()
    {
        if (actionSelected != 0)
        {
            if (actionSelected == 1)
            {
                actionSelected = 3;
            }
            else if (actionSelected == 3)
            {
                actionSelected = 1;
            }
        }
        else
        {
            actionSelected = 1;
        }

        isWalking = true;
        isIdle = false;
        animator.SetBool("isWalking", true);
    }
}