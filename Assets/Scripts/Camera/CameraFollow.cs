using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    [Space()]
    public float followSpeed = 10f;

    private Vector3 initialPos;
    private float oldXPos = 0;

    private float initialSize;

    private Camera cam;

    void Start()
    {
        initialPos = transform.position;

        cam = GetComponent<Camera>();

        //If there is no target assigned, try and find one
        if(!target)
        {
            //New target is probably tagged Player
            GameObject player = GameObject.FindWithTag("Player");

            //If player was found, set that as the new target
            if (player)
                target = player.transform;
        }

        initialSize = cam.orthographicSize;
    }

    void LateUpdate()
    {
        //If there is a target
        if(target)
        {
            Vector3 pos = target.transform.position;
            //Do not follow on Z (since it's 2D)
            pos.z = transform.position.z;
            pos.y = initialPos.y;

            if (pos.x < oldXPos)
                pos.x = oldXPos;
            else
                oldXPos = pos.x;

            transform.position = Vector3.Lerp(transform.position, pos, followSpeed * Time.deltaTime);
        }
    }

    void CameraShake(float shakeAmount)
    {
        Vector2 dir = new Vector2(Random.Range(-1, 1f), Random.Range(-1, 1f));
        dir.Normalize();

        cam.orthographicSize += 0.1f;

        transform.position += (Vector3)(dir * shakeAmount);

        StartCoroutine("ScreenZoom");
    }

    IEnumerator ScreenZoom()
    {
        float duration = 0.1f;
        float timeElapsed = 0f;

        while (timeElapsed <= duration)
        {
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, cam.orthographicSize - 0.05f, timeElapsed / duration);

            yield return new WaitForEndOfFrame();
            timeElapsed += Time.deltaTime;
        }

        timeElapsed = 0;

        while (timeElapsed <= duration)
        {
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, initialSize, timeElapsed / duration);

            yield return new WaitForEndOfFrame();
            timeElapsed += Time.deltaTime;
        }
    }
}
