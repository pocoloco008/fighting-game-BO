using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerHealthScript : MonoBehaviour
{
    public static event Action<int> OnPlayerDies;

    [Header("Player")]
    public int playerNumber;

    [Header("Health")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("UI")]
    public Slider healthSlider;

    private bool isDead;

    private void Start()
    {
        currentHealth = maxHealth;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead)
            return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        if (isDead)
            return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
    }

    private void Die()
    {
        isDead = true;

        Debug.Log($"{gameObject.name} died");

        OnPlayerDies?.Invoke(playerNumber);

        // Eventueel later:
        // animator.SetTrigger("Die");
        // enabled = false;
    }
}