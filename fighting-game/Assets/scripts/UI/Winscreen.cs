using UnityEngine;
using UnityEngine.UI;

public class Winscreen : MonoBehaviour
{

    [SerializeField] private Text wintext;


    void OnEnable()
    {
        HealthBarPlayer1.onPlayerDies += DisplayWin;
    }
    void OnDisable()
    {
        HealthBarPlayer1.onPlayerDies -= DisplayWin;
    }


    private void DisplayWin(int playerNum)
    {
        if (playerNum == 1)
        {
            wintext.text = "Player 2 wins!"; 
        }
        else
        {
            wintext.text = "Player 1 wins!";
        }
    }
}