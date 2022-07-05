using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPanel : MonoBehaviour {

    //GameObject ballObj;

    bool isWorking = false;

    Vector3 targetPos = Vector3.zero;

    public Vector3 hitPos
    { 
        get
        {
            return hitTopPos;
        }
    }

    Vector3 hitTopPos = Vector3.zero;
    Camera arCamera;

	// Use this for initialization
	void Start () {
        arCamera = this.transform.parent.GetComponent<Camera>();
        if(arCamera != null)
        {
            float far = arCamera.farClipPlane;
            this.transform.localEulerAngles = new Vector3(90, 0f, 0f);
            this.transform.localPosition = new Vector3(0, 0, far / 2.0f);
            this.transform.localScale = new Vector3(20, far, 10f);

        }
    }

    private void Update()
    {
        if (isWorking)
        {
            Ray ray = new Ray(this.targetPos, Vector3.up);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                // if (hit.collider.tag == "HitPanel")
                {
                    Vector3 pos = hit.point;
               //     ballObj.transform.position = pos;
                    hitTopPos = pos;
                   // if (amr != null)
                    {
                   //     amr.updateState(pos);
                    }
                }
            }
        }
    }

    public void setBottomPoint(Vector3 pos)
    {
        this.targetPos = pos;
    }

    public void StartWork()
    {
        isWorking = true;
    }

    public void StopWork()
    {
        isWorking = false;
        hitTopPos = Vector3.zero;
        //  ballObj.transform.position = Vector3.one*100000;
    }

}
