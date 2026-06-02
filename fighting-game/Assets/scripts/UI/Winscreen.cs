using UnityEngine;
using UnityEngine.UI;

public class Winscreen : MonoBehaviour
{

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
            
        }
        else
        {

        }
    }
}