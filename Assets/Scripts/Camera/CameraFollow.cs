using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    [Space()]
    public float followSpeed = 10f;

    void Start()
    {
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
            Vector3 pos = transform.position;

            //Smoothly follow target
            pos = Vector3.Lerp(pos, target.transform.position, followSpeed * Time.deltaTime);
            //Do not follow on Z (since it's 2D)
            pos.z = transform.position.z;

            transform.position = pos;
        }
    }
}
