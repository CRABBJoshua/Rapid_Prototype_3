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
	public int LevelNumber;

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
		//CollectableAmount = CollectableScript.GetComponent<CS_Collectable>().CollectableAmount;
		CollectableAmount = CS_Collectable.CollectableAmount;

		if(CollectableAmount == 100)
		{
			CollectableAmount = 0;
		}

        if(currentTime < 0)
        {
            Debug.Log("Quit");
			SceneManager.LoadScene(LevelNumber);
		}

		CollectableText.text = CollectableAmount.ToString();
	}
}
