using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionDistance = 3f;
    public Camera playerCamera;
    private PlayerInventory playerInventory;
    private VehicleManager vehicle;

    void Start()
    {
        playerInventory = FindFirstObjectByType<PlayerInventory>();
        vehicle = FindFirstObjectByType<VehicleManager>();

    }

    void Update()
    {
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, interactionDistance))
        {
            if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
            {
                Debug.Log("Interacting with: " + hitInfo.collider.name);

                if (hitInfo.collider.CompareTag("PlayerCar"))
                {
                    if (vehicle != null)
                    {
                        vehicle.EnterCar();
                        return;
                    }
                }

                KeyItem keyItem = hitInfo.collider.GetComponent<KeyItem>();
                if (keyItem != null)
                {
                    Debug.Log($"Key {keyItem.keyId} collected.");
                    playerInventory.AddKey(keyItem.keyId);
                    Destroy(hitInfo.collider.gameObject);
                    return;
                }

                DoorController door = hitInfo.collider.GetComponent<DoorController>();
                if (door != null)
                {
                    door.Interact(playerInventory);
                    return;
                }

                BedInteraction bed = hitInfo.collider.GetComponent<BedInteraction>();
                if (bed != null)
                {
                    bed.OnInteract();
                    return;
                }
            }
        }
    }
}
