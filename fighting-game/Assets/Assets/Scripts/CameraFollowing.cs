using UnityEngine;

public class CameraFollowing : MonoBehaviour
{
    public Transform player1;
    public Transform player2;

    public float distanceBack = 10f;
    public float height = 3f;
    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        if (player1 == null || player2 == null) return;

        Vector3 middle = (player1.position + player2.position) / 2f;

        Vector3 targetPosition = new Vector3(
            middle.x,
            middle.y + height,
            middle.z - distanceBack
        );

        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);

        transform.LookAt(new Vector3(middle.x, middle.y + 1f, middle.z));
    }
}