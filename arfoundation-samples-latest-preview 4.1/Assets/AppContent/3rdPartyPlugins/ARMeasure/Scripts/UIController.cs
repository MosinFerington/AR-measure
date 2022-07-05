using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARMeasure;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    public GameObject settingPanelObj;

    public Toggle rulerToggle;
    public Toggle heightToggle;
    public Toggle angleToggle;
    public Toggle polygonToggle;

    public GameObject selectImageObj;

    public GameObject cmBtnObj;
    public GameObject mBtnObj;
    public GameObject ftBtnObj;
    public GameObject inBtnObj;
    public GameObject ydBtnObj;
    public GameObject tipObj;

    TrianglesController triangleCtr;
    LinesController lineCtr;
    HeightController heightCtr;
    PolygonController polyCtr ;
    AnchorPoint anchortObj;

    private void Start()
    {
        
        triangleCtr = GameObject.FindObjectOfType<TrianglesController>();//find the angle controller
        lineCtr = GameObject.FindObjectOfType<LinesController>();//find the line controller
        heightCtr = GameObject.FindObjectOfType<HeightController>();//find the height controller
        polyCtr = GameObject.FindObjectOfType<PolygonController>();//find the polygon controller
        anchortObj = GameObject.FindObjectOfType<AnchorPoint>();

        rulerToggle.onValueChanged.AddListener((bool ischeckon) =>
        {
            selectMeasureMode(MeasureMode.MeasureLength, ischeckon);
        });

        heightToggle.onValueChanged.AddListener((bool ischeckon) =>
        {
            selectMeasureMode(MeasureMode.MeasureHeight, ischeckon);
        });

        angleToggle.onValueChanged.AddListener((bool ischeckon) =>
        {
            selectMeasureMode(MeasureMode.MeasureAngle, ischeckon);
        });

        polygonToggle.onValueChanged.AddListener((bool ischeckon) =>
        {
            selectMeasureMode(MeasureMode.MeasurePolygon, ischeckon);
        });
        

        cmBtnObj.GetComponent<Button>().onClick.AddListener(() =>
        {
            selectUnitMode(cmBtnObj, MeasureUnit.CM);
        });


        mBtnObj.GetComponent<Button>().onClick.AddListener(() =>
        {
            selectUnitMode(mBtnObj, MeasureUnit.M);
        });

        ftBtnObj.GetComponent<Button>().onClick.AddListener(() =>
        {
            selectUnitMode(ftBtnObj, MeasureUnit.FT);
        });

        inBtnObj.GetComponent<Button>().onClick.AddListener(() =>
        {
            selectUnitMode(inBtnObj, MeasureUnit.IN);
        });

        ydBtnObj.GetComponent<Button>().onClick.AddListener(() =>
        {
            selectUnitMode(ydBtnObj, MeasureUnit.YD);
        });

        anchortObj.trackPlaneEvent += trackPlaneStatus;

    }

    /// <summary>
    /// hide the tips
    /// </summary>
    void trackPlaneStatus()
    {
        hideMoveCameraTip();
    }

    /// <summary>
    /// open the measure mode panel 
    /// </summary>
    public void showPanel()
    {
        settingPanelObj.SetActive(true);
    }

    /// <summary>
    ///  close the measure mode panel 
    /// </summary>
    public void closePanel()
    {
        settingPanelObj.SetActive(false);
    }


    public void selectMeasureMode(MeasureMode mode, bool ischeckon)
    {
        if (ARMeasureManager.instance.mMeasureMode == mode)
        {
            return;
        }
        ARMeasureManager.instance.deleteAllObjs();
        if (ischeckon)
        {
            switch (mode)
            {
                case MeasureMode.MeasureLength:
                    {
                        ARMeasureManager.instance.mMeasureMode = MeasureMode.MeasureLength;
                        ARMeasureManager.instance.m_measureController = lineCtr;// assign the linecontroller to ARMeasureManager 
                    }
                    break;
                case MeasureMode.MeasureHeight:
                    {
                        ARMeasureManager.instance.mMeasureMode = MeasureMode.MeasureHeight;
                        ARMeasureManager.instance.m_measureController = heightCtr;// assign the heightcontroller to ARMeasureManager 
                    }
                    break;
                case MeasureMode.MeasureAngle:
                    {
                        ARMeasureManager.instance.mMeasureMode = MeasureMode.MeasureAngle;
                        ARMeasureManager.instance.m_measureController = triangleCtr;// assign the trianglecontroller to ARMeasureManager 
                    }
                    break;
                case MeasureMode.MeasurePolygon:
                    {
                        ARMeasureManager.instance.mMeasureMode = MeasureMode.MeasurePolygon;
                        ARMeasureManager.instance.m_measureController = polyCtr;// assign the trianglecontroller to ARMeasureManager 
                    }
                    break;
            }
        }

    }


    public void selectUnitMode(GameObject obj, MeasureUnit unitmode)
    {
        UnitConverter.mMeasureUnit = unitmode;
        selectImageObj.transform.position = obj.transform.position;

    }

    /// <summary>
    /// delete last measure object
    /// </summary>
    public void deleteLastObj()
    {
        ARMeasureManager.instance.deleteLastObj();
    }

    /// <summary>
    /// delete all measure objects
    /// </summary>
    public void deleteAllObjs()
    {
        ARMeasureManager.instance.deleteAllObjs();
    }


    public void showMoveCameraTip()
    {
        tipObj.SetActive(true);
    }


    public void hideMoveCameraTip()
    {
        if (anchortObj != null)
        {
            tipObj.SetActive(false);
        }

    }




}
