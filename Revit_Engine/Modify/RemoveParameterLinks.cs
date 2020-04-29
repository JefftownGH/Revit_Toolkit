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

using BH.oM.Adapters.Revit.Generic;
using BH.oM.Adapters.Revit.Settings;
using BH.oM.Reflection.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;


namespace BH.Engine.Adapters.Revit
{
    public static partial class Modify
    {
        /***************************************************/
        /****              Public methods               ****/
        /***************************************************/

        [Description("Removes link between Revit parameters and BHoM type property or CustomData inside existing ParameterMap.")]
        [Input("parameterMap", "ParameterMap to be modified.")]
        [Input("propertyName", "BHoM type property or CustomData name.")]
        [Input("parameterNames", "Revit parameter names to be removed.")]
        [Output("parameterMap")]
        public static ParameterMap RemoveParameterLinks(this ParameterMap parameterMap, IEnumerable<string> propertyNames)
        {
            if (parameterMap == null)
                return null;

            if (parameterMap.Type == null || parameterMap.ParameterLinks == null || propertyNames == null || propertyNames.Count() == 0)
                return parameterMap;

            ParameterMap clonedMap = parameterMap.GetShallowClone() as ParameterMap;
            clonedMap.ParameterLinks = parameterMap.ParameterLinks.Where(x => propertyNames.All(y => x.PropertyName != y)).ToList();
            return clonedMap;
        }

        /***************************************************/

        [Description("Removes link between Revit parameter and BHoM type property or CustomData inside existing ParameterMap.")]
        [Input("parameterSettings", "ParameterSettings to be modified.")]
        [Input("type", "Type related to ParameterMap meant to be modified.")]
        [Input("propertyName", "BHoM type property or CustomData name.")]
        [Input("parameterName", "Revit parameter name to be removed.")]
        [Output("parameterSettings")]
        public static ParameterSettings RemoveParameterLinks(this ParameterSettings parameterSettings, Type type, IEnumerable<string> propertyNames)
        {
            if (parameterSettings == null)
                return null;

            if (type == null || propertyNames == null || propertyNames.Count() == 0)
                return parameterSettings;

            ParameterMap parameterMap = parameterSettings.ParameterMaps.Find(x => x.Type == type);
            if (parameterMap == null)
                return parameterSettings;

            ParameterSettings cloneSettings = parameterSettings.GetShallowClone() as ParameterSettings;
            cloneSettings.ParameterMaps = new List<ParameterMap>(parameterSettings.ParameterMaps);
            cloneSettings.ParameterMaps.Remove(parameterMap);
            cloneSettings.ParameterMaps.Add(parameterMap.RemoveParameterLinks(propertyNames));
            return cloneSettings;
        }

        /***************************************************/
    }
}
