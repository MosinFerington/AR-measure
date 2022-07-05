using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    ARMeasuring arMeasureScrip;

    void Start()
    {
        arMeasureScrip = FindObjectOfType<ARMeasuring>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //called when user drags the point
    public void Ondrag()
    {
        DebugConsole.DC.Log("Dragging");

        //transform.position = arMeasureScrip.hit.Pose.position;
    }
}
