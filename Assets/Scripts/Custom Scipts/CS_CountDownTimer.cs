using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CS_CountDownTimer : MonoBehaviour
{
    public float currentTime = 0f;
    public float startingTime = 10f;
	public int CollectableAmount;

	[SerializeField] Text countdownText;
    [SerializeField] Text CollectableText;
    [SerializeField] CS_Collectable CollectableScript;

    private void Start()
    {
        currentTime = startingTime;
    }

    private void Update()
    {
        currentTime -= 1 * Time.deltaTime;
        countdownText.text = currentTime.ToString();
		CollectableAmount = CollectableScript.GetComponent<CS_Collectable>().CollectableAmount;
		CollectableText.text = CollectableAmount.ToString();

		if(CollectableAmount == 10)
		{
			CollectableAmount = 0;
		}

        if(currentTime < 0)
        {
            Debug.Log("Quit");
			SceneManager.LoadScene(1);
		}
    }
}
