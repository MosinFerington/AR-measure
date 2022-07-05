using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARMeasure;

public class TrianglesController : measureLine
{

    public GameObject trianglePerfab;

    public List<GameObject> sTriangleList;

    GameObject mCurrentLineObj = null;

    public static TrianglesController instance;

    int pointCount = 0;

    Vector3 orginVec;
    // Use this for initialization
    void Start()
    {
        // yield return new WaitForSeconds(0.05f);
        instance = this;

        if (LinesController.instance != null)
        {
            //Debug.LogError("Please check if the LinesController exsit in project ! ");
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Adds the point.
    /// </summary>
    /// <param name="pos">Position.</param>
    public bool Auto4thPointSet = false;
    override public void AddPoint(Vector3 pos)
    {
        // Automatically Set 4th corner
        if (Auto4thPointSet) 
        {
            if (pointCount % 3 == 0) // 0 pointCount
            {
                Debug.Log("Add point in Triangle is " + pos);
                LinesController.instance.AddPoint(pos);
                AddTriangle(pos);
                orginVec = pos;
                pointCount++;
            }
            else if (pointCount % 3 == 1)
            {
                LinesController.instance.AddPoint(pos);

                LinesController.instance.AddPoint(orginVec);

                mCurrentLineObj.GetComponent<PointTriangle>().StartMove();

                mCurrentLineObj.GetComponent<PointTriangle>().setPoint(2, pos); // was 1
                pointCount++;
            }

            else if (pointCount % 3 == 2)
            {
                mCurrentLineObj.GetComponent<PointTriangle>().setPoint(2, pos);
                LinesController.instance.AddPoint(pos);
                LockCurrentLine();
            }
        } 
        else // Manuall Set 4rt corner
        {
            if (pointCount % 4 == 0) 
            {
                Debug.Log("Add point in Triangle is " + pos);
                LinesController.instance.AddPoint(pos);
                AddTriangle(pos);
                orginVec = pos;
                pointCount++;
            }
            else if (pointCount % 4 == 1)
            {
                LinesController.instance.AddPoint(pos);

                LinesController.instance.AddPoint(orginVec);

                mCurrentLineObj.GetComponent<PointTriangle>().StartMove();

                mCurrentLineObj.GetComponent<PointTriangle>().setPoint(2, pos); // was 1
                pointCount++;
            }
            else if (pointCount % 4 == 2)
            {
                LinesController.instance.AddPoint(pos);

                LinesController.instance.AddPoint(orginVec);

                mCurrentLineObj.GetComponent<PointTriangle>().StartMove();

                mCurrentLineObj.GetComponent<PointTriangle>().setPoint(3, pos);
                pointCount++;
            }

            else if (pointCount % 4 == 3)
            {
                mCurrentLineObj.GetComponent<PointTriangle>().setPoint(3, pos);
                LinesController.instance.AddPoint(pos);
                LockCurrentLine();
            }
        }


    }


    /// <summary>
    /// Add a triangle to show the angle zone
    /// </summary>
    /// <param name="pos">Position.</param>
    public void AddTriangle(Vector3 pos)
    {
        GameObject go = Instantiate(trianglePerfab) as GameObject;

        sTriangleList.Add(go);

        go.GetComponent<PointTriangle>().setPoint(0, pos);
        mCurrentLineObj = go;
    }

    /// <summary>
    /// Updates the triangle.
    /// </summary>
    /// <param name="pos">Position.</param>
    override public void updateLine(Vector3 pos)
    {
        if (mCurrentLineObj != null)
        {
            if (pointCount % 3 == 1)
            {
                mCurrentLineObj.GetComponent<PointTriangle>().setPoint(1, pos);
            }
            else if (pointCount % 3 == 2)
            {
                mCurrentLineObj.GetComponent<PointTriangle>().setPoint(2, pos);
            }
            LinesController.instance.updateLine(pos);
        }
    }

    override public void LockCurrentLine()
    {
        if (mCurrentLineObj != null)
        {
            mCurrentLineObj.GetComponent<PointTriangle>().StopMove();
            mCurrentLineObj = null;
        }

        pointCount = 0;
    }

    override public void RemoveObjs()
    {
        foreach (GameObject go in sTriangleList)
        {
            Destroy(go);
        }
        sTriangleList.Clear();
        pointCount = 0;
        LinesController.instance.RemoveObjs();
    }

    public override void deleteLastObj()
    {
        if (sTriangleList.Count > 0)
        {
            Destroy(sTriangleList[sTriangleList.Count - 1]);
        }
        else
        {
            return;
        }

        sTriangleList.RemoveAt(sTriangleList.Count - 1);

        if (pointCount % 3 == 1)
        {
            LinesController.instance.deleteLastObj();
        }
        else if (pointCount % 3 == 2 || pointCount % 3 == 0)
        {
            LinesController.instance.deleteLastObj();
            LinesController.instance.deleteLastObj();
        }
        pointCount = 0;
    }


}
