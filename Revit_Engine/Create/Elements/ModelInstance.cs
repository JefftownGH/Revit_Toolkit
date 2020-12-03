/*
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

using BH.oM.Adapters.Revit.Elements;
using BH.oM.Adapters.Revit.Properties;
using BH.oM.Geometry;
using BH.oM.Reflection.Attributes;
using System.ComponentModel;

namespace BH.Engine.Adapters.Revit
{
    public static partial class Create
    {
        /***************************************************/
        /****              Public methods               ****/
        /***************************************************/

        [Description("Creates ModelInstance object based on point location, Revit family name and family type name. Such ModelInstance can be pushed to Revit as a point-driven element, e.g. chair.")]
        [Input("familyName", "Name of Revit family to be used when creating the element.")]
        [Input("familyTypeName", "Name of Revit family type to be used when creating the element.")]
        [InputFromProperty("location")]
        [InputFromProperty("orientation")]
        [InputFromProperty("hostId")]
        [Output("modelInstance")]
        public static ModelInstance ModelInstance(string familyName, string familyTypeName, Point location, Basis orientation = null, int hostId = -1)
        {
            if (location == null || string.IsNullOrWhiteSpace(familyTypeName) || string.IsNullOrWhiteSpace(familyName))
                return null;

            return ModelInstance(Create.InstanceProperties(familyName, familyTypeName), location, orientation, hostId);
        }

        /***************************************************/

        [Description("Creates ModelInstance object based on curve location, Revit family name and family type name. Such ModelInstance can be pushed to Revit as a curve-driven element, e.g. duct.")]
        [Input("familyName", "Name of Revit family to be used when creating the element.")]
        [Input("familyTypeName", "Name of Revit family type to be used when creating the element.")]
        [InputFromProperty("location")]
        [InputFromProperty("hostId")]
        [Output("modelInstance")]
        public static ModelInstance ModelInstance(string familyName, string familyTypeName, ICurve location, int hostId = -1)
        {
            if (location == null || string.IsNullOrWhiteSpace(familyTypeName) || string.IsNullOrWhiteSpace(familyName))
                return null;

            return ModelInstance(Create.InstanceProperties(familyName, familyTypeName), location, hostId);
        }

        /***************************************************/

        [Description("Creates ModelInstance object based on point location and BHoM InstanceProperties. Such ModelInstance can be pushed to Revit as a point-driven element, e.g. chair.")]
        [InputFromProperty("properties")]
        [InputFromProperty("location")]
        [InputFromProperty("orientation")]
        [InputFromProperty("hostId")]
        [Output("modelInstance")]
        public static ModelInstance ModelInstance(InstanceProperties properties, Point location, Basis orientation = null, int hostId = -1)
        {
            if (properties == null || location == null)
                return null;

            ModelInstance modelInstance = new ModelInstance()
            {
                Properties = properties,
                Name = properties.Name,
                Location = location,
                Orientation = orientation
            };

            return modelInstance;
        }

        /***************************************************/

        [Description("Creates ModelInstance object based on curve location and BHoM InstanceProperties. Such ModelInstance can be pushed to Revit as a point-driven element, e.g. chair.")]
        [InputFromProperty("properties")]
        [InputFromProperty("location")]
        [InputFromProperty("hostId")]
        [Output("modelInstance")]
        public static ModelInstance ModelInstance(InstanceProperties properties, ICurve location, int hostId = -1)
        {
            if (properties == null || location == null)
                return null;

            ModelInstance modelInstance = new ModelInstance()
            {
                Properties = properties,
                Name = properties.Name,
                Location = location
            };

            return modelInstance;
        }

        /***************************************************/

        [Description("Creates ModelInstance object based on curve location. Such ModelInstance can be pushed to Revit as model line.")]
        [InputFromProperty("location")]
        [Output("modelInstance")]
        public static ModelInstance ModelInstance(ICurve location)
        {
            if (location == null)
                return null;

            InstanceProperties instanceProperties = new InstanceProperties();
            instanceProperties.CategoryName = "Lines";

            ModelInstance modelInstance = new ModelInstance()
            {
                Properties = instanceProperties,
                Name = "Lines",
                Location = location
            };

            return modelInstance;
        }

        /***************************************************/

        [Description("Creates ModelInstance object based on curve location. Such ModelInstance can be pushed to Revit as model line.")]
        [Input("name", "Name of Revit line type to be applied to the model line.")]
        [InputFromProperty("location")]
        [Output("modelInstance")]
        public static ModelInstance ModelInstance(string name, ICurve location)
        {
            if (location == null)
                return null;

            InstanceProperties instanceProperties = new InstanceProperties();
            instanceProperties.Name = name;
            instanceProperties.CategoryName = "Lines";

            ModelInstance modelInstance = new ModelInstance()
            {
                Properties = instanceProperties,
                Name = "Lines",
                Location = location
            };

            return modelInstance;
        }

        /***************************************************/

        [Description("Creates ModelInstance object based on surface location. Such ModelInstance can be pushed to Revit as DirectShape.")]
        [Input("categoryName", "Name of Revit category, to which the pushed DirectShape element will be assigned.")]
        [InputFromProperty("location")]
        [Output("modelInstance")]
        public static ModelInstance ModelInstance(string categoryName, ISurface location)
        {
            if (string.IsNullOrWhiteSpace(categoryName) || location == null)
                return null;

            InstanceProperties instanceProperties = new InstanceProperties();
            instanceProperties.CategoryName = categoryName;

            ModelInstance modelInstance = new ModelInstance()
            {
                Properties = instanceProperties,
                Name = "Surface",
                Location = location
            };

            return modelInstance;
        }

        /***************************************************/

        [Description("Creates ModelInstance object based on solid location. Such ModelInstance can be pushed to Revit as DirectShape.")]
        [Input("categoryName", "Name of Revit category, to which the pushed DirectShape element will be assigned.")]
        [InputFromProperty("location")]
        [Output("modelInstance")]
        public static ModelInstance ModelInstance(string categoryName, ISolid location)
        {
            if (string.IsNullOrWhiteSpace(categoryName) || location == null)
                return null;

            InstanceProperties instanceProperties = new InstanceProperties();
            instanceProperties.CategoryName = categoryName;

            ModelInstance modelInstance = new ModelInstance()
            {
                Properties = instanceProperties,
                Name = "Solid",
                Location = location
            };

            return modelInstance;
        }

        /***************************************************/

        [Description("Creates ModelInstance object based on other BHoMObject's properties. After assigning location to it, such ModelInstance can be pushed to Revit as any model element.")]
        [Input("bHoMObject", "BHoM object to inherit properties from.")]
        [Output("modelInstance")]
        public static ModelInstance ModelInstance(oM.Base.IBHoMObject bHoMObject)
        {
            if (bHoMObject == null)
                return null;

            InstanceProperties instanceProperties = new InstanceProperties()
            {
                Name = bHoMObject.Name,
                CustomData = new System.Collections.Generic.Dictionary<string, object>(bHoMObject.CustomData)
            };

            ModelInstance modelInstance = new ModelInstance()
            {
                Name = bHoMObject.Name,
                Properties = instanceProperties,
                CustomData = new System.Collections.Generic.Dictionary<string, object>(bHoMObject.CustomData)
            };

            return modelInstance;
        }


        /***************************************************/
        /****            Deprecated methods             ****/
        /***************************************************/

        [ToBeRemoved("4.0", "Replaced with same named method with more parameters.")]
        [Description("Creates ModelInstance object based on point location, Revit family name and family type name. Such ModelInstance can be pushed to Revit as a point-driven element, e.g. chair.")]
        [Input("familyName", "Name of Revit family to be used when creating the element.")]
        [Input("familyTypeName", "Name of Revit family type to be used when creating the element.")]
        [InputFromProperty("location")]
        [Output("modelInstance")]
        public static ModelInstance ModelInstance(string familyName, string familyTypeName, Point location)
        {
            return Create.ModelInstance(familyName, familyTypeName, location, null, -1);
        }

        /***************************************************/

        [ToBeRemoved("4.0", "Replaced with same named method with more parameters.")]
        [Description("Creates ModelInstance object based on curve location, Revit family name and family type name. Such ModelInstance can be pushed to Revit as a curve-driven element, e.g. duct.")]
        [Input("familyName", "Name of Revit family to be used when creating the element.")]
        [Input("familyTypeName", "Name of Revit family type to be used when creating the element.")]
        [InputFromProperty("location")]
        [Output("modelInstance")]
        public static ModelInstance ModelInstance(string familyName, string familyTypeName, ICurve location)
        {
            return Create.ModelInstance(familyName, familyTypeName, location, -1);
        }

        /***************************************************/

        [ToBeRemoved("4.0", "Replaced with same named method with more parameters.")]
        [Description("Creates ModelInstance object based on point location and BHoM InstanceProperties. Such ModelInstance can be pushed to Revit as a point-driven element, e.g. chair.")]
        [InputFromProperty("properties")]
        [InputFromProperty("location")]
        [Output("modelInstance")]
        public static ModelInstance ModelInstance(InstanceProperties properties, Point location)
        {
            return Create.ModelInstance(properties, location, null, -1);
        }

        /***************************************************/

        [ToBeRemoved("4.0", "Replaced with same named method with more parameters.")]
        [Description("Creates ModelInstance object based on curve location and BHoM InstanceProperties. Such ModelInstance can be pushed to Revit as a point-driven element, e.g. chair.")]
        [InputFromProperty("properties")]
        [InputFromProperty("location")]
        [Output("modelInstance")]
        public static ModelInstance ModelInstance(InstanceProperties properties, ICurve location)
        {
            return Create.ModelInstance(properties, location, -1);
        }

        /***************************************************/
    }
}


