using System.Linq;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


public class ARMeasuring : MonoBehaviour
{
    public bool DebugMode = false;
    public bool OnButtonClick = false;

    ARRaycastManager rayCastManager;
    Actions actions;

    void Start()
    {
        rayCastManager = FindObjectOfType<ARRaycastManager>();
        
        actions = FindObjectOfType<Actions>();
        actions.debugMode = DebugMode;
    }

    void Update()
    {
        if (DebugMode == false)
        {
            if (Input.touchCount < 1)
            {
                return;
            }

            var touch = Input.GetTouch(0);

            if (touch.phase != TouchPhase.Began)
            {
                return;
            }

            if (OnButtonClick)
            {
                OnButtonClick = false;
                return;
            }

            var rayHit = new List<ARRaycastHit>();
            rayCastManager.Raycast(new Vector2(touch.position.x, touch.position.y), rayHit, TrackableType.Planes);

            // If hit AR plane.
            if (rayHit.Count > 0)
            {
                // Check if we hit any object.
                GameObject hitObject = null;

                Ray ray = Camera.main.ScreenPointToRay(touch.position);

                if (Physics.Raycast(ray, out RaycastHit physicHit, Mathf.Infinity, LayerMask.GetMask("Stickable")))
                {
                    hitObject = physicHit.transform.gameObject;
                }

                actions.PerfromTouch(Input.GetTouch(0).position, rayHit[0].pose.position, rayHit[0].pose.up, hitObject);
            }
        }
        else
        {
            Vector2 tapPosition;

            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);
                if (touch.phase != TouchPhase.Began)
                {
                    return;
                }

                tapPosition = touch.position;
            }
            if (Input.GetMouseButtonDown(0))
            {
                tapPosition = Input.mousePosition;
            }
            else
            {
                return;
            }

            if (OnButtonClick)
            {
                OnButtonClick = false;
                return;
            }

            Ray ray = Camera.main.ScreenPointToRay(tapPosition);

            if (Physics.Raycast(ray, out RaycastHit planeHit, Mathf.Infinity, LayerMask.GetMask("Plane")))
            {
                // Check if we hit any object.
                GameObject hitObject = null;

                if (Physics.Raycast(ray, out RaycastHit skickableHit, Mathf.Infinity, LayerMask.GetMask("Stickable")))
                {
                    hitObject = skickableHit.transform.gameObject;
                }

                actions.PerfromTouch(Input.GetTouch(0).position, planeHit.point, planeHit.normal, hitObject);
            }
        }
        
    }
}
