using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public delegate void StandardEvent();
    public event StandardEvent OnStarLevelChange;

    public static GameManager Instance;

    public CharacterStats characterStats;

    [System.Serializable]
    public class StarLevels
    {
        public int respectNeeded = 10;
        public float warningTime = 2f;
        public float policeSpeed = 5f;
        public float patrolInterval = 20f;
    }
    public StarLevels[] starLevels = new StarLevels[5];

    private int oldRespectValue = 0;
    private int respectGained = 0;

    [HideInInspector]
    public int starLevel = 0;

    [HideInInspector]
    public bool chanceRemoveStar = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        if(!characterStats)
        {
            GameObject player = GameObject.FindWithTag("Player");

            if (player)
                characterStats = player.GetComponent<CharacterStats>();
        }

        if (characterStats)
            characterStats.OnRespectChange += RespectChange;
    }

    void RespectChange(int value)
    {
        respectGained += value - oldRespectValue;
        oldRespectValue = value;

        if(starLevel < starLevels.Length && respectGained >= starLevels[starLevel].respectNeeded)
        {
            starLevel++;

            OnStarLevelChange();
        }
    }

    public void RemoveStar()
    {
        if(chanceRemoveStar && starLevel > 0)
        {
            chanceRemoveStar = false;

            starLevel--;

            if (starLevel > 0)
                respectGained = starLevels[starLevel - 1].respectNeeded;
            else
                respectGained = 0;

            OnStarLevelChange();
        }
    }
}
