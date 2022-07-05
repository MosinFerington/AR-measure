using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SetRoomName : MonoBehaviour
{
    public PolygonController PolygonController;
    public GameObject RoomNameInputPanel;
    public InputField RoomNameInput;

    public UnityEvent SetCameraPosition;


    public void ConfirmRoomName()
    {
        RoomNameInputPanel.SetActive(false);
        PolygonController.SetRoomName(RoomNameInput.text);
        SetCameraPosition.Invoke();
        RoomNameInput.text = "";
    }
}