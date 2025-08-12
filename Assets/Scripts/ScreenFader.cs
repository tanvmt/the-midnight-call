using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ScreenFader : MonoBehaviour
{
    private Image fadeImage;

    void Awake()
    {
        fadeImage = GetComponent<Image>();
        fadeImage.color = new Color(0, 0, 0, 0);
    }

    public IEnumerator FadeOut(float duration = 1f)
    {
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            fadeImage.color = new Color(0, 0, 0, timer / duration);
            yield return null;
        }
        fadeImage.color = new Color(0, 0, 0, 1);
    }

    public IEnumerator FadeIn(float duration = 1f)
    {
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            fadeImage.color = new Color(0, 0, 0, 1 - (timer / duration));
            yield return null;
        }
        fadeImage.color = new Color(0, 0, 0, 0);
    }
}
