using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class PointTriangle : MonoBehaviour {

    public Vector3[] mPoints = new Vector3[3];

    public bool isMoveing = false;

    public float Angle
    {
        get { return m_Angle; }
        set { m_Angle = value; }
    }

    public float m_Angle = 0;
    
    public TextMesh textMesh;
    public Transform textObj;

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        if (isMoveing)
        {
            //DrawTriangle();
            calcAngle();
            updateTextMesh();
        }
	}

    /// <summary>
    /// Starts the move.
    /// </summary>
    public void StartMove()
    {
        textObj.gameObject.SetActive(true);
        isMoveing = true;
    }

    /// <summary>
    /// Sets the point.
    /// </summary>
    /// <param name="i">The index.</param>
    /// <param name="pos">Position.</param>
    public void setPoint(int i, Vector3 pos)
    {
        mPoints[i] = pos;
    }

    public void StopMove()
    {
        isMoveing = false;
    }

    /// <summary>
    /// Draws the triangle.
    /// </summary>
    void DrawTriangle()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();

        mesh.vertices = mPoints;

        mesh.triangles = new int[] { 0, 1, 2 };

        mesh.uv = new Vector2[] {
            new Vector2(0, 0),
            new Vector2(0, 1),
            new Vector2(1, 1)
        };
    }
    
    float VectorAngle(Vector2 from, Vector2 to)
    {
        Vector3 cross = Vector3.Cross(from, to);
        m_Angle = Vector2.Angle(from, to);
        return cross.z > 0 ? -m_Angle : m_Angle;
    }
    
    void calcAngle()
    {
        Vector3 pos1 = mPoints[0];

        Vector3 pos2 = mPoints[1]; ;
        //    pos2.Set(pos2.x, pos2.z, 0);

        Vector3 pos3 = mPoints[2];
        //  pos2.Set(pos3.x, pos3.z, 0);

        //Vector3 line1 = pos2 - pos1;  // Initial - EA
        // Vector3 line2 = pos3 - pos1;  // Initial - EA

        Vector3 line1 = pos1 - pos2;
        Vector3 line2 = pos3 - pos2;

        m_Angle = Mathf.Abs( VectorAngle(new Vector2(line1.x, line1.z), new Vector2(line2.x, line2.z)));
    }

    void updateTextMesh()
    {
         Vector3 tarVec = (mPoints[0] + mPoints[1] + mPoints[2]) / 3.0f; // Initial - EA
        
        textObj.transform.LookAt((mPoints[1] + mPoints[2]) /2.0f) ;

       // textObj.transform.position = mPoints[0];
        textObj.transform.position = mPoints[1];
        string s = System.String.Format("{0:0.0}", m_Angle);
        textMesh.text = s + "°";
    }

}
