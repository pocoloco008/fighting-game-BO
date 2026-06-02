using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Slider healthBarSlider;

    public float maxHealth = 100f;
    public float currentHealth;


    void Start()
    {
        currentHealth = maxHealth;
    }


    void Update()
    {

        healthBarSlider.value = currentHealth;


        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(1f);
        }

        if (Input.GetKeyDown(KeyCode.A))
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

}
