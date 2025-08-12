using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerMonologueUI : MonoBehaviour
{
    public TextMeshProUGUI monologueText;
    public GameObject monologuePanel;
    public float displayDuration = 5f;

    private Coroutine displayCoroutine;

    void Start()
    {
        monologuePanel.SetActive(false);
    }

    public void ShowMonologue(string text)
    {
        if (displayCoroutine != null)
        {
            StopCoroutine(displayCoroutine);
        }

        displayCoroutine = StartCoroutine(DisplayMonologue(text));
    }

    private IEnumerator DisplayMonologue(string text)
    {
        monologueText.text = "<i>" + text + "</i>";
        monologuePanel.SetActive(true);

        yield return new WaitForSeconds(displayDuration);

        monologuePanel.SetActive(false);
    }
}
