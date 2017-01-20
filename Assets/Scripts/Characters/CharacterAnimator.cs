using UnityEngine;
using System.Collections;

public class CharacterAnimator : MonoBehaviour
{
    public GameObject targetGraphic;

    private CharacterMove characterMove;

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
}
