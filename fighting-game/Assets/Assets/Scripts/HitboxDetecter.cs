using UnityEngine;

public class HitboxDetecter : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Hitbox detected collision with Player!");
        }
    }

}
