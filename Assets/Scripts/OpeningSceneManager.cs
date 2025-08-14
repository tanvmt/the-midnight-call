using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.InputSystem;
using StarterAssets;

public class OpeningSceneManager : MonoBehaviour
{
    private enum SceneState { Narration, WakingUp, WaitingForAnswer, PhoneCall, WaitingForDoor, SceneEnd }
    private SceneState currentState;

    [Header("Narration & Subtitle Components")]
    public TextMeshProUGUI narrationText;
    public DialogueUI dialogueUI;
    public ScreenFader screenFader;

    [Header("Player Components")]
    public GameObject player;
    public FirstPersonController playerController;
    public Animator playerAnimator;
    public string wakeUpTriggerName = "StartWakingUp";
    public float wakeUpAnimationDuration = 5f;

    [Header("Audio Sources")]
    public AudioSource phoneRingingSound;
    public AudioSource callDialogueSound;

    [Header("Interaction Settings")]
    public Camera playerCamera;
    public float interactionDistance = 3f;
    public string phoneTag = "Phone";
    public string doorTag = "Door";

    [Header("Interaction UI")]
    public GameObject interactionPromptUI;

    [Header("Scene Transition Settings")]
    public string nextSceneName = "MotelScene";

    void Start()
    {
        currentState = SceneState.Narration;
        narrationText.text = "";
        if (dialogueUI != null)
        {
            dialogueUI.CloseDialogue();
        }
        else
        {
            Debug.LogError("DialogueUI has not been assigned in the Inspector!");
        }

        if (interactionPromptUI != null)
        {
            interactionPromptUI.SetActive(false);
        }
        
        if (player != null)
        {
            playerAnimator = player.GetComponent<Animator>();
        }
        else
        {
            Debug.LogError("Player Animator has not been assigned in the Inspector!");
            return;
        }

        if (screenFader == null)
        {
            Debug.LogError("ScreenFader has not been assigned in the Inspector!");
            return;
        }

        if (playerController != null)
        {
            playerController.enabled = false; // Disable player control during narration
        }
        else
        {
            Debug.LogError("Player Controller has not been assigned in the Inspector!");
        }

        StartCoroutine(OpeningSequence());
    }

    void Update()
    {
        if(currentState == SceneState.WaitingForAnswer || currentState == SceneState.WaitingForDoor)
        {
            HandleInteractionPrompt();
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                TryInteract();
            }
        }
    }

    private IEnumerator OpeningSequence()
    {
        yield return StartCoroutine(screenFader.FadeOut(0f));
        yield return StartCoroutine(FadeNarrationText("The Millers. That's all I ever knew of them...", 1.5f, 1.5f, 1f));
        yield return StartCoroutine(FadeNarrationText("...a name, and a mystery left behind.", 1.5f, 1.5f, 1f));
        narrationText.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        if (phoneRingingSound != null) phoneRingingSound.Play();
        yield return new WaitForSeconds(3f); 
        yield return StartCoroutine(screenFader.FadeIn(2f));

        currentState = SceneState.WakingUp;

        if (playerAnimator != null)
        {
            playerAnimator.SetTrigger(wakeUpTriggerName);
        }
        else
        {
            Debug.LogError("Player Animator is not assigned or does not have the specified trigger!");
        }

        yield return new WaitForSeconds(wakeUpAnimationDuration);

        // SnapPlayerToGround();

        if (playerController != null)
        {
            playerController.enabled = true; 
        }
        
        currentState = SceneState.WaitingForAnswer;
    }
    
    private void SnapPlayerToGround()
    {
        if (playerController == null) return;

        CharacterController controller = playerController.GetComponent<CharacterController>();
        Transform playerTransform = playerController.transform;

        if (controller == null)
        {
            Debug.LogError("CharacterController not found on the player object!");
            return;
        }

        float raycastDistance = 2f;
        RaycastHit hit;

        if (Physics.Raycast(playerTransform.position, Vector3.down, out hit, raycastDistance))
        {
            Vector3 targetPosition = hit.point;

            playerTransform.position = targetPosition;
            
            Debug.Log("Player position snapped to ground at: " + targetPosition);
        }
        else
        {
            Debug.LogWarning("SnapPlayerToGround: Raycast did not hit any ground below the player. The player might still fall.");
        }
    }
    
    private IEnumerator FadeNarrationText(string text, float fadeInTime, float fadeOutTime, float delay)
    {
        narrationText.text = text;

        // Fade In
        float timer = 0f;
        while (timer < fadeInTime)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Clamp01(timer / fadeInTime);
            narrationText.color = new Color(narrationText.color.r, narrationText.color.g, narrationText.color.b, alpha);
            yield return null;
        }

        // Delay
        yield return new WaitForSeconds(delay);

        // Fade Out
        timer = 0f;
        while (timer < fadeOutTime)
        {
            timer += Time.deltaTime;
            float alpha = 1f - Mathf.Clamp01(timer / fadeOutTime);
            narrationText.color = new Color(narrationText.color.r, narrationText.color.g, narrationText.color.b, alpha);
            yield return null;
        }
    }

    private IEnumerator PhoneCallSequence()
    {
        currentState = SceneState.PhoneCall;
        if (interactionPromptUI != null) interactionPromptUI.SetActive(false);
        if (phoneRingingSound != null) phoneRingingSound.Stop();
        if (callDialogueSound != null) callDialogueSound.Play();

        yield return new WaitForSeconds(1f);
        dialogueUI.ShowDialogue("The Miller family never left...");

        yield return new WaitForSeconds(3f);
        dialogueUI.ShowDialogue("...the truth is buried at the Silent Dawn Motel.");

        yield return new WaitForSeconds(3f);
        dialogueUI.ShowDialogue("You have to go there.");

        yield return new WaitForSeconds(2f);
        if (callDialogueSound != null) callDialogueSound.Stop();
        dialogueUI.ShowDialogue("[Call Ended]");

        yield return new WaitForSeconds(2f);
        dialogueUI.CloseDialogue();

        currentState = SceneState.WaitingForDoor;
    }

    private void HandleInteractionPrompt()
    {
        if (interactionPromptUI == null || playerCamera == null) return;

        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, interactionDistance))
        {
            bool isLookingAtPhone = currentState == SceneState.WaitingForAnswer && hit.collider.CompareTag(phoneTag);
            bool isLookingAtDoor = currentState == SceneState.WaitingForDoor && hit.collider.CompareTag(doorTag);
            if (isLookingAtPhone || isLookingAtDoor)
            {
                interactionPromptUI.SetActive(true);
                return;
            }
            else
            {
                interactionPromptUI.SetActive(false);
            }
        }
    }

    private void TryInteract()
    {
        if (playerCamera == null) return;

        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, interactionDistance))
        {
            if (currentState == SceneState.WaitingForAnswer && hit.collider.CompareTag(phoneTag))
            {
                StartCoroutine(PhoneCallSequence());
            }
            else if (currentState == SceneState.WaitingForDoor && hit.collider.CompareTag(doorTag))
            {
                currentState = SceneState.SceneEnd;
                SceneManager.LoadScene(nextSceneName);
            }
        }
    }
}
