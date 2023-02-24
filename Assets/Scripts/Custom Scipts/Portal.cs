using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject CurrentPortal;
    public GameObject Player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(Player.gameObject.tag == "Player")
        {
            StartCoroutine(Teleport()); 
        }
    }

    IEnumerator Teleport()
    {
        yield return new WaitForSeconds(1);
        Player.transform.position = new Vector2(CurrentPortal.transform.position.x, CurrentPortal.transform.position.y);
        Debug.Log("Worked");
    }

}
