using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using ARMeasure;

public class AnchorPoint : MonoBehaviour {

    bool isActive = false;
    Vector3 screenCenter = new Vector3();

    public delegate void TrackPlane();
    public event TrackPlane trackPlaneEvent;
    
    public ARPlaneManager m_PlaneMgr;
    public ARRaycastManager m_RaycastManager;

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
    
    // Use this for initialization
    void Start()
    {
        if (!m_PlaneMgr)
        {
            m_PlaneMgr = GameObject.FindObjectOfType<ARPlaneManager>();//
        }

        if (!m_RaycastManager)
        {
            m_RaycastManager = GameObject.FindObjectOfType<ARRaycastManager>();//
        }
        
        screenCenter.Set(Screen.width / 2, Screen.height / 2,0);
        m_PlaneMgr.planesChanged += AddAnchor;
    }

    void AddAnchor(ARPlanesChangedEventArgs args)
    {
        if(trackPlaneEvent != null)
        {
            trackPlaneEvent();
        }
        isActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {

            if (m_RaycastManager.Raycast(screenCenter, s_Hits, TrackableType.PlaneWithinPolygon| TrackableType.PlaneEstimated))
            {
                Pose hitPose = s_Hits[0].pose;

                this.transform.SetPositionAndRotation(hitPose.position, hitPose.rotation);

                if (ARMeasureManager.instance.mMeasureMode == MeasureMode.MeasureAngle ||
                    ARMeasureManager.instance.mMeasureMode == MeasureMode.MeasureLength||
                    ARMeasureManager.instance.mMeasureMode == MeasureMode.MeasurePolygon)
                {
                    ARMeasureManager.instance.updateState(transform.position);
                }
            }
        }
    }

    public void AddPoint()
    {
        Vector3 pos = this.transform.position;
        ARMeasureManager.instance.AddPoint(this.transform.position);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("LDCStartMenu");
    }

}
