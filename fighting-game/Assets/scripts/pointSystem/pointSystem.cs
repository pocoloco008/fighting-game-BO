using UnityEngine;

public class pointSystem : MonoBehaviour
{
    private int points = 0;
    private int maxpoints = 2;

    private void OnEnable()
    {
        HealthBar.onPlayerDies += 
    }

    private void pointToPlayer()
    {

    }

    private void win()
    {
        if(points == maxpoints)
        {
            Debug.Log("winner!!");
        }
    }
}
