using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [System.Serializable]
    public class AttackData
    {
        [Header("Info")]
        public string attackName;

        [Header("Animation")]
        public Sprite[] sprites;
        public float fps = 3f;

        [Header("Damage")]
        public float damage = 10f;
        public float cooldown = 0.3f;

        [Header("Hitbox")]
        public Vector2 hitboxSize = Vector2.one;
        public Vector2 hitboxOffset;
    }

    [Header("References")]
    public SpriteRenderer spriteRenderer;

    [Tooltip("Layer waarop spelers staan")]
    public LayerMask playerLayer;

    [Header("Attacks")]
    public AttackData lightPunch;
    public AttackData heavyPunchWeapon;
    public AttackData lightKick;
    public AttackData heavyKick;

    private bool attacking;

    #region Input

    public void OnLightPunch(InputValue value)
    {
        if (value.isPressed)
            TryAttack(lightPunch);
    }

    public void OnHeavyPunchWeapon(InputValue value)
    {
        if (value.isPressed)
            TryAttack(heavyPunchWeapon);
    }

    public void OnLightKick(InputValue value)
    {
        if (value.isPressed)
            TryAttack(lightKick);
    }

    public void OnHeavyKick(InputValue value)
    {
        if (value.isPressed)
            TryAttack(heavyKick);
    }

    #endregion

    private void TryAttack(AttackData attack)
    {
        if (attacking)
            return;

        StartCoroutine(AttackRoutine(attack));
    }

    private IEnumerator AttackRoutine(AttackData attack)
    {
        attacking = true;

        float frameTime = 1f / attack.fps;

        if (attack.sprites != null && attack.sprites.Length > 0)
        {
            foreach (Sprite frame in attack.sprites)
            {
                spriteRenderer.sprite = frame;
                yield return new WaitForSeconds(frameTime);
            }
        }

        Vector2 hitboxPosition =
            (Vector2)transform.position + attack.hitboxOffset;

        Collider2D[] hits = Physics2D.OverlapBoxAll(
            hitboxPosition,
            attack.hitboxSize,
            0f,
            playerLayer);

        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject == gameObject)
                continue;

            PlayerHealthScript health =
                hit.GetComponent<PlayerHealthScript>();

            if (health != null)
            {
                health.TakeDamage(attack.damage);

                Debug.Log(
                    $"{gameObject.name} gebruikte {attack.attackName} op {hit.name} voor {attack.damage} damage");
            }
        }

        yield return new WaitForSeconds(attack.cooldown);

        attacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        DrawAttack(lightPunch, Color.green);
        DrawAttack(heavyPunchWeapon, Color.red);
        DrawAttack(lightKick, Color.blue);
        DrawAttack(heavyKick, Color.yellow);
    }

    private void DrawAttack(AttackData attack, Color color)
    {
        Gizmos.color = color;

        Vector3 position =
            transform.position + (Vector3)attack.hitboxOffset;

        Gizmos.DrawWireCube(
            position,
            attack.hitboxSize);
    }
}