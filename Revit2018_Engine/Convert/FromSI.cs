﻿using System;
using System.Collections.Generic;
using System.Linq;
using BH.oM.Structural.Elements;

using BH.oM.Environmental.Elements;
using BH.oM.Environmental.Properties;
using BH.oM.Environmental.Interface;
using BH.oM.Geometry;

using BH.Engine.Environment;

using Autodesk.Revit.DB;

namespace BH.Engine.Revit
{

    /// <summary>
    /// BHoM Revit Engine Convert Methods
    /// </summary>
    public static partial class Convert
    {
        public static double FromSI(this double Value, UnitType UnitType)
        {
            switch(UnitType)
            {
                case UnitType.UT_Length:
                    return UnitUtils.ConvertToInternalUnits(Value, DisplayUnitType.DUT_METERS);
                case UnitType.UT_Mass:
                    return UnitUtils.ConvertToInternalUnits(Value, DisplayUnitType.DUT_KILOGRAMS_MASS);
                case UnitType.UT_Electrical_Current:
                    return UnitUtils.ConvertToInternalUnits(Value, DisplayUnitType.DUT_AMPERES);
                case UnitType.UT_HVAC_Temperature:
                    return UnitUtils.ConvertToInternalUnits(Value, DisplayUnitType.DUT_KELVIN);
                case UnitType.UT_Weight:
                    return UnitUtils.ConvertToInternalUnits(Value, DisplayUnitType.DUT_KILOGRAMS_MASS);
                case UnitType.UT_HVAC_Pressure:
                    return UnitUtils.ConvertToInternalUnits(Value, DisplayUnitType.DUT_PASCALS);
                case UnitType.UT_Piping_Pressure:
                    return UnitUtils.ConvertToInternalUnits(Value, DisplayUnitType.DUT_PASCALS);
                case UnitType.UT_HVAC_Velocity:
                    return UnitUtils.ConvertToInternalUnits(Value, DisplayUnitType.DUT_METERS_PER_SECOND);
                case UnitType.UT_Area:
                    return UnitUtils.ConvertToInternalUnits(Value, DisplayUnitType.DUT_SQUARE_METERS);
                case UnitType.UT_Volume:
                    return UnitUtils.ConvertToInternalUnits(Value, DisplayUnitType.DUT_CUBIC_METERS);
                default:
                    return Value;
            }
        }
    }
}