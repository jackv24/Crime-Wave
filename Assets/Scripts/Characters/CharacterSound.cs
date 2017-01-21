using UnityEngine;
using System.Collections;

public class CharacterSound : MonoBehaviour
{
    public AudioSource walkSoundSource;
    public AudioSource attackSoundSource;

    public AudioClip[] attackSounds;

    void Start()
    {
        walkSoundSource.Stop();
    }

    public void PlayWalkSound()
    {
        if(!walkSoundSource.isPlaying)
            walkSoundSource.Play();
    }

    public void StopWalkSound()
    {
        if (walkSoundSource.isPlaying)
            walkSoundSource.Stop();
    }

    public void PlayRandomAttack()
    {
        if (attackSounds.Length > 0)
            attackSoundSource.clip = attackSounds[Random.Range(0, attackSounds.Length)];

        attackSoundSource.Play();
    }
}
