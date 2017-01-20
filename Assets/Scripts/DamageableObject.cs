using UnityEngine;
using System.Collections;

public class DamageableObject : MonoBehaviour
{
    public ParticleSystem particleEffect;

    public float cameraShake = 2f;

    public void Destroy(float direction)
    {
        particleEffect.transform.SetParent(null);

        particleEffect.Play();

        if (direction < 0)
        {
            particleEffect.transform.Rotate(new Vector3(0, 180, 0), Space.World);

            Debug.Log("Rotated");
        }

        Camera.main.SendMessage("CameraShake", cameraShake);

        Destroy(gameObject);
    }
}
