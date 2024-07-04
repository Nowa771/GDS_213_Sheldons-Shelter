using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomSelectionManager : MonoBehaviour
{
    public static RoomSelectionManager Instance;

    public GameObject roomStatsPanel;
    public Text roomNameText;
    public Text roomStatsText;
    public Button closeButton;

    private bool isPanelActive = false; // Track the state of the panel

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        roomStatsPanel.SetActive(false);
        closeButton.onClick.AddListener(ToggleRoomStats); // Change to toggle method
    }

    public void ShowRoomStats(Room room)
    {
        roomNameText.text = room.roomName;
        roomStatsText.text = room.GetRoomStats();
        ToggleRoomStats(); // Toggle visibility
    }

    public void ToggleRoomStats()
    {
        isPanelActive = !isPanelActive; // Toggle the panel state

        if (isPanelActive)
        {
            roomStatsPanel.SetActive(true);
        }
        else
        {
            roomStatsPanel.SetActive(false);
        }
    }
}