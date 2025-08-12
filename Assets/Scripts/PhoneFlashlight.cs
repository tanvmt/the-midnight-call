using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PhoneFlashlight : MonoBehaviour
{
    [Header("Components")]
    public GameObject phoneModelObject;
    public Light lightSource;
    // public GameObject phoneScreenUI;
    public GameObject phoneAnimator;

    [Header("Settings")]
    public Key toggleKey = Key.F;
    private bool isPhoneUp = false;

    [Header("Sounds")]
    public AudioSource audioSource;
    public AudioClip raiseSound;
    public AudioClip lowerSound;

    void Start()
    {
        TurnOffLightAndUI();
        if (phoneAnimator != null)
        {
            phoneAnimator.GetComponent<Animator>().SetBool("IsPhoneUp", false);
            phoneAnimator.GetComponent<Animator>().Play("Idle");
        }
    }

    public void ShowPhone()
    {
        if (phoneModelObject != null)
        {
            phoneModelObject.SetActive(true);
        }
    }

    public void HidePhone()
    {
        if (phoneModelObject != null)
        {
            phoneModelObject.SetActive(false);
        }
    }

    public void ForceEnablePhone()
    {
        if (!isPhoneUp)
        {
            TogglePhone();
        }
    }

    public void ForceDisablePhone()
    {
        if(isPhoneUp)
        {
            TogglePhone();
        }
    }

    private void TogglePhone()
    {
        isPhoneUp = !isPhoneUp;
        phoneAnimator.GetComponent<Animator>().SetBool("IsPhoneUp", isPhoneUp);

        if (isPhoneUp)
        {
            // audioSource.PlayOneShot(raiseSound);
        }
        else
        {
            TurnOffLightAndUI();
            // audioSource.PlayOneShot(lowerSound);
        }
    }

    public void TurnOnLightAndUI()
    {
        if (lightSource != null) lightSource.enabled = true;
        // if(phoneScreenUI != null) phoneScreenUI.SetActive(true);
    }

    public void TurnOffLightAndUI()
    {
        if (lightSource != null) lightSource.enabled = false;
        // if(phoneScreenUI != null) phoneScreenUI.SetActive(false);
    }
    
    void Update()
    {
        // if(GameManager.Instance != null && GameManager.Instance.isPowerOut)
        // {
        //     if(Keyboard.current[toggleKey].wasPressedThisFrame)
        //     {
        //         TogglePhone();
        //     }
        // }

        if(GameManager.Instance != null)
        {
            if(Keyboard.current[toggleKey].wasPressedThisFrame)
            {
                TogglePhone();
            }
        }
    }
}
