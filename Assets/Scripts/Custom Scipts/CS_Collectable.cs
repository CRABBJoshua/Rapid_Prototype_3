using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Collectable : MonoBehaviour
{
	public int CollectableAmount;
	public int MaxCollectableAmount;
	public GameObject self;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			CollectableAmount++;
		}
	}
}
