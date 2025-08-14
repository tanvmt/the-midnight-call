using UnityEngine;
using System.Collections;

public class WeatherController : MonoBehaviour
{
    [Header("Rain Effect")]
    public ParticleSystem rainParticleSystem;
    public AudioSource rainAudioSource;

    [Header("Thunderstorm Effect")]
    public Light lightningLight;
    public AudioSource thunderAudioSource;
    public float minTimeBetweenThunder = 8f;
    public float maxTimeBetweenThunder = 20f;

    void Start()
    {
        if (lightningLight != null)
        {
            lightningLight.enabled = false;
        }

        if (rainParticleSystem != null)
        {
            rainParticleSystem.Play();
        }

        if (rainAudioSource != null)
        {
            rainAudioSource.Play();
        }

        StartCoroutine(ThunderstormRoutine());
    }

    private IEnumerator ThunderstormRoutine()
    {
        while (true)
        {
            float delay = Random.Range(minTimeBetweenThunder, maxTimeBetweenThunder);
            yield return new WaitForSeconds(delay);

            if (lightningLight != null)
            {
                lightningLight.enabled = true;
                Debug.Log("Lightning strike!");
                yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
                // lightningLight.enabled = false;
            }

            yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
            if (thunderAudioSource != null)
            {
                thunderAudioSource.Play();
            }
        }
    }
}
