using UnityEngine;

public class InputDamageManager : MonoBehaviour
{
    [Header("Player Health References")]
    [Tooltip("Assign Player 1's PlayerHealthScript (will try to auto-find by tag 'Player1' if empty)")]
    public PlayerHealthScript player1;

    [Tooltip("Assign Player 2's PlayerHealthScript (will try to auto-find by tag 'Player2' if empty)")]
    public PlayerHealthScript player2;

    [Header("Damage Settings")]
    [Tooltip("Damage applied when pressing the mapped key/button")]
    public float damageAmount = 10f;

    private void Awake()
    {
        // Try to auto-assign by tag if fields are empty
        if (player1 == null)
        {
            var go = GameObject.FindWithTag("Player1");
            if (go != null) player1 = go.GetComponent<PlayerHealthScript>();
        }

        if (player2 == null)
        {
            var go = GameObject.FindWithTag("Player2");
            if (go != null) player2 = go.GetComponent<PlayerHealthScript>();
        }
    }

    private void Update()
    {
        // Keyboard shortcuts for quick testing
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            player1?.TakeDamage(damageAmount);
            Debug.Log($"InputDamageManager: Damaged Player1 for {damageAmount}");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            player2?.TakeDamage(damageAmount);
            Debug.Log($"InputDamageManager: Damaged Player2 for {damageAmount}");
        }
    }

    // Public methods you can call from controller input handlers or UI buttons
    public void DamagePlayer1() => player1?.TakeDamage(damageAmount);
    public void DamagePlayer2() => player2?.TakeDamage(damageAmount);

    // Overloads to pass custom damage values (useful for different attack strengths)
    public void DamagePlayer1(float amount) => player1?.TakeDamage(amount);
    public void DamagePlayer2(float amount) => player2?.TakeDamage(amount);
}