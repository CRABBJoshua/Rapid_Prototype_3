using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CS_BobTheBlob : MonoBehaviour
{
	/*TODOLIST:
	 * Damage to the player
	 * Death
	 */

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
	private bool edgeCheck = false;
	private bool groundCheck = false;
    float direction;
	public ExampleController Player;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();   
        rb = GetComponent<Rigidbody2D>();   
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			Debug.Log("Damage?");
			Player.TakeDamage(20);

			if (Player.transform.position.y > (transform.position.y) + 0.1)
			{
				Debug.Log("Death");
				Destroy(gameObject);
			}
		}
	}

	void Start()
    {
        if(isSpriteFacingRight == true)
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

	//private void OnDrawGizmos()
	//{
	//	if (Application.isPlaying)
	//		return;

	//	Gizmos.DrawCube(transform.position + (Vector3)edgeCheckOffset, Vector3.one * edgeCheckSize);
	//}

	void Update()
    {
        edgeCheckPos = new Vector2(transform.position.x + (direction * edgeCheckOffset.x), transform.position.y - edgeCheckOffset.y);

		//Physics2D.OverlapBox(point, size, angle, layerMask, mindepth);
		//Physics2D.OverlapBox()
        edgeCheck = Physics2D.OverlapBox(edgeCheckPos, edgeCheckSize, 0, collisionLayer);
        groundCheck = Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, collisionLayer);

		//print(edgeCheckPos);
		//Debug.Log(edgeCheck + " .. " + groundCheck);

		if (edgeCheck == false && groundCheck == true)
        {
			//Debug.Log("change direction: " + edgeCheck + " .. " + groundCheck);
            ChangeDirection();
        }
    }

    void FixedUpdate()
    {
		rb.velocity = new Vector2(direction * movementSpeed, rb.velocity.y);
	}

    void ChangeDirection()
    {
		//Debug.Log(direction);

        if(direction == 1)
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
