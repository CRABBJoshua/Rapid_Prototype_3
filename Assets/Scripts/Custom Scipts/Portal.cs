using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject CurrentPortal;
    public GameObject Player;
    public float exitForce_X;
    public float exitForce_Y;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Player.gameObject.tag == "Player")
        {
            StartCoroutine(Teleport());
        } 
    }

    IEnumerator Teleport()
    {
        yield return new WaitForSeconds(0.1f);
        Player.transform.position = new Vector2(CurrentPortal.transform.position.x + exitForce_X, CurrentPortal.transform.position.y + exitForce_Y);
        Debug.Log("Worked");
    }

}
