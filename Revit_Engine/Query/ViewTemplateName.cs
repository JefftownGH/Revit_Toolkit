﻿using BH.oM.DataManipulation.Queries;

namespace BH.Engine.Adapters.Revit
{
    public static partial class Query
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static string ViewTemplateName(this FilterQuery filterQuery)
        {
            if (filterQuery == null)
                return null;

            if (!filterQuery.Equalities.ContainsKey(Convert.FilterQuery.ViewTemplateName))
                return null;

            return filterQuery.Equalities[Convert.FilterQuery.ViewTemplateName] as string;
        }

        /***************************************************/
    }
}