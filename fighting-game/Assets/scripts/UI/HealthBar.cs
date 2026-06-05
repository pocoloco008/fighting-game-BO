using UnityEngine;
using UnityEngine.UI;
using System;

public class HealthBarPlayer1 : MonoBehaviour
{

    public static event Action<int> onPlayerDies;

    [SerializeField] private KeyCode damageKey;
    [SerializeField] private KeyCode healthKey;

    public Slider healthBar;

    public float maxHealth = 100f;
    public float currentHealth;

    public int playerNumber;


    void Start()
    {
        currentHealth = maxHealth;
    }


    void Update()
    {
        healthBar.value = currentHealth;

        if (currentHealth == 0f)
        {
            dead();

        }

        if (Input.GetKeyDown(damageKey))
        {
            TakeDamage(1f);
        }
        if (Input.GetKeyDown(healthKey))
        {
            addHealth(10f);
        }
    }

    private void TakeDamage(float damage)
    {
        currentHealth -= damage;
    }

    private void addHealth(float health)
    {
        currentHealth += health;
    }

    private void dead()
    {
        onPlayerDies?.Invoke(playerNumber);
    }

}
