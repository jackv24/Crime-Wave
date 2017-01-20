using UnityEngine;
using System.Collections;

public class CharacterAnimator : MonoBehaviour
{
    public GameObject targetGraphic;

    private CharacterMove characterMove;
    public Animator animator;

    void Start()
    {
        characterMove = GetComponent<CharacterMove>();

        if (targetGraphic && characterMove)
        {
            characterMove.OnChangedDirection += delegate (float direction)
            {
                Vector3 scale = targetGraphic.transform.localScale;
                scale.x = direction;
                targetGraphic.transform.localScale = scale;
            };
        }
    }

    void Update()
    {
        if (animator)
        {
            if (characterMove)
            {
                animator.SetFloat("speed", Mathf.Abs(characterMove.velocity.x));
                animator.SetBool("isGrounded", characterMove.isGrounded);
            }
        }
    }

    public void Attack()
    {
        if(animator)
        {
            animator.SetTrigger("attack");
        }
    }

    public void SetHidden(bool value)
    {
        animator.SetBool("isHidden", value);
    }
}
