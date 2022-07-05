using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageCameraPositionForScreenshot : MonoBehaviour
{
    public PolygonController PolygonController;
    public ManageLineConversion ManageLineConversion;
    public Transform Camera;
    public List<float> LineLengths;

    float MaxLineLength;
    public float ScaleIndex = 1;
    float CameraHeight;

    private void Start()
    {
        //float CameraHeight = ExtensionMethods.RemapValue(50.89f, 0f, 5000f, 0f, 200f);
        //Debug.Log("CameraHeight: " + CameraHeight);
    }

    public void SetCameraPositionAndRotation()
    {
        Debug.Log("SetCameraPositionAndRotation (1)");
        PointPolygon PointPolygon = PolygonController.mCurrentPolygonObj.GetComponent<PointPolygon>();
        if (PointPolygon != null)
        {
            Debug.Log("SetCameraPositionAndRotation (2)");

            MaxLineLength = GetMaxLineLength();
            Debug.Log("<color=green>GetMaxLineLength: </color>" + MaxLineLength);

            CameraHeight = ExtensionMethods.RemapValue(MaxLineLength, 0f, 5000f, 0f, 175f); // was 200
            Debug.Log("<color=green>CameraHeight: </color>" + CameraHeight);

            ScaleIndex = CameraHeight / 2f;
            if (ScaleIndex <= 1)
            {
                ScaleIndex = 1;
            }
            Debug.Log("<color=green>ScaleIndex: </color>" + ScaleIndex);


            Camera.position = new Vector3(0, CameraHeight, 0); // was 2

            Camera.SetParent(PointPolygon.textObj.transform.GetChild(0).transform.GetChild(0).transform);

            Camera.localPosition = new Vector3(0, 0, Camera.localPosition.z);

            Camera.localEulerAngles = Vector3.zero;//new Vector3(0, 0, 180); 

            Debug.Log("Camera localPosition: " + Camera.localPosition);
            DetachCamera();

            ManageLineConversion.ConvertLines();

            //StartCoroutine(ConvertLinesWithDelay());
        }
    }

    IEnumerator ConvertLinesWithDelay()
    {
        yield return new WaitForSeconds(0.2f);
        ManageLineConversion.ConvertLines();
    }


    float GetMaxLineLength()
    {
        int LineAmount = PolygonController.sLinesList.Count;

        LineLengths.Clear();

        for (int i = 0; i < LineAmount; i++)
        {
            LineLengths.Add(PolygonController.sLinesList[i].GetComponent<PointLine>().Length);
        }

        float maxValue = Mathf.Max(LineLengths.ToArray());
        return maxValue;
    }

    public void DetachCamera()
    {
        Debug.Log("DETACH CAMERA");
        Camera.transform.SetParent(null);
        // Camera.DetachChildren();
    }

}


public static class ExtensionMethods
{
    public static float RemapValue(this float aValue, float aIn1, float aIn2, float aOut1, float aOut2)
    {
        float t = (aValue - aIn1) / (aIn2 - aIn1);
        t = Mathf.Clamp01(t);
        return aOut1 + (aOut2 - aOut1) * t;
    }
}