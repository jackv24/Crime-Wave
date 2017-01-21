using UnityEngine;
using System.Collections;

public class CharacterStats : MonoBehaviour
{
    public delegate void IntEvent(int value);
    public event IntEvent OnRespectChange;

    [HideInInspector]
    public bool inHideRegion = false;
    [HideInInspector]
    public bool isHidden = false;

    public int respect = 0;

    public GameObject hidePrompt;

    private CharacterAnimator characterAnimator;

    void Awake()
    {
        characterAnimator = GetComponent<CharacterAnimator>();
    }

    void Start()
    {
        hidePrompt.SetActive(false);
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

    public void AddRespect(int amount)
    {
        respect += amount;

        if (OnRespectChange != null)
            OnRespectChange(respect);
    }

    public void ShowPrompt(bool value)
    {
        hidePrompt.SetActive(value);
    }
}
