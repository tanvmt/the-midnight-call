using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionDistance = 3f;
    public Camera playerCamera;
    private PlayerInventory playerInventory;

    void Start()
    {
        playerInventory = GetComponentInParent<PlayerInventory>();
    }

    void Update()
    {
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, interactionDistance))
        {
            Debug.Log("Hit: " + hitInfo.collider.name);
            if(hitInfo.collider.CompareTag("Interactable"))
            {
                if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
                {
                    Debug.Log("Interacting with: " + hitInfo.collider.name);

                    KeyItem keyItem = hitInfo.collider.GetComponent<KeyItem>();
                    if (keyItem != null)
                    {
                        playerInventory.AddKey(keyItem.keyId);
                        Destroy(hitInfo.collider.gameObject);
                        return;
                    }

                    DoorController door = hitInfo.collider.GetComponent<DoorController>();

                    if (door != null)
                    {
                        door.Interact(playerInventory);
                    }
                }
            }
        }
    }
}
