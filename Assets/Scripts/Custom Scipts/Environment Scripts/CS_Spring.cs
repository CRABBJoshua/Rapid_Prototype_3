using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Spring : MonoBehaviour
{
	public Vector2 bouncingPower;
	public GameObject Player;

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			other.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(bouncingPower.x, bouncingPower.y);
			other.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0f;
			Invoke("SetGravityScale", 2);
		}
	}

	void SetGravityScale()
	{
		Debug.Log("Changed");
		Player.GetComponent<Rigidbody2D>().gravityScale = 3f;
	}
}
