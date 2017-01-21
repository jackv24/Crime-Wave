using UnityEngine;
using System.Collections;

public class CharacterStats : MonoBehaviour
{
    public int maxStars = 5;
    public int currentStars = 0;

    [HideInInspector]
    public bool inHideRegion = false;
    [HideInInspector]
    public bool isHidden = false;

    private CharacterAnimator characterAnimator;

    void Awake()
    {
        characterAnimator = GetComponent<CharacterAnimator>();
    }

    public void Hide()
    {
        if (inHideRegion)
        {
            isHidden = true;

            characterAnimator.SetHidden(true);
        }
    }

    public void UnHide()
    {
        if (isHidden)
        {
            isHidden = false;

            characterAnimator.SetHidden(false);
        }
    }
}
