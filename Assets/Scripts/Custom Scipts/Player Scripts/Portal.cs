using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Portal : MonoBehaviour
{
    public GameObject Portal1;
    public GameObject Portal2;
    public ExampleController Player;
    public float exitForce_X;
    public float exitForce_Y;

	private static int n = 0;

	//private bool EnterLeft;
	//private bool EnterRight;

	//private void OnCollisionEnter2D(Collision2D collision)
	//{
	//	EnterLeft = true;
	//	Debug.Log(EnterLeft);
	//}

	//private void OnCollisionEnter(Collision collision)
	//{
	//	EnterRight = true;
	//	Debug.Log(EnterRight);
	//}

	private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
			//float m_InMove = GameObject.FindGameObjectWithTag("Player").GetComponent<ExampleController>().m_InMove;

			float m_InMove = collision.GetComponent<ExampleController>().m_InMove;

			if (m_InMove == 1)
			{
				exitForce_X = 2;
			}
			else if(m_InMove == -1)
			{
				exitForce_X = -2;
			}

			if (this.gameObject.tag == "Portal1")
            {
                StartCoroutine(TeleportToPortal2());
            }
            else if (this.gameObject.tag == "Portal2")
            {
                StartCoroutine(TeleportToPortal1());
            }
        } 
    }

    private void Awake()
    {
		n++;
		FindPlayer();
	}

	private void FindPlayer()
	{
		print("Awake called " + n);

		//GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
		GameObject playerObj = GameObject.Find("Character");

		if (playerObj == null)
			print("playerObj null");
		else
			print("player obj's tag is " + playerObj.tag);

		this.Player = playerObj.GetComponent<ExampleController>();

		if (Player == null)
			print("Player null");
	}

    IEnumerator TeleportToPortal2()
    {
        yield return new WaitForSeconds(0.1f);
		if(Player == null)
		{
			FindPlayer();

			if (Player == null)
				Debug.Log("P2 The player is null still??");
		}
		Player.transform.position = new Vector2(Portal2.transform.position.x + exitForce_X, Portal2.transform.position.y + exitForce_Y);
        Debug.Log("Worked");
    }
    IEnumerator TeleportToPortal1()
    {
        yield return new WaitForSeconds(0.1f);
		
		if (Player == null)
		{
			FindPlayer();

			if (Player == null)
				Debug.Log("P1 The player is null still??");
		}

		Player.transform.position = new Vector2(Portal1.transform.position.x + exitForce_X, Portal1.transform.position.y + exitForce_Y);
        Debug.Log("Worked");
    }

}
