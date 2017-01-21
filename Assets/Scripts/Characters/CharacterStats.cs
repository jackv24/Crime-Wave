using UnityEngine;
using System.Collections;

public class CharacterStats : MonoBehaviour
{
    public delegate void NormalEvent();

    public event NormalEvent OnStarLevelChange;

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

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            currentStars++;

            if (OnStarLevelChange != null)
                OnStarLevelChange();
        }
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
