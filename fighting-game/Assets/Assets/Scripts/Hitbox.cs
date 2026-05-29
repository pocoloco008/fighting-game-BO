using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public Fighter owner;
    public string moveName;

    private void OnTriggerEnter(Collider other)
    {
        Fighter target = other.GetComponent<Fighter>();

        if (target == null) return;
        if (target == owner) return;

        Vector3 hitPoint = other.ClosestPoint(transform.position);

        Debug.Log(owner.playerName + " hit " + target.playerName +
                  " with " + moveName +
                  " at world position " + hitPoint);
    }
}