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

using System.ComponentModel;
using System.Collections.Generic;

using BH.oM.Data.Requests;
using BH.oM.Adapters.Revit.Enums;
using BH.oM.Base;
using BH.oM.Reflection.Attributes;

namespace BH.Engine.Adapters.Revit
{
    public static partial class Create
    {
        [Description("Creates FilterRequest which combines other FilterQueries by logical or operator.")]
        [Input("filterQueries", "Filter Queries to be combined")]
        [Output("FilterRequest")]
        public static FilterRequest LogicalOrFilterRequest(IEnumerable<FilterRequest> filterQueries)
        {
            FilterRequest aFilterRequest = new FilterRequest();
            aFilterRequest.Type = typeof(BHoMObject);
            aFilterRequest.Equalities[Convert.FilterRequest.RequestType] = RequestType.LogicalOr;
            aFilterRequest.Equalities[Convert.FilterRequest.FilterQueries] = filterQueries;
            return aFilterRequest;
        }

        [Description("Creates FilterRequest which combines two FilterQueries by logical or operator.")]
        [Input("filterQuery_1", "First FilterRequest to be combined")]
        [Input("filterQuery_2", "Second FilterRequest to be combined")]
        [Output("FilterRequest")]
        public static FilterRequest LogicalOrFilterRequest(FilterRequest filterQuery_1, FilterRequest filterQuery_2)
        {
            FilterRequest aFilterRequest = new FilterRequest();
            aFilterRequest.Type = typeof(BHoMObject);
            aFilterRequest.Equalities[Convert.FilterRequest.RequestType] = RequestType.LogicalOr;
            aFilterRequest.Equalities[Convert.FilterRequest.FilterQueries] = new List<FilterRequest>() { filterQuery_1, filterQuery_2 };
            return aFilterRequest;
        }
    }
}