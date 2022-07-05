using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class PointPolygon : MonoBehaviour {
    
    public bool isUpdating = false;
    public List<Vector3> sPointList;

    public float Area
    {
        get { return m_Area; }
        set { m_Area = value; }
    }

    public float m_Area = 0;
    
    public Text areaText;
    public Transform textObj;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        if (isUpdating)
        {
            //updateMesh();
        }
	}
    
    /// <summary>
    /// Sets the point.
    /// </summary>
    /// <param name="i">The index.</param>
    /// <param name="pos">Position.</param>
    public void addPoint( Vector3 pos)
    {
        sPointList.Add(pos);
        if(sPointList.Count>3) // was 2
        {
            isUpdating = true;
            CalculateTextPos();
        }
    }

    public void lockPolygon()
    {
        isUpdating = false;
    }
    

    private void updateMesh()
    {
        List<Vector3> lists = sPointList;
        int iVertexCount = lists.Count;
        int iTrsCount = lists.Count - 2;
        //int iTimer = 0;

        MeshFilter filter = this.gameObject.GetComponent<MeshFilter>();
        MeshRenderer renderer = this.gameObject.GetComponent<MeshRenderer>();

        int[] newTriangles = new int[3 * iTrsCount];
        for (int i = 0; i < iTrsCount; i++)
        {
            newTriangles[3 * i] = 0;//固定第一个点    
            newTriangles[3 * i + 1] = i + 1;
            newTriangles[3 * i + 2] = i + 2;
        }

        Vector3[] newVertices = new Vector3[iVertexCount];
        for (int i = 0; i < iVertexCount; i++)
        {
            var pos = lists[i];
            newVertices[i] = pos;
        }

        var mesh = filter.mesh;
        mesh.vertices = newVertices;
        mesh.triangles = newTriangles;
        CalculateArea();
    }

    public void CalculateArea()
    {
        MeshFilter filter = this.gameObject.GetComponent<MeshFilter>();

        var mesh = filter.mesh;
        float num = 0f;
        for (int i = 0; i < mesh.triangles.Length / 3; i++)
        {
            Vector3 a = mesh.vertices[mesh.triangles[i * 3]];
            Vector3 b = mesh.vertices[mesh.triangles[i * 3 + 1]];
            Vector3 a2 = mesh.vertices[mesh.triangles[i * 3 + 2]];
            
            num += 0.5f * Vector3.Cross(a - b, a2 - b).magnitude;
        }
        string area =  System.String.Format("{0:0.000}", num);
        areaText.text = area + "m²";
    }

    public void CalculateTextPos()
    {
        //areaText.enabled = false;

        float ax = 0, ay = 0, az = 0;
        for(int i = 0;i!= sPointList.Count; i++)
        {
            ax += sPointList[i].x;
            ay += sPointList[i].y;
            az += sPointList[i].z;
        }
        int num = sPointList.Count;
        textObj.transform.position = new Vector3(ax / num, ay / num, az / num);
    }
}
