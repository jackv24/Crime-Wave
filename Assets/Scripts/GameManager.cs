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

    public GameObject policeCarPrefab;

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

        StartCoroutine("UpdatePolice");
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
        if(starLevel > 0)
        {

            starLevel--;

            if (starLevel > 0)
                respectGained = starLevels[starLevel - 1].respectNeeded;
            else
                respectGained = 0;

            OnStarLevelChange();
        }
    }

    public void EndGame()
    {
        Debug.Log("Busted! Game Over!");
        Time.timeScale = 0;
    }

    IEnumerator UpdatePolice()
    {
        float nextTime = 0;
        int lastStar = 0;

        while (true)
        {
            if(starLevel != lastStar)
            {
                lastStar = starLevel;
                nextTime = Time.time;
            }

            if (starLevel < starLevels.Length && starLevel > 0)
            {
                if (Time.time >= nextTime)
                {
                    PoliceCar police = ((GameObject)Instantiate(policeCarPrefab, policeCarPrefab.transform.position, policeCarPrefab.transform.localRotation)).GetComponent<PoliceCar>();

                    police.moveSpeed = starLevels[starLevel].policeSpeed;

                    float spawnX = characterStats.gameObject.transform.position.x - (police.moveSpeed * starLevels[starLevel].warningTime);
                    police.gameObject.transform.position = new Vector3(spawnX, police.gameObject.transform.position.y, police.gameObject.transform.position.z);
                }

                nextTime = starLevels[starLevel].patrolInterval + Time.time;
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
