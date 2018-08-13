﻿using Autodesk.Revit.DB;
using BH.Engine.Revit;
using BH.Engine.Structure;
using BH.oM.Revit;
using BH.oM.Structural.Elements;
using System.Collections.Generic;
using System.Linq;
namespace BH.UI.Cobra.Engine
{
    public static partial class Convert
    {
        /***************************************************/
        /****             Internal methods              ****/
        /***************************************************/

        internal static Floor ToRevitFloor(this PanelPlanar panelPlanar, Document document, PushSettings pushSettings = null)
        {
            //TODO: if no CustomData, set the level & offset manually?

            if (panelPlanar == null || document == null)
                return null;

            pushSettings.DefaultIfNull();

            object aCustomDataValue = null;

            CurveArray aCurves = new CurveArray();
            foreach (Curve c in panelPlanar.AllEdgeCurves().Select(c => c.ToRevit(pushSettings)))
            {
                aCurves.Append(c);
            }

            Level aLevel = null;

            aCustomDataValue = panelPlanar.ICustomData("Level");
            if (aCustomDataValue != null && aCustomDataValue is int)
            {
                ElementId aElementId = new ElementId((int)aCustomDataValue);
                aLevel = document.GetElement(aElementId) as Level;
            }

            if (aLevel == null)
                aLevel = Query.BottomLevel(panelPlanar.Outline(), document);

            FloorType aFloorType = panelPlanar.Property.ToRevitFloorType(document, pushSettings);

            if (aFloorType == null)
            {
                List<FloorType> aFloorTypeList = new FilteredElementCollector(document).OfClass(typeof(FloorType)).OfCategory(BuiltInCategory.OST_Floors).Cast<FloorType>().ToList();

                aCustomDataValue = panelPlanar.ICustomData("Type");
                if (aCustomDataValue != null && aCustomDataValue is int)
                {
                    ElementId aElementId = new ElementId((int)aCustomDataValue);
                    aFloorType = aFloorTypeList.Find(x => x.Id == aElementId);
                }

                if (aFloorType == null)
                    aFloorTypeList.Find(x => x.Name == panelPlanar.Name);

                if (aFloorType == null)
                {
                    BH.Engine.Reflection.Compute.RecordError(string.Format("Floor type has not been found for given BHoM panel property. BHoM Guid: {0}", panelPlanar.BHoM_Guid));
                    return null;
                }
            }

            Floor aFloor = document.Create.NewFloor(aCurves, aFloorType, aLevel, false);

            aFloor.CheckIfNullPush(panelPlanar);

            if (aFloor != null)
            {
                if (pushSettings.CopyCustomData)
                {
                    BuiltInParameter[] paramsToIgnore = new BuiltInParameter[]
                    {
                        BuiltInParameter.ELEM_FAMILY_PARAM,
                        BuiltInParameter.ELEM_FAMILY_AND_TYPE_PARAM,
                        BuiltInParameter.ALL_MODEL_IMAGE,
                        BuiltInParameter.ELEM_TYPE_PARAM
                    };
                    Modify.SetParameters(aFloor, panelPlanar, paramsToIgnore, pushSettings.ConvertUnits);
                }
            }

            return aFloor;
        }
    }
}