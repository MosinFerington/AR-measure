using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using UnityEngine;

using GrapeCity.Documents.Imaging;
using GrapeCity.Documents.Svg;

using SimpleFileBrowser;

public class Actions : MonoBehaviour
{
    // This is the prefab that is selected by the user.
    public GameObject selectedPrefab;

    public GameObject pointPrefab;
    public GameObject linePrefab;

    private List<GameObject> points;
    
    private List<GameObject> lines;

    private List<GameObject> texts;

    private List<GameObject> historyGo;

    public GameObject[] selectors;

    public GameObject SVGViewer;

    void Start()
    {
        points = new List<GameObject>();
        texts = new List<GameObject>();
        historyGo = new List<GameObject>();
        lines = new List<GameObject>();
        selectedPrefab = pointPrefab;

        SelectPoint();
    }

    public void PerfromTouch(Vector2 vpress,Vector3 pos, Vector3 normal, GameObject hitObject)
    {
        //instanciate gameobject POINT in space
        if (selectedPrefab == pointPrefab)
        {
            if (points.Count > 0)
            {
                var firstPoint = points[0];

                Plane plane = new Plane(firstPoint.transform.up, firstPoint.transform.position);
                Ray ray = Camera.main.ScreenPointToRay(vpress);

                plane.Raycast(ray, out float enter);

                pos = ray.GetPoint(enter);
            }

            if (hitObject != null)
            {
                pos = hitObject.transform.position;
            }

            points.Add(GameObject.Instantiate(selectedPrefab, pos, Quaternion.Euler(90, 0, 0)));
            points[^1].transform.up = normal;

            historyGo.Add(points[points.Count - 1]);

            //DebugConsole.DC.Log("Point: x=" + vpress.x + " y=" + vpress.y);

            if (points.Count > 1)
            {
                lines.Add(GameObject.Instantiate(linePrefab, pos, Quaternion.Euler(90, 0, 0)));
                lines[^1].GetComponent<Line>().SetParamValues(points[points.Count - 1].transform, points[points.Count - 2].transform);

                lines[^1].transform.SetParent(points[^1].transform);
            }
        }
    }

    public void UndoDraw()
    {
        if (historyGo.Count > 0)
        {
            GameObject.Destroy(historyGo[^1]);
            historyGo.RemoveAt(historyGo.Count - 1);
            
        }
        if (points.Count > 0)
        {
            points.RemoveAt(points.Count - 1);

        }
        if (lines.Count > 1)
        {
            lines.RemoveAt(lines.Count - 1);
        }
    }

    /// <summary>
    ///  SELECTION ACTIONS
    /// </summary>
    public void SelectPoint()
    {
        //we select the point prefab gameobject
        selectedPrefab = pointPrefab;
        ChangeSelector(1);
    }

    public void SelectPointer()
    { 
        //we deselect any prefab
        selectedPrefab = null;
        ChangeSelector(0);
    }

    public void ChangeSelector(int a)
    {
        for(int ii=0;ii<selectors.Length;ii++)
        {
            selectors[ii].SetActive(false);
        }

        selectors[a].SetActive(true);

    }

    [ContextMenu("Save SVG")]
    public void SaveSVG()
    {
        SelectPointer();

        if (points.Count == 0)
        {
            return;
        }

        //SVGViewer.SetActive(true);

        //float width = SVGViewer.GetComponent<RectTransform>().rect.width;
        //float height = SVGViewer.GetComponent<RectTransform>().rect.height;

        var units = SvgLengthUnits.Centimeters;

        var firstPoint = points[0].transform;
        var planeMatrix = firstPoint.localToWorldMatrix;

        var pointsPositions = new List<Vector3>();

        foreach (var p in points)
        {
            var pointMatrix = p.transform.localToWorldMatrix;
            var inverse = planeMatrix.inverse * pointMatrix;
            inverse.SetTRS(inverse.ExtractPosition(), inverse.ExtractRotation(), pointMatrix.ExtractScale());

            pointsPositions.Add(inverse.ExtractPosition());
            //p.transform.FromMatrix(inverse);
        }

        float minx = pointsPositions.Min(el => el.x);
        float minz = pointsPositions.Min(el => el.z);

        float maxx = pointsPositions.Max(el => el.x);
        float maxz = pointsPositions.Max(el => el.z);

        //---Create SVG---//
        var svgDoc = new GcSvgDocument();
        svgDoc.RootSvg.Width = new SvgLength(maxx - minx, units);
        svgDoc.RootSvg.Height = new SvgLength(maxz - minz, units);

        for (int ind = 0; ind < pointsPositions.Count; ind++)
        {
            var p = pointsPositions[ind];

            p.x -= minx;
            p.z -= minz;

            pointsPositions[ind] = p;
        }

        for (int ind = 0; ind < pointsPositions.Count - 1; ind++)
        {
            var line = new SvgLineElement()
            {
                X1 = new SvgLength(pointsPositions[ind].x, units),
                Y1 = new SvgLength(pointsPositions[ind].z, units),
                X2 = new SvgLength(pointsPositions[ind + 1].x, units),
                Y2 = new SvgLength(pointsPositions[ind + 1].z, units),
                Stroke = new SvgPaint(System.Drawing.Color.FromArgb(81, 59, 116))
            };

            svgDoc.RootSvg.Children.Add(line);
        }

        SvgViewBox view = new SvgViewBox
        {
            MinX = 0,
            MinY = 0,
            Width = (maxx - minx) * 37.7952755906f,
            Height = (maxz - minz) * 37.7952755906f
        };

        svgDoc.RootSvg.ViewBox = view;

        FileBrowser.SetFilters(false, new FileBrowser.Filter("SVG", ".svg"));
        FileBrowser.SetDefaultFilter(".svg");
        StartCoroutine(ShowSaveDialogCoroutine(svgDoc));

        //---Renderer SVG---//
        //using var bmp = new GcBitmap((int)width, (int)height, true);

        //using (var g = bmp.CreateGraphics(System.Drawing.Color.White))
        //{
        //    g.DrawSvg(svgDoc, new System.Drawing.RectangleF(0, 0, bmp.PixelWidth, bmp.PixelHeight));
        //}

        //var texture = new Texture2D((int)width, (int)height, TextureFormat.ARGB32, false);
        //texture.LoadRawTextureData(bmp.RawData, bmp.PixelWidth * bmp.PixelHeight * sizeof(uint));
        //texture.Apply();

        ////bmp.SaveAsPng(@"F:\test.png");

        ////---Set Texture---//
        //var rawImage = SVGViewer.GetComponent<UnityEngine.UI.RawImage>();
        //rawImage.texture = texture;
    }

    IEnumerator ShowSaveDialogCoroutine(GcSvgDocument svgDoc)
    {
        // Show a load file dialog and wait for a response from user
        // Load file/folder: both, Allow multiple selection: true
        // Initial path: default (Documents), Initial filename: empty
        // Title: "Load File", Submit button text: "Load"
        yield return FileBrowser.WaitForSaveDialog(FileBrowser.PickMode.Files, false, null, null, "Save SVG", "Save");

        // Dialog is closed
        // Print whether the user has selected some files/folders or cancelled the operation (FileBrowser.Success)
        Debug.Log(FileBrowser.Success);

        if (!FileBrowser.Success)
        {
            yield break;
        }

        StringBuilder stringBuilder = new StringBuilder(); 
        svgDoc.Save(stringBuilder);

        FileBrowserHelpers.WriteTextToFile(FileBrowser.Result[0], stringBuilder.ToString());
    }
}
