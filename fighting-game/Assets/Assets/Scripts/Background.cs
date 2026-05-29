using UnityEngine;

public class Background : MonoBehaviour
{
    public Transform cameraTransform;
    public float parallaxAmount = 0.2f;

    private Vector3 startPosition;
    private Vector3 cameraStartPosition;

    void Start()
    {
        startPosition = transform.position;
        cameraStartPosition = cameraTransform.position;
    }

    void LateUpdate()
    {
        Vector3 cameraMovement = cameraTransform.position - cameraStartPosition;

        transform.position = new Vector3(
            startPosition.x + cameraMovement.x * parallaxAmount,
            startPosition.y,
            startPosition.z
        );
    }
}