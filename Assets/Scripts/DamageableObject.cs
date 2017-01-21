using UnityEngine;
using System.Collections;

public class DamageableObject : MonoBehaviour
{
    public int respectValue = 10;

    [Space()]
    public ParticleSystem particleEffect;

    public GameObject brokenVersion;

    [Space()]
    public AudioSource breakSoundSource;
    public float minPitchBend = -0.1f;
    public float maxPitchBend = 0.1f;

    [Space()]
    public float explosionRadius = 2f;
    public float explosionForce = 5f;
    public float maxTorque = 45f;

    [Space()]
    public float cameraShake = 2f;

    public void Destroy(float direction, CharacterStats stats)
    {
        if (particleEffect)
        {
            particleEffect.transform.SetParent(null);

            particleEffect.Play();

            if (direction < 0)
            {
                particleEffect.transform.Rotate(new Vector3(0, 180, 0), Space.World);
            }

            Destroy(particleEffect.gameObject, particleEffect.duration);
        }

        if(brokenVersion)
        {
            Instantiate(brokenVersion, transform.position, Quaternion.identity);
        }

        if(breakSoundSource)
        {
            breakSoundSource.transform.SetParent(null);

            breakSoundSource.pitch += Random.Range(minPitchBend, maxPitchBend);
            breakSoundSource.Play();

            Destroy(breakSoundSource, breakSoundSource.clip.length);
        }

        Camera.main.SendMessage("CameraShake", cameraShake);

        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D col in cols)
        {
            Rigidbody2D body = col.GetComponent<Rigidbody2D>();

            if(body)
            {
                Vector2 dir = (col.transform.position - transform.position).normalized;

                body.AddForceAtPosition(dir * explosionForce, transform.position, ForceMode2D.Impulse);
                body.AddTorque(Random.Range(-maxTorque, maxTorque), ForceMode2D.Impulse);
            }
        }

        stats.AddRespect(respectValue);

        Destroy(gameObject);
    }
}
