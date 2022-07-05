using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARMeasure;

public class HeightController : measureLine {
    
    //check the height by using this hitpanelobj. locat at ARCamera/HitPanel
    public HitPanel hitPanelObj;

    public GameObject linePerfab;
    public List<GameObject> sLinesList;

    GameObject mCurrentLineObj = null;

    public static HeightController instance;

    int pointCount = 0;

    bool isWorking = false;

    Vector3 originPos;
    // Use this for initialization
    void Start()
    {
        instance = this;

 //       hitPanelObj = GameObject.FindObjectOfType<HitPanel>();

        if(hitPanelObj != null)
        {
            Debug.LogError(" Can't find the HitPanel , please check if exist under the ARCamera Obj !");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isWorking)
        {
          
            updateLine(hitPanelObj.hitPos);
        }
    }

    override public void AddPoint(Vector3 pos)
    {
        if (pointCount % 2 == 0)
        {
            AddLine(pos);
            pointCount++; 
        }
        else
        {
            isWorking = false;
            LockCurrentLine();
            pointCount = 0;
        }
       
    }

    override public void AddLine(Vector3 pos)
    {
        GameObject go = Instantiate(linePerfab, pos, Quaternion.identity) as GameObject;
        go.transform.localPosition = pos;
        
        sLinesList.Add(go);

        go.GetComponent<PointLine>().setPoint(0, pos);//set the origin point of line
        go.GetComponent<PointLine>().setPoint(1, pos);//set the end point of line
        mCurrentLineObj = go;

        isWorking = true;
        originPos = pos;
        hitPanelObj.setBottomPoint(pos);
        hitPanelObj.StartWork();

        mCurrentLineObj.GetComponent<PointLine>().StartMove();//start moving line
    }

    override public void updateLine(Vector3 pos)
    {
        if (mCurrentLineObj != null)
        {
            if(pos != Vector3.zero )
            {
                mCurrentLineObj.GetComponent<PointLine>().setPoint(1, pos);//use the height poss
            }
            else
            {
                mCurrentLineObj.GetComponent<PointLine>().setPoint(1, originPos);//use the origin pos
            }
        }
    }

    override public void LockCurrentLine()
    {
        hitPanelObj.StopWork();
        mCurrentLineObj.GetComponent<PointLine>().StopMove();
        mCurrentLineObj = null;
        pointCount = 0;
    }

    override public void RemoveObjs()
    {
        foreach (GameObject go in sLinesList)
        {
            Destroy(go);
        }
        hitPanelObj.StopWork();
        sLinesList.Clear();
        pointCount = 0;
    }

    public override void deleteLastObj()
    {
        if (sLinesList.Count > 0)
        {
            Destroy(sLinesList[sLinesList.Count - 1]);
        }
        else
        {
            return;
        }

        hitPanelObj.StopWork();
        sLinesList.RemoveAt(sLinesList.Count - 1);
        pointCount = 0;
        mCurrentLineObj = null;
    }

}
