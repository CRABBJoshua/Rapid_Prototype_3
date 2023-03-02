using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_DashPanels : MonoBehaviour
{
    public BetterCharacterController Player;
    public Vector2 SpeedForce;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Dashed!");
        if (Player.gameObject.tag == "Player")
        {
            StartCoroutine(Boost());
        }
    }

    private void Awake()
    {
        Player = GetComponent<BetterCharacterController>();
    }

    IEnumerator Boost()
    {
        yield return new WaitForSeconds(0.1f);
        Player.rb.AddForce(Vector2.left * SpeedForce, ForceMode2D.Force);
    }
}
