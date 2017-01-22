using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public delegate void StandardEvent();
    public event StandardEvent OnStarLevelChange;
    public event StandardEvent OnGameEnd;

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

    private bool firstCarSpawned = false;

    public float restartGameTime = 5f;

    public bool isGameRunning = true;

    public int targetRespect = 3000;

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

        OnStarLevelChange += delegate ()
        {
            StopCoroutine("UpdatePolice");
            StartCoroutine("UpdatePolice", starLevel);
        };
    }

    void RespectChange(int value)
    {
        respectGained += value - oldRespectValue;
        oldRespectValue = value;

        if (value >= targetRespect)
            HUDControl.Instance.ShowWin();

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
        OnGameEnd();

        isGameRunning = false;

        StartCoroutine("ReloadLevel", restartGameTime);
    }

    IEnumerator ReloadLevel(float delay)
    {
        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator UpdatePolice(int star)
    {
        if (star <= 0)
            yield return null;

        StarLevels level = starLevels[star - 1];

        while (true)
        {
            float time = firstCarSpawned ? Random.Range(1f, level.patrolInterval) : 0;

            yield return new WaitForSeconds(time);

            PoliceCar police = ((GameObject)Instantiate(policeCarPrefab, policeCarPrefab.transform.position, policeCarPrefab.transform.localRotation)).GetComponent<PoliceCar>();

            police.moveSpeed = level.policeSpeed;

            float spawnX = characterStats.gameObject.transform.position.x - (police.moveSpeed * level.warningTime);
            police.gameObject.transform.position = new Vector3(spawnX, police.gameObject.transform.position.y, police.gameObject.transform.position.z);

            firstCarSpawned = true;
        }
    }
}
