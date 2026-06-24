using System;
using UnityEngine;

public class pointSystem : MonoBehaviour
{

    public static event Action<int> Win;


    public GameObject[] player;
    public int playernumber;

    [SerializeField] public int[] points = {0,0};
    private int maxpoints = 2;

    void OnEnable()
    {
        HealthBar.onPlayerDies += pointToPlayer;
    }

    private void OnDisable()
    {
        HealthBar.onPlayerDies -= pointToPlayer;
    }

    private void pointToPlayer(int playernumber)
    {
        if (playernumber == 1)
        {
            points[0] += 1;
        }
        else
        {
            points[1] += 1;
        }

        foreach (int pointsOfCurrectPlayer in points) 
        {
            if (pointsOfCurrectPlayer >= maxpoints) 
            {
                Winner();
            } 
        }

      
    }

    private void Winner()
    {
            Win.Invoke(playernumber);
    }
}
