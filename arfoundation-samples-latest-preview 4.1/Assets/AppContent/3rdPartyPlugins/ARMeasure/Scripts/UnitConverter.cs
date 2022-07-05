using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARMeasure
{
    public class UnitConverter
    {
        public static MeasureUnit mMeasureUnit;

        public static float toIn = 39.3701f;
        public static float toCm = 100f;
        public static float toFt = 3.2808399f;
        public static float toYd = 1.0936133f;

        public static float convertToTargetUnit(float dis)
        {
            float unitLength = dis;

            switch(mMeasureUnit)
            {
                case MeasureUnit.CM:
                    unitLength =  unitLength* toCm;
                    break;
                case MeasureUnit.M:
                    break;
                case MeasureUnit.FT:
                    unitLength = unitLength * toFt;
                    break;
                case MeasureUnit.IN:
                    unitLength = unitLength * toIn;
                    break;
                case MeasureUnit.YD:
                    unitLength = unitLength * toYd;
                    break;
            }
            return unitLength;
        }

        public static string unitString()
        {
            string unitstr = "cm";

            switch (mMeasureUnit)
            {
                case MeasureUnit.CM:
                    unitstr = "cm";
                    break;
                case MeasureUnit.M:
                    unitstr = "m";
                    break;
                case MeasureUnit.FT:
                    unitstr = "ft";
                    break;
                case MeasureUnit.IN:
                    unitstr = "in";
                    break;
                case MeasureUnit.YD:
                    unitstr = "yd";
                    break;
            }
            return unitstr;
        }
    }
}

