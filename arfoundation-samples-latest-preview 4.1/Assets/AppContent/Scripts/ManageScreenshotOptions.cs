using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManageScreenshotOptions : MonoBehaviour
{
    public Snapshot Snapshot;
    public List<Sprite> RoomPlanSprites;
    public GameObject RoomPlanOptionPrefab;

    public GameObject RoomPlanPanel;
    public Image RoomPlanMainImage;
    private void Start()
    {
        RoomPlanOptionPrefab.SetActive(false);
    }

    public void AddRoomPlan()
    {
        Sprite RoomPlan = Sprite.Create(Snapshot.ImageToSave, new Rect(0, 0, Snapshot.ImageToSave.width, Snapshot.ImageToSave.height), new Vector2(0.5f, 0.5f), 100f);
        RoomPlanSprites.Add(RoomPlan);
        RoomPlanOptionPrefab.transform.GetChild(0).GetComponent<Image>().sprite = RoomPlan as Sprite;

        GameObject Prefab = Instantiate(RoomPlanOptionPrefab, RoomPlanOptionPrefab.transform.parent);

        Prefab.name = Prefab.transform.GetSiblingIndex().ToString();

        Prefab.SetActive(true);
    }

    public void OpenRoomPlan(GameObject RoomOption)
    {
        int index = RoomOption.transform.GetSiblingIndex()-1;

        Debug.Log("Room Option to Open: " + index);

        RoomPlanMainImage.sprite = RoomPlanSprites[index];

        RoomPlanPanel.SetActive(true);
    }
}
