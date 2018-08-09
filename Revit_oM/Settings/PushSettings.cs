﻿using BH.oM.Base;

namespace BH.oM.Revit
{
    public class PushSettings : BHoMObject
    {
        /***************************************************/
        /**** Public Properties                        ****/
        /***************************************************/
        public bool CopyCustomData { get; set; } = true;

        public bool ConvertUnits { get; set; } = true;

        public bool Replace { get; set; } = true;

        /***************************************************/

        public static PushSettings Default = new PushSettings();

        /***************************************************/
    }
}