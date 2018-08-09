﻿using System.Collections.Generic;

using Autodesk.Revit.DB;
using BH.oM.Base;
using BH.oM.Environment.Elements;
using BH.Engine.Environment;
using BH.oM.Adapters.Revit;

namespace BH.Engine.Revit
{
    /// <summary>
    /// BHoM Revit Engine Query Methods
    /// </summary>
    public static partial class Query
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        /// <summary>
        /// Gets Level of Revit element
        /// </summary>
        /// <param name="hostObject">Revit Host Object</param>
        /// <param name="face">Revit Face</param>
        /// <param name="pullSettings">BHoM PullSettings</param>
        /// <returns name="BuildingElements">BHoM BuildingElements</returns>
        /// <search>
        /// Query, HostedBuildingElements, Revit, Hosted Building Elements, BuildingElements, Element, Face
        /// </search>
        static public List<BuildingElement> HostedBuildingElements(HostObject hostObject, Face face, PullSettings pullSettings = null)
        {
            if (hostObject == null)
                return null;

            IList<ElementId> aElementIdList = hostObject.FindInserts(false, false, false, false);
            if (aElementIdList == null || aElementIdList.Count < 1)
                return null;

            if (pullSettings == null)
                pullSettings = PullSettings.Default;

            List<BuildingElement> aBuildingElmementList = new List<BuildingElement>();
            foreach (ElementId aElementId in aElementIdList)
            {
                Element aElement_Hosted = hostObject.Document.GetElement(aElementId);
                if ((BuiltInCategory)aElement_Hosted.Category.Id.IntegerValue == BuiltInCategory.OST_Windows || (BuiltInCategory)aElement_Hosted.Category.Id.IntegerValue == BuiltInCategory.OST_Doors)
                {
                    IntersectionResult aIntersectionResult = null;

                    BoundingBoxXYZ aBoundingBoxXYZ = aElement_Hosted.get_BoundingBox(null);

                    aIntersectionResult = face.Project(aBoundingBoxXYZ.Max);
                    if (aIntersectionResult == null)
                        continue;

                    XYZ aXYZ_Max = aIntersectionResult.XYZPoint;
                    UV aUV_Max = aIntersectionResult.UVPoint;

                    aIntersectionResult = face.Project(aBoundingBoxXYZ.Min);
                    if (aIntersectionResult == null)
                        continue;

                    XYZ aXYZ_Min = aIntersectionResult.XYZPoint;
                    UV aUV_Min = aIntersectionResult.UVPoint;

                    double aU = aUV_Max.U - aUV_Min.U;
                    double aV = aUV_Max.V - aUV_Min.V;

                    XYZ aXYZ_V = face.Evaluate(new UV(aUV_Max.U, aUV_Max.V - aV));
                    if (aXYZ_V == null)
                        continue;

                    XYZ aXYZ_U = face.Evaluate(new UV(aUV_Max.U - aU, aUV_Max.V));
                    if (aXYZ_U == null)
                        continue;

                    List<oM.Geometry.Point> aPointList = new List<oM.Geometry.Point>();
                    aPointList.Add(aXYZ_Max.ToBHoM(pullSettings));
                    aPointList.Add(aXYZ_U.ToBHoM(pullSettings));
                    aPointList.Add(aXYZ_Min.ToBHoM(pullSettings));
                    aPointList.Add(aXYZ_V.ToBHoM(pullSettings));
                    aPointList.Add(aXYZ_Max.ToBHoM(pullSettings));

                    BuildingElementPanel aBuildingElementPanel = Create.BuildingElementPanel(new oM.Geometry.Polyline[] { Geometry.Create.Polyline(aPointList) });

                    BuildingElement aBuildingElement = Convert.ToBHoMBuildingElement(aElement_Hosted, aBuildingElementPanel, pullSettings);
                    if (aBuildingElement != null)
                        aBuildingElmementList.Add(aBuildingElement);

                }
            }
            return aBuildingElmementList;
        }

        /***************************************************/
    }
}

