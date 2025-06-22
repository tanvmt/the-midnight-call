using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private List<string> collectedKeys = new List<string>();

    public void AddKey(string keyId)
    {
        if (!collectedKeys.Contains(keyId))
        {
            collectedKeys.Add(keyId);
            Debug.Log("Key collected: " + keyId);
        }
    }

    public bool HasKey(string keyId)
    {
        return collectedKeys.Contains(keyId);
    }
}
