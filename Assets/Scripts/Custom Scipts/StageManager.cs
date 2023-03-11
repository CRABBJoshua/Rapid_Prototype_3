using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class StageManager : MonoBehaviour
{
    private static StageManager Instance;

    public static StageManager
    {
        get
        {
            if (Instance == null)
            {
                Instance = FindObjectOfType<StageManager>();
                Instance.StartSingleton();
            }

            return Instance;
        }
    }
}
