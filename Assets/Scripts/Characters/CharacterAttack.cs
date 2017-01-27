using UnityEngine;
using System.Collections;

public class CharacterAttack : MonoBehaviour
{
    public float attackRadius = 2f;
    public Vector2 attackOffset = Vector2.right;

    public float damageDelay = 0.1f;

    public float explosionForce = 10f;

    private CharacterAnimator characterAnimator;
    private CharacterStats characterStats;
    private CharacterSound characterSound;

    void Awake()
    {
        characterAnimator = GetComponent<CharacterAnimator>();
        characterStats = GetComponent<CharacterStats>();
        characterSound = GetComponent<CharacterSound>();
    }

    public void Attack(float direction)
    {
        if (characterStats.isHidden)
            return;

        if (characterAnimator)
            characterAnimator.Attack();

        if (characterSound)
            characterSound.PlayRandomAttack();

        StartCoroutine("DelayAttack", direction);
    }

    IEnumerator DelayAttack(float direction)
    {
        yield return new WaitForSeconds(damageDelay);
        
        Collider2D[] colliders = Physics2D.OverlapCircleAll((Vector2)transform.position + new Vector2(attackOffset.x * direction, attackOffset.y), attackRadius);

        foreach (Collider2D col in colliders)
        {
            if (col.tag == "Damageable")
            {
                Vector2 dir = (col.transform.position - transform.position).normalized;

                DamageableObject o = col.GetComponent<DamageableObject>();

                if (o)
                    o.Destroy(dir.x, characterStats);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if(!Application.isPlaying)
            Gizmos.DrawWireSphere(transform.position + (Vector3)attackOffset, attackRadius);
    }
}
