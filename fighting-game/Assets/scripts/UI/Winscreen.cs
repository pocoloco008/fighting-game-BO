using UnityEngine;
using UnityEngine.UI;

public class Winscreen : MonoBehaviour
{

    [SerializeField] private Text wintext;


    void OnEnable()
    {
        pointSystem.Win += DisplayWin;
    }
    void OnDisable()
    {
        pointSystem.Win -= DisplayWin;
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