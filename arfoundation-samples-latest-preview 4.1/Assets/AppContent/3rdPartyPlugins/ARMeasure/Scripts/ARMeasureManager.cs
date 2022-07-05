using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARMeasure
{
    public class ARMeasureManager : MonoBehaviour
    {
        public MeasureMode mMeasureMode;

        public static ARMeasureManager instance;

        public measureLine m_measureController;
        
        // Use this for initialization
        void Start()
        {
            UnitConverter.mMeasureUnit = MeasureUnit.CM;

            if (m_measureController == null)
            {
                m_measureController = GameObject.FindObjectOfType<measureLine>();

                if (m_measureController == null)
                {
                    Debug.LogError(" MeasureLine Object is Null, Please check if have LinesController,HeightController or TriangleController in project ! ");
                }
            }
            instance = this;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void AddPoint(Vector3 pos)
        {
            if (m_measureController != null)
            {
                m_measureController.AddPoint(pos);
            }
        }

        public void updateState(Vector3 pos)
        {
            if (m_measureController != null)
            {
                m_measureController.updateLine(pos);
            }
        }

        public void deleteAllObjs()
        {
            if (m_measureController != null)
            {
                m_measureController.RemoveObjs();
            }
        }

        public void deleteLastObj()
        {

            if (m_measureController != null)
            {
                m_measureController.deleteLastObj();
            }

        }
    }

}
