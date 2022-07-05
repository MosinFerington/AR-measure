using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARMeasure;
using UnityEngine.UI;

public class PolygonController : measureLine
{
    public GameObject linePerfab;
    public GameObject polygonPerfab;

    public List<GameObject> sLinesList;
    public List<GameObject> sPolygonList;

    GameObject mCurrentLineObj = null;
    [HideInInspector]
    public GameObject mCurrentPolygonObj = null;

    public static PolygonController instance;

    int pointCount = 0;

    Vector3 firstPos;
    float distance = 0;

    // Use this for initialization
    void Awake()
    {
        instance = this;
    }

   
    /// <summary>
    /// Adds the point.
    /// </summary>
    /// <param name="pos">Position.</param>
    public int CornerAmountLimitation = 4;
    override public void AddPoint(Vector3 pos)
    {
        Debug.Log("Polygon Point Count: " + pointCount + " Initial Position: " + firstPos);

        if (pointCount == 0)
        {
            Debug.Log(pointCount + ":  " + pos);
            firstPos = pos;
            AddLine(pos);
            addPolygon(pos);
            addPolygonPoint(pos);
            SetRoomName("");
        }
        else
        {
            Debug.Log(pointCount + ":  " + pos);
            LockCurrentLine();//lock current line
            AddLine(pos);//create a new line
            addPolygonPoint(pos);

            if (pointCount == 3)//the polygon close //distance < 0.05f
            {
                mCurrentPolygonObj.GetComponent<PointPolygon>().lockPolygon();
                pointCount = 0;
                StartCoroutine(waitForClosePolygon());

                //CalculateTextPos();

                return;
            }
            // }
        }
        pointCount++;
    }

    /*
    public void CalculateTextPos()
    {
        RoomTitleText.transform.parent.gameObject.SetActive(true);
        RoomTitleText.enabled = true;
        float ax = 0, ay = 0, az = 0;
        for (int i = 0; i != sLinesList.Count; i++)
        {
            ax += sLinesList[i].GetComponent<PointLine>().mPoints[0].x;
            ay += sLinesList[i].GetComponent<PointLine>().mPoints[0].y;
            az += sLinesList[i].GetComponent<PointLine>().mPoints[0].z;
        }
        int num = sLinesList.Count;
        RoomTitleTextContainer.transform.position = new Vector3(ax / num, ay / num, az / num);
    }
*/

    IEnumerator waitForClosePolygon()
    {
        yield return new WaitForEndOfFrame();
        mCurrentLineObj.GetComponent<PointLine>().setPoint(1, firstPos);
        yield return new WaitForEndOfFrame();
        LockCurrentLine();//lock current line，close the polygon zone
    }

    /// <summary>
    /// Add a line start with the pos .
    /// </summary>
    /// <param name="pos">Position.</param>
    override public void AddLine(Vector3 pos)
    {
        GameObject go = Instantiate(linePerfab, pos, Quaternion.identity) as GameObject;
        sLinesList.Add(go);

        go.GetComponent<PointLine>().setPoint(0, pos);
        go.GetComponent<PointLine>().setPoint(1, pos);
        mCurrentLineObj = go;
        if (pointCount > 0)
        {
            mCurrentLineObj.GetComponent<PointLine>().StartMove(false);
        }
        else
        {
            mCurrentLineObj.GetComponent<PointLine>().StartMove(true);
        }
    }

    void addPolygon(Vector3 pos)
    {
        GameObject go = Instantiate(polygonPerfab, Vector3.zero, Quaternion.identity) as GameObject;
        sPolygonList.Add(go);
        mCurrentPolygonObj = go;
    }

    public void SetRoomName(string RoomName)
    {
        if (mCurrentPolygonObj)
        {
            mCurrentPolygonObj.GetComponent<PointPolygon>().areaText.text = RoomName;
        }
    }

    void addPolygonPoint(Vector3 pos)
    {
        if (mCurrentPolygonObj)
        {
            mCurrentPolygonObj.GetComponent<PointPolygon>().addPoint(pos);
        }
    }

    /// <summary>
    /// Updates the length of the line.
    /// </summary>
    /// <param name="pos">Position.</param>
    override public void updateLine(Vector3 pos)
    {
        if (mCurrentLineObj != null && pointCount > 0)
        {
            Debug.Log("-" + pointCount + ":  " + pos);

            mCurrentLineObj.GetComponent<PointLine>().setPoint(1, pos);
        }
    }

    /// <summary>
    /// Locks the current line.
    /// </summary>
    override public void LockCurrentLine()
    {
        if (mCurrentLineObj != null)
        {
            mCurrentLineObj.GetComponent<PointLine>().StopMove();
            mCurrentLineObj = null;
        }
        Debug.Log("current point is count is " + pointCount);
    }

    /// <summary>
    /// Removes the objects.
    /// </summary>
    override public void RemoveObjs()
    {
        foreach (GameObject go in sLinesList)
        {
            Destroy(go);
        }
        sLinesList.Clear();

        foreach (GameObject go in sPolygonList)
        {
            Destroy(go);
        }
        sPolygonList.Clear();
        pointCount = 0;
        mCurrentLineObj = null;
    }


}
