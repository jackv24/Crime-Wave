using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    [Space()]
    public float followSpeed = 10f;

    public bool followY = false;
    public bool followX = true;

    private Vector3 initialPos;

    void Start()
    {
        initialPos = transform.position;

        //If there is no target assigned, try and find one
        if(!target)
        {
            //New target is probably tagged Player
            GameObject player = GameObject.FindWithTag("Player");

            //If player was found, set that as the new target
            if (player)
                target = player.transform;
        }
    }

    void LateUpdate()
    {
        //If there is a target
        if(target)
        {
            Vector3 pos = target.transform.position;
            //Do not follow on Z (since it's 2D)
            pos.z = transform.position.z;

            if (!followY)
                pos.y = initialPos.y;
            if (!followX)
                pos.x = initialPos.x;

            transform.position = Vector3.Lerp(transform.position, pos, followSpeed * Time.deltaTime);
        }
    }

    void CameraShake(float amount)
    {
        Vector2 dir = new Vector2(Random.Range(-1, 1f), Random.Range(-1, 1f));
        dir.Normalize();

        transform.position += (Vector3)(dir * amount);
    }
}
