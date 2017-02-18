using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using InControl;
using UnityEngine.UI;

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
    private int lastLayerOrder = 0;

    public float restartGameTime = 5f;

    public bool isGameRunning = true;

    public int targetRespect = 3000;

    public GameObject damageTextPrefab;
    public AnimationCurve textAnim = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
    public float animDuration = 1f;

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

        SceneManager.LoadScene(respectGained >= targetRespect ? 0 : SceneManager.GetActiveScene().buildIndex);
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

            if (firstCarSpawned)
                police.GetComponent<SpriteRenderer>().sortingOrder = lastLayerOrder + 1;

            lastLayerOrder = police.GetComponent<SpriteRenderer>().sortingOrder;

            police.moveSpeed = level.policeSpeed;

            float spawnX = characterStats.gameObject.transform.position.x - (police.moveSpeed * level.warningTime);
            police.gameObject.transform.position = new Vector3(spawnX, police.gameObject.transform.position.y, police.gameObject.transform.position.z);

            firstCarSpawned = true;
        }
    }

    public void ShowDamageText(int amount, Vector2 worldPos)
    {
        GameObject obj = (GameObject)Instantiate(damageTextPrefab, worldPos, Quaternion.identity);

        Text text = obj.GetComponentInChildren<Text>();
        text.text = string.Format(text.text, amount);

        StartCoroutine("AnimateDamageText", obj.transform);
    }

    IEnumerator AnimateDamageText(Transform t)
    {
        float elapsedTime = 0;

        while(elapsedTime < animDuration)
        {
            t.position += Vector3.up * Time.deltaTime;

            yield return new WaitForEndOfFrame();

            elapsedTime += Time.deltaTime;
        }

        Destroy(t.gameObject);
    }
}
