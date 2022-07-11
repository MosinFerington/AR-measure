using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


public class ARMeasuring : MonoBehaviour
{
    public bool DebugMode = false;

    private ARRaycastManager rayCastManager;
    private GraphicRaycaster graphicRaycaster;
    private Actions actions;

    void Start()
    {
        rayCastManager = FindObjectOfType<ARRaycastManager>();
        graphicRaycaster = FindObjectOfType<GraphicRaycaster>();

        actions = FindObjectOfType<Actions>();
    }

    void Update()
    {
        /// 1. Init tap info ///

        Vector2 tapPosition;
        var pointerEventData = new PointerEventData(null);

        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            if (touch.phase != TouchPhase.Began)
            {
                return;
            }

            tapPosition = touch.position;
            pointerEventData.pointerId = touch.fingerId;
        }
        if (Input.GetMouseButtonDown(0))
        {
            tapPosition = Input.mousePosition;
        }
        else
        {
            return;
        }

        /// 2. Block UI click ///

        pointerEventData.position = tapPosition;
        var result = new List<RaycastResult>();

        graphicRaycaster.Raycast(pointerEventData, result);
        if (result.Any(el => el.gameObject.CompareTag("BlockableUI")))
        {
            return;
        }

        /// 3. Process tap ///

        if (DebugMode == false)
        {
            var rayHit = new List<ARRaycastHit>();
            rayCastManager.Raycast(new Vector2(tapPosition.x, tapPosition.y), rayHit, TrackableType.Planes);

            // If hit AR plane.
            if (rayHit.Count > 0)
            {
                // Check if we hit any stickable object.
                GameObject hitObject = null;

                Ray ray = Camera.main.ScreenPointToRay(tapPosition);

                if (Physics.Raycast(ray, out RaycastHit physicHit, Mathf.Infinity, LayerMask.GetMask("Stickable")))
                {
                    hitObject = physicHit.transform.gameObject;
                }

                actions.PerfromTouch(tapPosition, rayHit[0].pose.position, rayHit[0].pose.up, hitObject);
            }
        }
        else
        {
            Ray ray = Camera.main.ScreenPointToRay(tapPosition);

            // If hit plane.
            if (Physics.Raycast(ray, out RaycastHit planeHit, Mathf.Infinity, LayerMask.GetMask("Plane")))
            {
                // Check if we hit any stickable object.
                GameObject hitObject = null;

                if (Physics.Raycast(ray, out RaycastHit skickableHit, Mathf.Infinity, LayerMask.GetMask("Stickable")))
                {
                    hitObject = skickableHit.transform.gameObject;
                }

                actions.PerfromTouch(tapPosition, planeHit.point, planeHit.normal, hitObject);
            }
        }
    }
}
