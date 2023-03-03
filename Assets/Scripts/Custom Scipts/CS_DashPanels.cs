using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_DashPanels : MonoBehaviour
{
    private GameObject Player;
    public float SpeedForce;
    public Vector2 ForceDirection;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Dashed!");
        if (Player.gameObject.tag == "Player")
        {
            StartCoroutine(Boost());
        }
    }

    private void Awake()
    {
       Player = GameObject.FindGameObjectWithTag("Player"); 
    }

    IEnumerator Boost()
    {
        yield return new WaitForSeconds(0.1f);
        Player.GetComponent<Rigidbody2D>().AddForce(ForceDirection * SpeedForce, ForceMode2D.Force);
    }
}
