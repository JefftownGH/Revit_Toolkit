﻿using System.Collections.Generic;

using BH.oM.Environment.Elements;
using BH.oM.Environment.Interface;

using Autodesk.Revit.DB;
using BH.oM.Base;
using BH.oM.Adapters.Revit;

namespace BH.Engine.Revit
{

    /// <summary>
    /// BHoM Revit Engine Convert Methods
    /// </summary>
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        internal static Material ToRevit(this IMaterial material, Document document, PushSettings pushSettings = null)
        {
            ElementId aElementId = Material.Create(document, material.Name);
            return document.GetElement(aElementId) as Material;
        }
    }
}
