using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

    public GameObject tipObj;

    // Use this for initialization
	void Start () {
        AnchorPoint anchorPoint = GameObject.FindObjectOfType<AnchorPoint>();
        if(anchorPoint)
        {
            anchorPoint.trackPlaneEvent += AnchorPoint_TrackPlaneEvent;
        }
	}
    void AnchorPoint_TrackPlaneEvent()
    {
        tipObj.SetActive(false);
    }

	// Update is called once per frame
	void Update () {
		
	}
}
