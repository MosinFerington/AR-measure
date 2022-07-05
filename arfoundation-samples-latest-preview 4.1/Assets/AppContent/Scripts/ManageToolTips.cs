using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ManageToolTips : MonoBehaviour
{
    [SerializeField]
    public List<ToolTip> ToolTipList;

    public GameObject ToolTipPanel;
    public Text ToolTipText;
    public Image ToolTipImage;
    public GameObject MeasurementsPanel;
    public Text MeasurementsText;
    public GameObject AddButton;
    int Counter = -1;

    public UnityEvent TriggerRoomNamePanelPopup;

    [System.Serializable]
    public class ToolTip
    {
        public string Text;
        public Sprite Icon;
    }

    void Start()
    {
        CheckToolTip();
        MeasurementsPanel.SetActive(false);
    }

    void ResetCounter()
    {
        Counter = -1;
        CheckToolTip();
    }

    public void CheckToolTip()
    {
        Debug.Log("Point Count: " + Counter);
        
        Counter++;
        if (Counter == 4) // was 3
        {
            if (MeasurementsPanel != null)
            {
              //  MeasurementsPanel.SetActive(true);
            }
            if (AddButton != null)
                AddButton.SetActive(false);

            if (ToolTipPanel != null)
                ToolTipPanel.SetActive(false);

            ResetCounter();

            TriggerRoomNamePanelPopup.Invoke();
        }
        else
        {
            
            ToolTipText.text = ToolTipList[Counter].Text;

            if (ToolTipList[Counter].Icon != null)
                ToolTipImage.sprite = ToolTipList[Counter].Icon;
            else
                ToolTipImage.sprite = null;
        }      
    }

    IEnumerator ToolTipDisable()
    {
        yield return new WaitForSeconds(3.0f);
    }

    public void ResetToolTips()
    {
        if (AddButton != null)
            AddButton.SetActive(true);
        if (ToolTipPanel != null)
            ToolTipPanel.SetActive(true);
        if (MeasurementsPanel != null)
            MeasurementsPanel.SetActive(false);

        ResetCounter();
    }
}
