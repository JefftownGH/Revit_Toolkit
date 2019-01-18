/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2018, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */

using System.Linq;
using System.Collections.Generic;

using BH.oM.Adapters.Revit.Enums;
using BH.oM.Adapters.Revit.Settings;
using BH.oM.DataManipulation.Queries;

namespace BH.UI.Revit.Engine
{
    public static partial class Query
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        static public Discipline Discipline(this FilterQuery filterQuery, RevitSettings revitSettings)
        {
            Discipline? aDiscipline = null;

            aDiscipline = BH.Engine.Adapters.Revit.Query.Discipline(filterQuery);
            if (aDiscipline != null && aDiscipline.HasValue)
                return aDiscipline.Value;

            aDiscipline = BH.Engine.Adapters.Revit.Query.DefaultDiscipline(filterQuery);
            if (aDiscipline != null && aDiscipline.HasValue)
                return aDiscipline.Value;

            aDiscipline = BH.Engine.Adapters.Revit.Query.DefaultDiscipline(revitSettings);
            if (aDiscipline != null && aDiscipline.HasValue)
                return aDiscipline.Value;

            return oM.Adapters.Revit.Enums.Discipline.Structural;
        }

        /***************************************************/

        static public Discipline Discipline(this IEnumerable<FilterQuery> filterQueries, RevitSettings revitSettings)
        {
            if (filterQueries == null || filterQueries.Count() == 0)
                return oM.Adapters.Revit.Enums.Discipline.Structural;

            Discipline? aDiscipline = null;

            foreach (FilterQuery aFilterQuery in filterQueries)
            {
                aDiscipline = BH.Engine.Adapters.Revit.Query.Discipline(aFilterQuery);
                if (aDiscipline != null && aDiscipline.HasValue)
                    return aDiscipline.Value;
            }

            foreach (FilterQuery aFilterQuery in filterQueries)
            {
                aDiscipline = BH.Engine.Adapters.Revit.Query.DefaultDiscipline(aFilterQuery);
                if (aDiscipline != null && aDiscipline.HasValue)
                    return aDiscipline.Value;
            }

            aDiscipline = BH.Engine.Adapters.Revit.Query.DefaultDiscipline(revitSettings);
            if (aDiscipline != null && aDiscipline.HasValue)
                return aDiscipline.Value;

            return oM.Adapters.Revit.Enums.Discipline.Structural;
        }

        /***************************************************/
    }
}