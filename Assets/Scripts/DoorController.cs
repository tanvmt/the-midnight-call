using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public bool isLocked = false;
    public string requiredKeyId = "";

    private bool isOpen = false;
    public bool IsOpen { get { return isOpen; } }
    public Quaternion initialLocalRotation;
    public float openAngle = 90f;

    public float animationTime = 1f;
    private bool isAnimating = false;

    void Start()
    {
        initialLocalRotation = transform.localRotation;
    }

    public void Interact(PlayerInventory playerInventory)
    {
        if(isAnimating)
            return;

        if (isLocked)
        {
            if(playerInventory!=null && playerInventory.HasKey(requiredKeyId))
            {
                isLocked = false;
                Debug.Log("Door unlocked with key: " + requiredKeyId);
            }
            else
            {
                Debug.Log("Door is locked. You need the key: " + requiredKeyId);
                return;
            }
        }

        isOpen = !isOpen;

        StartCoroutine(AnimateDoor());
    }

    private IEnumerator AnimateDoor()
    {
        isAnimating = true;

        Quaternion startRotation = transform.localRotation;
        Quaternion endRotation;

        if (isOpen)
        {
            endRotation = initialLocalRotation * Quaternion.Euler(0, openAngle, 0);
        }
        else
        {
            endRotation = initialLocalRotation;
        }

        float elapesedTime = 0f;
        while (elapesedTime < animationTime)
        {
            elapesedTime += Time.deltaTime;
            float t = elapesedTime / animationTime;

            transform.localRotation = Quaternion.Slerp(startRotation, endRotation, t);
            yield return null;
        }

        transform.localRotation = endRotation; 
        isAnimating = false;
    }
}
