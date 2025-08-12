using UnityEngine;
using TMPro;

public class TaskManagerUI : MonoBehaviour
{
    public TextMeshProUGUI taskText;
    public GameObject taskPanel;

    void Start()
    {
        taskPanel.SetActive(false);
    }

    public void SetNewTask(string taskDescription)
    {
        taskText.text = "- " + taskDescription;
        taskPanel.SetActive(true);
    }

    public void HideTask()
    {
        taskPanel.SetActive(false);
    }
}
