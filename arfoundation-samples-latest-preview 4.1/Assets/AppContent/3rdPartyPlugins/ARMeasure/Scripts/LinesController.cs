using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARMeasure;

public class LinesController : measureLine
{

    public GameObject linePerfab;
    public List<GameObject> sLinesList;

    GameObject mCurrentLineObj = null;

    public static LinesController instance;

    int pointCount = 0;
    // Use this for initialization
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Adds the point.
    /// </summary>
    /// <param name="pos">Position.</param>
    override public void AddPoint(Vector3 pos)
    {
        if (pointCount % 2 == 0)
        {
            AddLine(pos);
            pointCount++;
        }
        else
        {
            LockCurrentLine();
        }

    }

    /// <summary>
    /// Add a line start with the pos .
    /// </summary>
    /// <param name="pos">Position.</param>
    override public void AddLine(Vector3 pos)
    {
        GameObject go = Instantiate(linePerfab, pos, Quaternion.identity) as GameObject;
        sLinesList.Add(go);

        if (sLinesList.Count == 1)                                                                          // ADDED NEW LINE EdgarasArt
            go.GetComponent<PointLine>().setPoint(0, pos);
        if (sLinesList.Count == 2)                                                                          // ADDED NEW LINE EdgarasArt
            go.GetComponent<PointLine>().setPoint(0, sLinesList[0].GetComponent<PointLine>().mPoints[1]);   // ADDED NEW LINE EdgarasArt
        mCurrentLineObj = go;


        mCurrentLineObj.GetComponent<PointLine>().StartMove();
    }

    /// <summary>
    /// Updates the length of the line.
    /// </summary>
    /// <param name="pos">Position.</param>
    override public void updateLine(Vector3 pos)
    {
        if (mCurrentLineObj != null)
        {
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
        pointCount = 0;
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
        pointCount = 0;
    }


    /// <summary>
    /// Deletes the last object.
    /// </summary>
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
        sLinesList.RemoveAt(sLinesList.Count - 1);

        pointCount = 0;
    }

}
