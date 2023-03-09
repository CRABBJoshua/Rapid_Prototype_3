using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CS_CountDownTimer : MonoBehaviour
{
    public float currentTime = 0f;
    public float startingTime = 10f;

    public Text countdownText;

    private void Start()
    {
        currentTime = startingTime;
    }

    private void Update()
    {
        currentTime -= 1 * Time.deltaTime;
        print(currentTime);
    }
}
