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

    public Text respectText;
    private string respectTextString;

    public GameObject arrestedImage;

    void Start()
    {
        starImages.Add(starImage.GetComponent<Image>());

        if(statsTarget)
        {
            for (int i = 1; i < GameManager.Instance.starLevels.Length; i++)
            {
                GameObject obj = (GameObject)Instantiate(starImage, starImage.transform.parent);

                starImages.Add(obj.GetComponent<Image>());
            }
        }

        UpdateStars();

        GameManager.Instance.OnStarLevelChange += UpdateStars;

        if (respectText)
        {
            respectTextString = respectText.text;
            respectText.text = string.Format(respectTextString, statsTarget.respect);

            statsTarget.OnRespectChange += delegate (int value)
            {
                respectText.text = string.Format(respectTextString, value);
            };
        }

        if(arrestedImage)
        {
            GameManager.Instance.OnGameEnd += delegate ()
            {
                arrestedImage.SetActive(true);
            };
        }
    }

    public void UpdateStars()
    {
        int maxStars = GameManager.Instance.starLevels.Length;
        int stars = GameManager.Instance.starLevel;

        for(int i = 0; i < maxStars; i++)
        {
            if (i < stars)
                starImages[i].sprite = fullStar;
            else
                starImages[i].sprite = emptyStar;
        }
    }

}
