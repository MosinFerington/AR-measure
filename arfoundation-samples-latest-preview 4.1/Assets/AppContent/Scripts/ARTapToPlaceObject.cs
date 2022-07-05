using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System;
using UnityEngine.EventSystems;

public class ARTapToPlaceObject : MonoBehaviour
{
    ARSession ARSession;
    ARPlaneManager ARPlaneManager;
    ARPointCloudManager ARPointCloudManager;
    ARRaycastManager ARRaycastManager;
    //public GameObject placementIndicator;
    public GameObject InfoPanel;
    public GameObject BottomPanel;
    public GameObject ToolTipPanel;
    private Pose placementPose;
    private bool placementPoseIsValid = false;

    bool PlacedAR = false;
    int fingerID;

    void Awake()
    {
#if !UNITY_EDITOR
			fingerID = 0;
#endif

#if UNITY_EDITOR
        fingerID = -1;
#endif
    }

    void Start()
    {
        ARRaycastManager = FindObjectOfType<ARRaycastManager>();
        ARPlaneManager = FindObjectOfType<ARPlaneManager>();
        ARPointCloudManager = FindObjectOfType<ARPointCloudManager>();
        ARSession = FindObjectOfType<ARSession>();
        InfoPanel.SetActive(true);
        BottomPanel.SetActive(false);
        ToolTipPanel.SetActive(false);

        /*
        if (ShowPlanes)
            CheckPlanes(true);
        else
            CheckPlanes(false);

        if (ShowPoints)
            CheckPoints(true);
        else
            CheckPoints(false);
            */
    }

    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();

        if (placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)// && PlacedAR == false)
        {
            if (EventSystem.current.IsPointerOverGameObject(fingerID))
            {
                Debug.Log("UI touched");
                return;
            }
            else
            {
                PlaceObject();
            }
        }
    }

    public void PlaceObject()
    {
        if (placementPoseIsValid)
        {
            PlacedAR = true;
            InfoPanel.SetActive(false);
            BottomPanel.SetActive(true);
            ToolTipPanel.SetActive(true);
        }
        else
        {

        }
    }

    public void Reset()
    {
        PlacedAR = false;
        CheckPlanes(true);
        InfoPanel.SetActive(true);
        BottomPanel.SetActive(false);
        ToolTipPanel.SetActive(false);
        //  placementIndicator.SetActive(true);
    }

    private void UpdatePlacementPose()
    {
        if (Camera.current != null)
        {
            var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
            var hits = new List<ARRaycastHit>();
            ARRaycastManager.Raycast(screenCenter, hits, TrackableType.PlaneWithinPolygon | TrackableType.PlaneEstimated); // TrackableType.PlaneWithinBounds);

            placementPoseIsValid = hits.Count > 0;
            if (placementPoseIsValid)
            {
                placementPose = hits[0].pose;

                var cameraForward = Camera.current.transform.forward;
                var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
                placementPose.rotation = Quaternion.LookRotation(cameraBearing);
            }
        }
    }

    private void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid)
        {
            InfoPanel.SetActive(false);
            BottomPanel.SetActive(true);
            ToolTipPanel.SetActive(true);
            /* if (Content.activeSelf == false)
            {
                SetModelButton.SetActive(true);
                placementIndicator.SetActive(true);
            }*/
        }
        else
        {
            InfoPanel.SetActive(true);
            BottomPanel.SetActive(false);
            ToolTipPanel.SetActive(false);
            //  placementIndicator.SetActive(false);
        }
    }

    void CheckPlanes(bool status)
    {
        ARPlaneManager.enabled = status;
        foreach (var plane in ARPlaneManager.trackables)
        {
            plane.gameObject.SetActive(status);
        }
    }

    void CheckPoints(bool status)
    {
        ARPointCloudManager.enabled = status;
        foreach (var Point in ARPointCloudManager.trackables)
        {
            Point.gameObject.SetActive(status);
        }
    }

}