using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class NPCInteraction : MonoBehaviour
{
    [Header("UI References")]
    public DialogueUI dialogueUI;

    [Header("Dialogue Settings")]
    [TextArea(3, 10)]
    public string[] dialogueLines;

    [Header("Item to Give")]
    public KeyItem keyToGive;

    private bool playerInRange = false;
    private bool dialogueActive = false;
    private bool dialogueFinished = false;

    void Update()
    {
        if (playerInRange && !dialogueActive && !dialogueFinished && Keyboard.current.eKey.wasPressedThisFrame)
        {
            StartCoroutine(RunDialogue());
        }
    }

    private IEnumerator RunDialogue()
    {
        dialogueActive = true;
        int currentLine = 0;

        while (currentLine < dialogueLines.Length)
        {
            dialogueUI.ShowDialogue(dialogueLines[currentLine]);
            dialogueUI.SetContinuePrompt(false);

            yield return new WaitForSeconds(0.5f);
            dialogueUI.SetContinuePrompt(true);

            yield return new WaitUntil(() => Keyboard.current.eKey.wasPressedThisFrame);

            currentLine++;
        }

        dialogueUI.CloseDialogue();
        GiveKeyToPlayer();

        dialogueActive = false;
        dialogueFinished = true;
    }

    private void GiveKeyToPlayer()
    {
        if (keyToGive != null)
        {
            PlayerInventory playerInventory = FindFirstObjectByType<PlayerInventory>();
            if (playerInventory != null)
            {
                playerInventory.AddKey(keyToGive.keyId);
                Debug.Log($"Key {keyToGive.keyId} given to player.");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Player entered NPC interaction range.");
            dialogueUI.ShowDialogue("Press 'E' to talk to the NPC.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Player exited NPC interaction range.");
            dialogueUI.CloseDialogue();
        }
    }
}
