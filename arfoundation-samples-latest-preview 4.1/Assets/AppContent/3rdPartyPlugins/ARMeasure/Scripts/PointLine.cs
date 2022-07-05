using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ARMeasure;

public class PointLine : MonoBehaviour {

    public Vector3[] mPoints = new Vector3[2];

    public float lineWidth = 0.1f;
    private LineRenderer lineObj;
    public bool isMoveing = false;
    
    public Text distanceText;
    public Transform cavansTextObj;

    public GameObject pointPrefab;
    public float Length
    {
        get { return m_Length; }
        set { m_Length = value; }
    }

    float m_Length = 0.0f;

    // Use this for initialization
    void Start () {

        lineObj = GetComponent<LineRenderer>();
        lineObj.startWidth = lineWidth;
        lineObj.endWidth = lineWidth;
        lineObj.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
		if(isMoveing)
        {
            lineObj.SetPosition(0, mPoints[0]);
            lineObj.SetPosition(1, mPoints[1]);
            updateTextMesh();
            lineObj.enabled = true;
        }
    }
    
    public void StartMove(bool isCreatePoint = true)
    {
        updateTextMesh();
        isMoveing = true;
        
        if(isCreatePoint)
        {
            GameObject tagSphere = Instantiate(pointPrefab, mPoints[0], Quaternion.identity) as GameObject;
            tagSphere.name = "StartPoint";
            tagSphere.transform.parent = this.transform;
        }
    }

    public void StopMove(bool isCreatePoint = true)
    {
        Vector3 tarVec = (mPoints[0] + mPoints[1]) / 2.0f;
        cavansTextObj.transform.position = tarVec;
        isMoveing = false;

      //  if(isCreatePoint)
        {
            GameObject tagSphere = Instantiate(pointPrefab, mPoints[1], Quaternion.identity) as GameObject;
            tagSphere.name = "EndPoint";
            tagSphere.transform.parent = this.transform;
        }
    }

    void updateTextMesh()
    {
        Vector3 tarVec = mPoints[1];
        float distInInches = Vector3.Distance(mPoints[0], mPoints[1]) ;
        distInInches = UnitConverter.convertToTargetUnit(distInInches);
        
        m_Length = distInInches;

        string s = System.String.Format("{0:0.00}", distInInches);

        distanceText.text = s + UnitConverter.unitString();

        Vector3 dir = mPoints[1] - mPoints[0];
        cavansTextObj.transform.rotation = Quaternion.LookRotation(dir, this.transform.up);

        cavansTextObj.transform.position = tarVec + dir.normalized*0.1f;
    }

    public void setPoint(int i, Vector3 pos)
    {
        mPoints[i] = pos;
    }

    public void createStaticLine(Vector3 pos1,Vector3 pos2)
    {
        lineObj.SetPosition(0, mPoints[0]);
        lineObj.SetPosition(1, mPoints[1]);
        updateTextMesh();

        Vector3 tarVec = (mPoints[0] + mPoints[1]) / 2.0f;
        cavansTextObj.transform.position = tarVec;
    }
}
