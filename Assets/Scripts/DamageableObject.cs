using UnityEngine;
using System.Collections;

public class DamageableObject : MonoBehaviour
{
    public ParticleSystem particleEffect;

    public float explosionRadius = 2f;
    public float explosionForce = 5f;
    public float maxTorque = 45f;

    public float cameraShake = 2f;

    public void Destroy(float direction)
    {
        particleEffect.transform.SetParent(null);

        particleEffect.Play();

        if (direction < 0)
        {
            particleEffect.transform.Rotate(new Vector3(0, 180, 0), Space.World);
        }

        Camera.main.SendMessage("CameraShake", cameraShake);

        Destroy(particleEffect.gameObject, particleEffect.duration);

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

        Destroy(gameObject);
    }
}
