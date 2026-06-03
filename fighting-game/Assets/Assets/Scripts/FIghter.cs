using System.Collections;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    public string playerName = "Player 1";

    /// Movement
    public float moveSpeed = 4f;
    public Transform opponent;

    //Controls
    public KeyCode leftKey;
    public KeyCode rightKey;
    public KeyCode punchKey;
    public KeyCode kickKey;

    //Hitboxes
    public GameObject punchHitbox;
    public GameObject kickHitbox;

    public float punchTime = 0.15f;
    public float kickTime = 0.2f;

    void Start()
    {
        if (punchHitbox != null)
            punchHitbox.SetActive(false);

        if (kickHitbox != null)
            kickHitbox.SetActive(false);
    }

    void Update()
    {
        Move();
        FaceOpponent();

        if (Input.GetKeyDown(punchKey))
        {
            StartCoroutine(Attack(punchHitbox, "Bean Punch", punchTime));
        }

        if (Input.GetKeyDown(kickKey))
        {
            StartCoroutine(Attack(kickHitbox, "Bean Kick", kickTime));
        }
    }

    void Move()
    {
        float move = 0f;

        if (Input.GetKey(leftKey)) move = -1f;
        if (Input.GetKey(rightKey)) move = 1f;

        transform.position += new Vector3(move * moveSpeed * Time.deltaTime, 0f, 0f);
    }

    void FaceOpponent()
    {
        if (opponent == null) return;

        Vector3 direction = opponent.position - transform.position;

        if (direction.x > 0)
            transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        else
            transform.rotation = Quaternion.Euler(0f, -90f, 0f);
    }

    IEnumerator Attack(GameObject hitbox, string moveName, float activeTime)
    {
        if (hitbox == null)
        {
            Debug.LogError(playerName + " has no hitbox assigned for " + moveName);
            yield break;
        }

        Hitbox hb = hitbox.GetComponent<Hitbox>();

        if (hb == null)
        {
            Debug.LogError(hitbox.name + " has no Hitbox script!");
            yield break;
        }

        hb.owner = this;
        hb.moveName = moveName;

        hitbox.SetActive(true);
        yield return new WaitForSeconds(activeTime);
        hitbox.SetActive(false);
    }
}