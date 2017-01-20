using UnityEngine;
using System.Collections;
using UnityEditor;

public class CharacterAttack : MonoBehaviour
{
    public float attackRadius = 2f;
    public Vector2 attackOffset = Vector2.right;

    public float explosionForce = 10f;

    private CharacterAnimator characterAnimator;

    void Awake()
    {
        characterAnimator = GetComponent<CharacterAnimator>();
    }

    public void Attack(float direction)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll((Vector2)transform.position + new Vector2(attackOffset.x * direction, attackOffset.y), attackRadius);

        foreach(Collider2D col in colliders)
        {
            if (col.tag == "Damageable")
            {
                Vector2 dir = (col.transform.position - transform.position).normalized;

                DamageableObject o = col.GetComponent<DamageableObject>();

                if (o)
                    o.Destroy(dir.x);
            }
        }

        if (characterAnimator)
            characterAnimator.Attack();
    }

    void OnDrawGizmosSelected()
    {
        if(!Application.isPlaying)
            Gizmos.DrawWireSphere(transform.position + (Vector3)attackOffset, attackRadius);
    }
}
