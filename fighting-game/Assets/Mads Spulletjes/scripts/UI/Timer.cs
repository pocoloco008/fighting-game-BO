using System;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    public static event Action<int> onTimeEnds;

    [SerializeField] private Text timerText;

    private float timeRemaining = 90f;
    private bool timerRunning = true;

    private void Start()
    {
        timerText = GetComponent<Text>();
        timerText.text = "90";
    }

    private void Update()
    {
        if (timerRunning)
        {
            timeRemaining -= Time.deltaTime;
            timerText.text = Mathf.Round(timeRemaining).ToString();

            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                timerRunning = false;
                timerText.text = "End";

                if (onTimeEnds != null)
                {
                    onTimeEnds.Invoke(0);
                }
            }
        }


    }
}

