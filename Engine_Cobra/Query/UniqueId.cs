﻿using BH.oM.Base;

namespace BH.UI.Cobra.Engine
{
    public static partial class Query
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static string UniqueId(this BHoMObject bHoMObject)
        {
            if (bHoMObject == null)
                return null;

            object aValue = null;
            if (bHoMObject.CustomData.TryGetValue(Convert.AdapterId, out aValue))
            {
                if (aValue is string)
                    return (string)aValue;
                else
                    return null;
            }

            return null;
        }

        /***************************************************/
    }
}
