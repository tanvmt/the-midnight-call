using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class VehicleManager : MonoBehaviour
{
    [Header("Objects")]
    public GameObject playerCharacter;
    public CarController carController;
    public CinemachineCamera carCamera;
    public Transform driverSeatCameraAnchor;
    public DoorController carDoor;
    public ScreenFader screenFader;

    [Header("Settings")]
    public Transform exitPoint;
    private bool isDriving = true;
    private bool isTransitioning = false;

    [Header("Player Components")]
    public PhoneFlashlight phoneController;

    void Start()
    {
        playerCharacter.SetActive(false);
        carController.enabled = true;
        carCamera.Priority = 20;

        if (phoneController != null)
        {
            phoneController.HidePhone(); 
        }
    }

    void Update()
    {
        if (isDriving && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            StartCoroutine(ExitCarRoutine());
        }
    }

    public void EnterCar()
    {
        if (!isDriving && !isTransitioning)
        {
            StartCoroutine(EnterCarRoutine());
        }
    }

    private IEnumerator EnterCarRoutine()
    {
        isTransitioning = true;

        if (phoneController != null)
        {
            phoneController.ForceDisablePhone();
            phoneController.HidePhone();
        }

        if (carDoor != null) carDoor.Interact(null);
        yield return new WaitForSeconds(0.5f);

        yield return StartCoroutine(screenFader.FadeOut());

        isDriving = true;
        playerCharacter.SetActive(false);
        carController.enabled = true;
        carCamera.Priority = 20;

        if (driverSeatCameraAnchor != null)
        {
            driverSeatCameraAnchor.localRotation = Quaternion.identity;
        }

        if (carDoor != null) carDoor.Interact(null);

        yield return StartCoroutine(screenFader.FadeIn());

        isTransitioning = false;
    }

    private IEnumerator ExitCarRoutine()
    {
        isTransitioning = true;

        

        if (carDoor != null) carDoor.Interact(null);
        yield return new WaitForSeconds(0.5f);

        yield return StartCoroutine(screenFader.FadeOut());

        Vector3 finalExitPosition = exitPoint.position;

        isDriving = false;
        playerCharacter.SetActive(true);
        if (phoneController != null)
        {
            phoneController.ShowPhone(); 
        }
        carController.enabled = false;
        carCamera.Priority = 9;

        playerCharacter.transform.position = finalExitPosition;

        Vector3 carForwardDirection = carController.transform.forward;
        carForwardDirection.y = 0; 

        playerCharacter.transform.rotation = Quaternion.LookRotation(carForwardDirection);

        if (carDoor != null) carDoor.Interact(null);

        yield return StartCoroutine(screenFader.FadeIn());

        isTransitioning = false;
    }
}
