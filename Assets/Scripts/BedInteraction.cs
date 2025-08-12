using UnityEngine;

public class BedInteraction : MonoBehaviour
{
    [Header("Required Objects")]
    public DoorController roomDoor;

    private bool hasTriggeredPowerOutage = false;

    public void OnInteract()
    {
        if (hasTriggeredPowerOutage) return;

        if (roomDoor.IsOpen)
        {
            HandleDoorIsOpen();
        }
        else
        {
            HandleDoorIsClosed();
        }
    }

    private void HandleDoorIsOpen()
    {
        GameManager gm = FindFirstObjectByType<GameManager>();
        if (gm != null)
        {
            gm.monologueManager.ShowMonologue("I should probably lock the door first. This place feels... off.");
            gm.taskManager.SetNewTask("Lock the room door.");
        }
    }

    private void HandleDoorIsClosed()
    {
        GameManager gm = FindFirstObjectByType<GameManager>();
        if (gm != null)
        {
            gm.taskManager.HideTask();

            gm.monologueManager.ShowMonologue("Alright, time to get some sleep. What a long day.");

            gm.InitiatePowerOutage();
            hasTriggeredPowerOutage = true;
        }
    }
}
