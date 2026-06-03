using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("Projectile")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform shootPoint;

    [Header("Settings")]
    [SerializeField] private KeyCode shootKey = KeyCode.G;
    [SerializeField] private Vector3 shootDirection = Vector3.right;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float damage = 25f;

    private void Update()
    {
        if (Input.GetKeyDown(shootKey))
        {
            Shoot();
        }
    }

    private void Shoot()
    {

        Vector3 spawnPosition = shootPoint != null ? shootPoint.position : transform.position;
        Vector3 direction = shootDirection.normalized;

        GameObject projectileObject = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectile.Initialize(direction, projectileSpeed, damage);
        }
    }
}