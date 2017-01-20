using UnityEngine;
using System.Collections;
using UnityEditor;

public class CharacterAttack : MonoBehaviour
{
    public float attackRadius = 2f;

    public float explosionForce = 10f;

    public void Attack(int direction)
    {
        Debug.Log("Attacking " + direction);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRadius);

        foreach(Collider2D col in colliders)
        {
            if (col.tag == "Damageable")
            {
                Debug.Log("Knocked");

                Rigidbody2D body = col.GetComponent<Rigidbody2D>();

                Vector2 dir = (col.transform.position - transform.position).normalized;

                body.AddForceAtPosition(dir * explosionForce, transform.position, ForceMode2D.Impulse);
                body.AddTorque(direction > 0 ? 5 : -5, ForceMode2D.Impulse);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
