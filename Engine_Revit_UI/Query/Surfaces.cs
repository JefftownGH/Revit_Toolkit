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

using Autodesk.Revit.DB;
using BH.oM.Adapters.Revit.Settings;
using System.Collections.Generic;

namespace BH.UI.Revit.Engine
{
    public static partial class Query
    {
        /***************************************************/
        /****              Public methods               ****/
        /***************************************************/
        
        public static List<oM.Geometry.ISurface> Surfaces(this GeometryElement geometryElement, Transform transform = null, RevitSettings settings = null)
        {
            if (geometryElement == null)
                return null;

            List<oM.Geometry.ISurface> result = new List<oM.Geometry.ISurface>();
            foreach (GeometryObject geometryObject in geometryElement)
            {
                if (geometryObject is GeometryInstance)
                {
                    GeometryInstance geometryInstance = (GeometryInstance)geometryObject;

                    Transform geometryTransform = geometryInstance.Transform;
                    if (transform != null)
                        geometryTransform = geometryTransform.Multiply(transform.Inverse);

                    List<oM.Geometry.ISurface> surfaces = null;
                    GeometryElement geomElement = null;

                    geomElement = geometryInstance.GetInstanceGeometry(geometryTransform);
                    if (geomElement == null)
                        continue;

                    surfaces = geomElement.Surfaces(null, settings);
                    if (surfaces != null && surfaces.Count != 0)
                        result.AddRange(surfaces);                  
                }
                else if (geometryObject is Solid)
                {
                    Solid solid = (Solid)geometryObject;
                    FaceArray faces = solid.Faces;
                    if (faces == null)
                        continue;

                    List<oM.Geometry.ISurface> surfaces = faces.FromRevit();
                    if (surfaces != null && surfaces.Count != 0)
                        result.AddRange(surfaces);                                            
                }              
            }
            return result;
        }

        /***************************************************/

        public static List<oM.Geometry.ISurface> Surfaces(this Element element, Options options, RevitSettings settings = null)
        {
            GeometryElement geometryElement = element.get_Geometry(options);

            Transform transform = null;
            if (element is FamilyInstance)
                transform = ((FamilyInstance)element).GetTotalTransform();

            return geometryElement.Surfaces(transform, settings);
        }

        /***************************************************/
    }
}
