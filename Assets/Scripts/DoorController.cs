using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public bool isLocked = false;
    public string requiredKeyId = "";

    private bool isOpen = false;
    public Quaternion initialRotation;
    public float openAngle = 90f;

    public float animationTime = 1f;
    private bool isAnimating = false;

    void Start()
    {
        initialRotation = transform.rotation;
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

        Quaternion startRotation = transform.rotation;
        Quaternion endRotation;

        if (isOpen)
        {
            endRotation = initialRotation * Quaternion.Euler(0, openAngle, 0);
        }
        else
        {
            endRotation = initialRotation;
        }

        float elapesedTime = 0f;
        while (elapesedTime < animationTime)
        {
            elapesedTime += Time.deltaTime;
            float t = elapesedTime / animationTime;

            transform.rotation = Quaternion.Slerp(startRotation, endRotation, t);
            yield return null;
        }

        transform.rotation = endRotation; 
        isAnimating = false;
    }
}
