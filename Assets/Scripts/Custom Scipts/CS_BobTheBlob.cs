using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CS_BobTheBlob : MonoBehaviour
{
    SpriteRenderer sr;
    Rigidbody2D rb;

    [SerializeField] bool isSpriteFacingRight;
    [SerializeField] float movementSpeed;
    [SerializeField] Transform groundCheckPos;
    [SerializeField] LayerMask collisionLayer;
    [SerializeField] Vector2 edgeCheckOffset;

    Vector2 edgeCheckPos;
    Vector2 edgeCheckSize;
    Vector2 groundCheckSize;
    bool edgeCheck;
    bool groundCheck;
    float direction;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();   
        rb = GetComponent<Rigidbody2D>();   
    }

    void Start()
    {
        if(isSpriteFacingRight == false)
        {
            direction = -1;
        }
        else
        {
            direction = 1;
        }

        edgeCheckSize = new Vector2(0.5f, 0.5f);
        groundCheckSize = new Vector2(0.25f, 0.25f);
    }

    void Update()
    {
        edgeCheckPos = new Vector2(transform.position.x + (direction * edgeCheckOffset.x), transform.position.y - edgeCheckOffset.y);

        edgeCheck = Physics2D.OverlapBox(edgeCheckPos, edgeCheckSize, 0, collisionLayer);
        groundCheck = Physics.OverlapBox(groundCheckPos.position, groundCheckSize, 0, collisionLayer);

        if(edgeCheck == false && groundCheck == true)
        {
            ChangeDirection();
        }
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(direction * movementSpeed, rb.velocity.y);    
    }

    void ChangeDirection()
    {
        if(direction == 0)
        {
            direction = -1;
            if (isSpriteFacingRight)
                sr.flipX = true;
            else
                sr.flipX = false;
        }
        else
        {
            direction = 1;
            if(isSpriteFacingRight)
                sr.flipX = false;
            else
                sr.flipX = true;
        }
    }

    void OnDrawGizmos()
    {
        if(edgeCheck == true)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(edgeCheckPos, edgeCheckSize);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(edgeCheckPos, edgeCheckSize);
        }
        if(groundCheck == true)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(groundCheckPos.position, groundCheckSize);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(groundCheckPos.position, groundCheckSize);
        }
    }
}
