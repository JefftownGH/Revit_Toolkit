﻿using System.Collections.Generic;

using Autodesk.Revit.DB;
using BH.oM.Base;
using System.Linq;

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
        /// <param name="element">Revit Element</param>
        /// <param name="objects">BHoM Objects</param>
        /// <returns name="Level">BHoM Level</returns>
        /// <search>
        /// Query, Level, Revit, Level, element
        /// </search>
        static public oM.Architecture.Elements.Level Level(Element element, Dictionary<ElementId, List<BHoMObject>> objects = null)
        {
            oM.Architecture.Elements.Level aLevel = null;
            if (objects != null)
            {
                List<BHoMObject> aBHoMObjectList = new List<BHoMObject>();
                if (objects.TryGetValue(element.LevelId, out aBHoMObjectList))
                    if (aBHoMObjectList != null && aBHoMObjectList.Count > 0)
                        aLevel = aBHoMObjectList.First() as oM.Architecture.Elements.Level;
            }

            if (aLevel == null)
            {
                aLevel = (element.Document.GetElement(element.LevelId) as Level).ToBHoM() as oM.Architecture.Elements.Level;
                if (objects != null)
                    objects.Add(element.LevelId, new List<BHoMObject>(new BHoMObject[] { aLevel }));
            }

            return aLevel;
        }

        /***************************************************/
    }
}