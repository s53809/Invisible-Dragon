using UnityEngine;

public class SFXPlayer : MonoPooledObject
{
    private AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PrintSFX(AudioClip sfx)
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        audioSource.clip = sfx;
        audioSource.Play();

        Invoke("DisableThisObject", audioSource.clip.length);
    }

    private void DisableThisObject()
    {
        gameObject.SetActive(false);
    }
}
