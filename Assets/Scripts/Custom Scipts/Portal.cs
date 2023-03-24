using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject Portal1;
    public GameObject Portal2;
    public GameObject Player;
    public float exitForce_X;
    public float exitForce_Y;

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
            float m_InMove = Player.gameObject.GetComponent<ExampleController>().m_InMove;

			if(m_InMove == 1)
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
            if (this.gameObject.tag == "Portal2")
            {
                StartCoroutine(TeleportToPortal1());
            }
        } 
    }

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    IEnumerator TeleportToPortal2()
    {
        yield return new WaitForSeconds(0.1f);
        Player.transform.position = new Vector2(Portal2.transform.position.x + exitForce_X, Portal2.transform.position.y + exitForce_Y);
        Debug.Log("Worked");
    }
    IEnumerator TeleportToPortal1()
    {
        yield return new WaitForSeconds(0.1f);
        Player.transform.position = new Vector2(Portal1.transform.position.x + exitForce_X, Portal1.transform.position.y + exitForce_Y);
        Debug.Log("Worked");
    }

}
