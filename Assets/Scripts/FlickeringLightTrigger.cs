using System.Collections;
using UnityEngine;

public class FlickeringLightTrigger : MonoBehaviour
{
    public Light targetLight;
    public float flickerDuration = 3f;

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (targetLight == null)
        {
            Debug.LogError("No Target Light in Inspector", this.gameObject);
            return;
        }

        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            StartCoroutine(FlickerTheLight());
        }
    }

    private IEnumerator FlickerTheLight()
    {
        GetComponent<Collider>().enabled = false;

        float timer = 0f;
        while (timer < flickerDuration)
        {
            targetLight.enabled = !targetLight.enabled;
            
            timer += flickerDuration / 10;
            yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
        }

        targetLight.enabled = true;
        Destroy(gameObject);
    }
}