﻿using BH.oM.Adapters.Revit.Settings;

namespace BH.Engine.Adapters.Revit
{
    public static partial class Modify
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/
        
        public static SelectionSettings IncludeSelected(this SelectionSettings selectionSettings, bool includeSelected = false)
        {
            if (selectionSettings == null)
                return null;

            SelectionSettings aSelectionSettings = selectionSettings.GetShallowClone() as SelectionSettings;
            aSelectionSettings.IncludeSelected = includeSelected;

            return aSelectionSettings;
        }

        /***************************************************/
    }
}