using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    [SerializeField] private Text timerText;

    private void Start()
    {
        timerText = GetComponent<Text>();

        timerText.text = "90:00";
    }

    private void Update()
    {
     timerText.text = Mathf.Round(90 - Time.time).ToString();
    }
}