﻿/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2020, the respective contributors. All rights reserved.
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

using BH.oM.Adapters.Revit.Enums;
using BH.oM.Adapters.Revit;
using BH.oM.Base;
using BH.oM.Reflection.Attributes;

namespace BH.Engine.Adapters.Revit
{
    public static partial class Create
    {
        /***************************************************/
        /****              Public methods               ****/
        /***************************************************/

        [Description("Creates an IRequest that filters elements by given parameter value criterion.")]
        [Input("parameterName", "Parameter name to be queried")]
        [Input("numberComparisonType", "NumberComparisonType")]
        [Input("value", "Parameter Value. If Revit parameter include units then this value shall be expressed in SI Units")]
        [Output("ParameterNumberRequest")]
        public static ParameterNumberRequest ParameterNumberRequest(string parameterName, NumberComparisonType numberComparisonType, double value, double tolerance = BH.oM.Geometry.Tolerance.Distance)
        {
            return new ParameterNumberRequest { ParameterName = parameterName, NumberComparisonType = numberComparisonType, Value = value, Tolerance = tolerance };
        }

        /***************************************************/
    }
}