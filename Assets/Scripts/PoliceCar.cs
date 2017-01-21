using UnityEngine;
using System.Collections;

public class PoliceCar : MonoBehaviour
{
    public float moveSpeed = 5f;

    public Vector2 catchCircleOrigin;
    public float catchCircleRadius = 5f;

    public float destroyAtPlayerDistance = 100f;

    public Transform player;
    private CharacterStats stats;

    void Start()
    {
        if(!player)
        {
            GameObject o = GameObject.FindWithTag("Player");

            if (o)
                player = o.transform;
        }

        if (player)
            stats = player.GetComponent<CharacterStats>();
    }

    void Update()
    {
        if(GameManager.Instance.isGameRunning)
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);

        if(Vector2.Distance((Vector2)transform.position + catchCircleOrigin, player.position) <= catchCircleRadius)
        {
            if(stats)
            {
                if (!stats.isHidden)
                    GameManager.Instance.EndGame();
            }
        }
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + catchCircleOrigin, catchCircleRadius);
    }
}
