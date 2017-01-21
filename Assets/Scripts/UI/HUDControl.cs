using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class HUDControl : MonoBehaviour
{
    public CharacterStats statsTarget;

    public GameObject starImage;

    public Sprite fullStar;
    public Sprite emptyStar;

    private List<Image> starImages = new List<Image>();

    void Start()
    {
        starImages.Add(starImage.GetComponent<Image>());

        if(statsTarget)
        {
            for (int i = 1; i < statsTarget.maxStars; i++)
            {
                GameObject obj = (GameObject)Instantiate(starImage, starImage.transform.parent);

                starImages.Add(obj.GetComponent<Image>());
            }
        }

        UpdateStars();
    }

    public void UpdateStars()
    {
        int maxStars = statsTarget.maxStars;
        int stars = statsTarget.currentStars;

        for(int i = 0; i < maxStars; i++)
        {
            if (i < stars)
                starImages[i].sprite = fullStar;
            else
                starImages[i].sprite = emptyStar;
        }
    }

}
