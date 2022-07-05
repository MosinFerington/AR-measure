using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ManageLineConversion : MonoBehaviour
{
    public PolygonController PolygonController;
    public ManageCameraPositionForScreenshot ManageCameraPositionForScreenshot;
    public Material InitialLineMaterial;
    public Material BlackSolidLineMaterial;

    public Color InitialLabelColor;
    public Color BlackLabelColor;

    public Color WhiteTextColor;
    public Color BlackTextColor;

    float InitialLineWidth = 0.005f;
    float InitialLabelScale;

    public UnityEvent TriggerScreenshotAfterLinesConverted;

    void Start()
    {

    }

    // Trigger prior to saving room plan
    public void ConvertLines()
    {
        SetLinesMaterial(BlackSolidLineMaterial);

        SetLinesWidth(0.005f, ManageCameraPositionForScreenshot.ScaleIndex);

        SetLabelsColor(BlackLabelColor);
        SetTextColor(WhiteTextColor);

        TriggerScreenshotAfterLinesConverted.Invoke();
        //StartCoroutine(TriggerScreenshotAfterDelay());
    }

    IEnumerator TriggerScreenshotAfterDelay()
    {
        yield return new WaitForSeconds(0.2f);
        TriggerScreenshotAfterLinesConverted.Invoke();
    }

    // Trigger after room plan is saved or "remove" button is used
    public void ResetLines()
    {
        SetLinesMaterial(InitialLineMaterial);

        SetLinesWidth(0.005f, 1f);

        SetLabelsColor(InitialLabelColor);
        SetTextColor(WhiteTextColor);
    }



    void SetLinesWidth(float OriginalLineWidth, float Scale)
    {
        int CountLines = PolygonController.sLinesList.Count;
        float width = OriginalLineWidth * Scale;
        for (int i = 0; i < CountLines; i++)
        {
            LineRenderer LineRenderer = PolygonController.sLinesList[i].GetComponent<LineRenderer>();
            
            LineRenderer.startWidth = width;
            LineRenderer.endWidth = width;
            PolygonController.sLinesList[i].GetComponent<PointLine>().cavansTextObj.transform.localScale = Vector3.one * Scale;
        }

        PolygonController.sPolygonList[0].GetComponent<PointPolygon>().textObj.transform.localScale = Vector3.one * Scale;
    }

    void SetLinesMaterial(Material LineMaterial)
    {
        int CountLines = PolygonController.sLinesList.Count;

        for (int i = 0; i < CountLines; i++)
        {
            PolygonController.sLinesList[i].GetComponent<LineRenderer>().sharedMaterial = LineMaterial;
        }

    }


    void SetLabelsColor(Color LabelColor)
    {
        int CountLines = PolygonController.sLinesList.Count;

        for (int i = 0; i < CountLines; i++)
        {
            PolygonController.sLinesList[i].transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().color = LabelColor;
        }
    }

    void SetTextColor(Color TextColor)
    {
        int CountLines = PolygonController.sLinesList.Count;

        for (int i = 0; i < CountLines; i++)
        {
            PolygonController.sLinesList[i].transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().color = TextColor;
        }
    }

}
