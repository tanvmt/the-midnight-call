using UnityEngine;
using TMPro;
using System;

public class DialogueUI : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public GameObject continuePrompt;

    void Start()
    {
        CloseDialogue();
    }

    public void ShowDialogue(string text)
    {
        dialoguePanel.SetActive(true);
        dialogueText.text = text;
    }

    public void SetContinuePrompt(bool active)
    {
        continuePrompt.SetActive(active);
    }

    public void CloseDialogue()
    {
        dialoguePanel.SetActive(false);
    }
}
