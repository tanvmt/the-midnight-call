using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Settings")]
    public List<Light> motelLights;
    public PhoneFlashlight phoneController;
    public bool isPowerOut = false;

    [Header("UI Managers")]
    public TaskManagerUI taskManager;
    public PlayerMonologueUI monologueManager;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void InitiatePowerOutage()
    {
        if (isPowerOut) return;

        isPowerOut = true;

        Debug.Log("Power outage initiated.");
        foreach (Light light in motelLights)
        {
            light.enabled = false;
        }

        if (phoneController != null)
        {
            phoneController.ForceEnablePhone();
        }

        taskManager.SetNewTask("Find the breaker box to restore power.");
    }

    public void RestorePower()
    {
        if (!isPowerOut) return;

        isPowerOut = false;

        Debug.Log("Power restored.");
        foreach (Light light in motelLights)
        {
            light.enabled = true;
        }

        if (phoneController != null)
        {
            phoneController.ForceDisablePhone();
        }

        // if (playerFlashlight != null)
        // {
        //     playerFlashlight.SetActive(false);
        // }
    }

}
