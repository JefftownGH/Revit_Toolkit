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

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure.StructuralSections;
using BH.oM.Environment.Elements;
using BH.oM.Environment.Properties;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Properties.Section.ShapeProfiles;
using BH.oM.Structure.Properties.Surface;
using System;
using System.Collections.Generic;

namespace BH.UI.Revit.Engine
{
    /***************************************************/
    /**** Public Methods                            ****/
    /***************************************************/
    
    public static partial class Query
    {
        public static IEnumerable<Type> BHoMTypes(this Element element)
        {
            List<Type> aResult = new List<Type>();

            if (element is FamilyInstance)
            {
                //Structural framing
                switch ((BuiltInCategory)element.Category.Id.IntegerValue)
                {
                    case Autodesk.Revit.DB.BuiltInCategory.OST_StructuralFraming:
                    //case BuiltInCategory.OST_StructuralFoundation:
                    case Autodesk.Revit.DB.BuiltInCategory.OST_StructuralColumns:
                    case Autodesk.Revit.DB.BuiltInCategory.OST_Columns:
                    case Autodesk.Revit.DB.BuiltInCategory.OST_VerticalBracing:
                    case Autodesk.Revit.DB.BuiltInCategory.OST_Truss:
                    case Autodesk.Revit.DB.BuiltInCategory.OST_StructuralTruss:
                    case Autodesk.Revit.DB.BuiltInCategory.OST_HorizontalBracing:
                    case Autodesk.Revit.DB.BuiltInCategory.OST_Purlin:
                    case Autodesk.Revit.DB.BuiltInCategory.OST_Joist:
                    case Autodesk.Revit.DB.BuiltInCategory.OST_Girder:
                    case Autodesk.Revit.DB.BuiltInCategory.OST_StructuralStiffener:
                    case Autodesk.Revit.DB.BuiltInCategory.OST_StructuralFramingOther:
                        aResult.Add(typeof(FramingElement));
                        return aResult;
                }

                //Environmental Windows and Doors
                switch ((BuiltInCategory)element.Category.Id.IntegerValue)
                {
                    case Autodesk.Revit.DB.BuiltInCategory.OST_Windows:
                    case Autodesk.Revit.DB.BuiltInCategory.OST_Doors:
                        aResult.Add(typeof(BuildingElement));
                        return aResult;
                }
            }

            if(element is FamilySymbol)
            {
                //Environmental Windows and Doors
                switch ((BuiltInCategory)element.Category.Id.IntegerValue)
                {
                    case Autodesk.Revit.DB.BuiltInCategory.OST_Windows:
                    case Autodesk.Revit.DB.BuiltInCategory.OST_Doors:
                        aResult.Add(typeof(BuildingElementProperties));
                        return aResult;
                }
            }

            if (element is CeilingType)
            {
                aResult.Add(typeof(BuildingElementProperties));
                return aResult;
            }

            if (element is WallType)
            {
                aResult.Add(typeof(BuildingElementProperties));
                aResult.Add(typeof(ISurfaceProperty));
                return aResult;
            }

            if (element is FloorType)
            {
                aResult.Add(typeof(BuildingElementProperties));
                aResult.Add(typeof(ISurfaceProperty));
                return aResult;
            }

            if (element is RoofType)
            {
                aResult.Add(typeof(BuildingElementProperties));
                aResult.Add(typeof(ISurfaceProperty));
                return aResult;
            }

            if (BH.Engine.Adapters.Revit.Query.IsAssignableFromByFullName(element.GetType(), typeof(SpatialElement)))
            {
                if(element is Autodesk.Revit.DB.Mechanical.Space)
                {
                    aResult.Add(typeof(Space));
                    return aResult;
                }
            }

            if (element is Wall)
            {
                aResult.Add(typeof(BuildingElement));
                aResult.Add(typeof(PanelPlanar));
                return aResult;
            }

            if (element is Ceiling)
            {
                aResult.Add(typeof(BuildingElement));
                return aResult;
            }


            if (BH.Engine.Adapters.Revit.Query.IsAssignableFromByFullName(element.GetType(), typeof(RoofBase)))
            {
                aResult.Add(typeof(BuildingElement));
                return aResult;
            }

            if (element is Floor)
            {
                aResult.Add(typeof(BuildingElement));
                aResult.Add(typeof(PanelPlanar));
                return aResult;
            }

            if (element is Level)
            {
                aResult.Add(typeof(Level));
                return aResult;
            }

            if (element is Grid)
            {
                aResult.Add(typeof(oM.Architecture.Elements.Grid));
                return aResult;
            }

            if(element is ViewSheet)
            {
                aResult.Add(typeof(oM.Adapters.Revit.Elements.Sheet));
                return aResult;
            }

            if (element is Viewport)
            {
                aResult.Add(typeof(oM.Adapters.Revit.Elements.Viewport));
                return aResult;
            }

            if (element is ViewPlan)
            {
                aResult.Add(typeof(oM.Adapters.Revit.Elements.ViewPlan));
                return aResult;
            }

            if (element is ProjectInfo)
            {
                aResult.Add(typeof(Building));
                return aResult;
            }

            return null; //TODO: shouldn't it be aResult?
        }

        /***************************************************/

        public static IEnumerable<Type> BHoMTypes(this StructuralSectionShape sectionShape)
        {
            int shapeNum = (int)sectionShape;
            List<Type> aResult = new List<Type>();

            if (shapeNum == 2)
            {
                aResult.Add(typeof(RectangleProfile));
                return aResult;
            }

            if (shapeNum == 5)
            {
                aResult.Add(typeof(TubeProfile));
                return aResult;
            }

            if (shapeNum == 6)
            {
                aResult.Add(typeof(ISectionProfile));
                return aResult;
            }

            if (shapeNum == 8)
            {
                aResult.Add(typeof(ISectionProfile));
                return aResult;
            }

            if (shapeNum == 9)
            {
                aResult.Add(typeof(ChannelProfile));
                return aResult;
            }

            if (shapeNum == 11)
            {
                aResult.Add(typeof(AngleProfile));
                return aResult;
            }

            if (shapeNum == 12)
            {
                aResult.Add(typeof(RectangleProfile));
                return aResult;
            }

            if (shapeNum == 13)
            {
                aResult.Add(typeof(CircleProfile));
                return aResult;
            }

            if (shapeNum == 14)
            {
                aResult.Add(typeof(BoxProfile));
                return aResult;
            }

            if (shapeNum == 15)
            {
                aResult.Add(typeof(TubeProfile));
                return aResult;
            }

            if (shapeNum == 16)
            {
                aResult.Add(typeof(FabricatedISectionProfile));
                return aResult;
            }

            if (shapeNum == 17)
            {
                aResult.Add(typeof(TSectionProfile));
                return aResult;
            }

            if (shapeNum == 19)
            {
                aResult.Add(typeof(TSectionProfile));
                return aResult;
            }

            if (shapeNum == 20)
            {
                aResult.Add(typeof(ChannelProfile));
                return aResult;
            }

            if (shapeNum == 23)
            {
                aResult.Add(typeof(AngleProfile));
                return aResult;
            }

            if (shapeNum == 25)
            {
                aResult.Add(typeof(ZSectionProfile));
                return aResult;
            }

            if (shapeNum == 31)
            {
                aResult.Add(typeof(RectangleProfile));
                return aResult;
            }

            if (shapeNum == 33)
            {
                aResult.Add(typeof(TSectionProfile));
                return aResult;
            }

            if (shapeNum == 35)
            {
                aResult.Add(typeof(CircleProfile));
                return aResult;
            }

            return aResult;
        }

        /***************************************************/

        public static IEnumerable<Type> BHoMTypes(this string familyName)
        {
            List<Type> aResult = new List<Type>();

            if (familyName.EndsWith("_Concrete-RectangularBeam"))
            {
                aResult.Add(typeof(RectangleProfile));
                return aResult;
            }

            if (familyName.EndsWith("_ConcreteRectangular"))
            {
                aResult.Add(typeof(RectangleProfile));
                return aResult;
            }

            if (familyName.EndsWith("_ConcreteRectangularWithCL"))
            {
                aResult.Add(typeof(RectangleProfile));
                return aResult;
            }

            if (familyName.EndsWith("_ConcreteSquare"))
            {
                aResult.Add(typeof(RectangleProfile));
                return aResult;
            }

            if (familyName.EndsWith("_ConcreteSquareWithCL"))
            {
                aResult.Add(typeof(RectangleProfile));
                return aResult;
            }

            if (familyName.EndsWith("_Precast-RectangularBeam"))
            {
                aResult.Add(typeof(RectangleProfile));
                return aResult;
            }

            if (familyName.EndsWith("_Precast-RectangularColumn"))
            {
                aResult.Add(typeof(RectangleProfile));
                return aResult;
            }

            if (familyName.EndsWith("_ConcreteRectangular-PrecastWithCL"))
            {
                aResult.Add(typeof(RectangleProfile));
                return aResult;
            }

            if (familyName.EndsWith("_ConcreteRectangular-Precast"))
            {
                aResult.Add(typeof(RectangleProfile));
                return aResult;
            }

            if (familyName.EndsWith("_Precast-SquareColumnWithCL"))
            {
                aResult.Add(typeof(RectangleProfile));
                return aResult;
            }

            if (familyName.EndsWith("_Precast-SquareColumn"))
            {
                aResult.Add(typeof(RectangleProfile));
                return aResult;
            }

            if (familyName.EndsWith("_Precast-RectangularColumnWithCL"))
            {
                aResult.Add(typeof(RectangleProfile));
                return aResult;
            }

            if (familyName == "BHm_StructuralFraming_Timber")
            {
                aResult.Add(typeof(RectangleProfile));
                return aResult;
            }

            if (familyName == "BHm_StructuralColumns_Timber")
            {
                aResult.Add(typeof(RectangleProfile));
                return aResult;
            }

            if (familyName.EndsWith("_LaminatedVeneerLumber"))
            {
                aResult.Add(typeof(RectangleProfile));
                return aResult;
            }

            if (familyName.EndsWith("_ParallelStrandLumberWithCL"))
            {
                aResult.Add(typeof(RectangleProfile));
                return aResult;
            }

            if (familyName.EndsWith("_ParallelStrandLumber"))
            {
                aResult.Add(typeof(RectangleProfile));
                return aResult;
            }

            if (familyName.EndsWith("_Glulam(1)"))
            {
                aResult.Add(typeof(RectangleProfile));
                return aResult;
            }

            if (familyName.EndsWith("_Glulam(1)WithCL"))
            {
                aResult.Add(typeof(RectangleProfile));
                return aResult;
            }

            if (familyName.EndsWith("_Glulam(2)"))
            {
                aResult.Add(typeof(RectangleProfile));
                return aResult;
            }

            if (familyName.EndsWith("_Glulam(2)WithCL"))
            {
                aResult.Add(typeof(RectangleProfile));
                return aResult;
            }

            if (familyName.EndsWith("_DimensionLumber"))
            {
                aResult.Add(typeof(RectangleProfile));
                return aResult;
            }

            if (familyName.EndsWith("_DimensionLumberWithCL"))
            {
                aResult.Add(typeof(RectangleProfile));
                return aResult;
            }

            if (familyName.EndsWith("_TimberWithCL"))
            {
                aResult.Add(typeof(RectangleProfile));
                return aResult;
            }

            if (familyName.EndsWith("_Plate"))
            {
                aResult.Add(typeof(RectangleProfile));
                return aResult;
            }

            if (familyName.EndsWith("_RSJ-RolledSteelJoists"))
            {
                aResult.Add(typeof(ISectionProfile));
                return aResult;
            }

            if (familyName.EndsWith("_UC-UniversalColumns"))
            {
                aResult.Add(typeof(ISectionProfile));
                return aResult;
            }

            if (familyName.EndsWith("_UC-UniversalColumns-Column"))
            {
                aResult.Add(typeof(ISectionProfile));
                return aResult;
            }

            if (familyName.EndsWith("_UB-UniversalBeams"))
            {
                aResult.Add(typeof(ISectionProfile));
                return aResult;
            }

            if (familyName.EndsWith("_UB-UniversalBeams-Column"))
            {
                aResult.Add(typeof(ISectionProfile));
                return aResult;
            }

            if (familyName.EndsWith("_UBP-UniversalBearingPile"))
            {
                aResult.Add(typeof(ISectionProfile));
                return aResult;
            }

            if (familyName.EndsWith("_UBP-UniversalBearingPile-Column"))
            {
                aResult.Add(typeof(ISectionProfile));
                return aResult;
            }

            if (familyName.EndsWith("_ASB-Beams"))
            {
                aResult.Add(typeof(ISectionProfile));
                return aResult;
            }

            if (familyName.EndsWith("_UKC-UKColumns"))
            {
                aResult.Add(typeof(ISectionProfile));
                return aResult;
            }

            if (familyName.EndsWith("_UKC-UKColumns-Column"))
            {
                aResult.Add(typeof(ISectionProfile));
                return aResult;
            }

            if (familyName.EndsWith("_UKB-UKBeams"))
            {
                aResult.Add(typeof(ISectionProfile));
                return aResult;
            }

            if (familyName.EndsWith("_UKB-UKBeams-Column"))
            {
                aResult.Add(typeof(ISectionProfile));
                return aResult;
            }

            if (familyName.EndsWith("_UKBP-UKBearingPiles"))
            {
                aResult.Add(typeof(ISectionProfile));
                return aResult;
            }

            if (familyName.EndsWith("_UKBP-UKBearingPiles-Column"))
            {
                aResult.Add(typeof(ISectionProfile));
                return aResult;
            }

            if (familyName.EndsWith("_IPN-Beams"))
            {
                aResult.Add(typeof(ISectionProfile));
                return aResult;
            }

            if (familyName.EndsWith("_IPN-Column"))
            {
                aResult.Add(typeof(ISectionProfile));
                return aResult;
            }

            if (familyName.EndsWith("_IPE-Beams"))
            {
                aResult.Add(typeof(ISectionProfile));
                return aResult;
            }

            if (familyName.EndsWith("_IPE-Column"))
            {
                aResult.Add(typeof(ISectionProfile));
                return aResult;
            }

            if (familyName.EndsWith("_H-WideFlangeBeams"))
            {
                aResult.Add(typeof(ISectionProfile));
                return aResult;
            }

            if (familyName.EndsWith("_H-WideFlange-Column"))
            {
                aResult.Add(typeof(ISectionProfile));
                return aResult;
            }

            if (familyName.EndsWith("_M-MiscellaneousWideFlange-Column"))
            {
                aResult.Add(typeof(ISectionProfile));
                return aResult;
            }

            if (familyName.EndsWith("_W-WideFlange-Column"))
            {
                aResult.Add(typeof(ISectionProfile));
                return aResult;
            }

            if (familyName.EndsWith("_HP-BearingPile-Column"))
            {
                aResult.Add(typeof(ISectionProfile));
                return aResult;
            }

            if (familyName.EndsWith("_RSJ-RolledSteelJoists-Column"))
            {
                aResult.Add(typeof(ISectionProfile));
                return aResult;
            }

            if (familyName.EndsWith("_RoundBar"))
            {
                aResult.Add(typeof(CircleProfile));
                return aResult;
            }

            if (familyName.EndsWith("_ConcreteRound"))
            {
                aResult.Add(typeof(CircleProfile));
                return aResult;
            }

            if (familyName.EndsWith("_ConcreteRoundWithCL"))
            {
                aResult.Add(typeof(CircleProfile));
                return aResult;
            }

            if (familyName.EndsWith("_PlateGirder"))
            {
                aResult.Add(typeof(FabricatedISectionProfile));
                return aResult;
            }

            if (familyName.EndsWith("_WeldedWideFlange"))
            {
                aResult.Add(typeof(FabricatedISectionProfile));
                return aResult;
            }

            if (familyName.EndsWith("_WeldedReducedFlange"))
            {
                aResult.Add(typeof(FabricatedISectionProfile));
                return aResult;
            }

            if (familyName.EndsWith("_Plate-Column"))
            {
                aResult.Add(typeof(FabricatedISectionProfile));
                return aResult;
            }

            if (familyName.EndsWith("_WWF-WeldedWideFlange-Column"))
            {
                aResult.Add(typeof(FabricatedISectionProfile));
                return aResult;
            }

            if (familyName.EndsWith("_WRF-WeldedReducedFlange-Column"))
            {
                aResult.Add(typeof(FabricatedISectionProfile));
                return aResult;
            }

            if (familyName.EndsWith("_L-Angles"))
            {
                aResult.Add(typeof(AngleProfile));
                return aResult;
            }

            if (familyName.EndsWith("_L-EqualLegAngles"))
            {
                aResult.Add(typeof(AngleProfile));
                return aResult;
            }

            if (familyName.EndsWith("_L-EqualLegAngles-Column"))
            {
                aResult.Add(typeof(AngleProfile));
                return aResult;
            }

            if (familyName.EndsWith("_L-UnequalLegAngles"))
            {
                aResult.Add(typeof(AngleProfile));
                return aResult;
            }

            if (familyName.EndsWith("_L-UnequalLegAngles-Column"))
            {
                aResult.Add(typeof(AngleProfile));
                return aResult;
            }

            if (familyName.EndsWith("_UKA-UKAngles"))
            {
                aResult.Add(typeof(AngleProfile));
                return aResult;
            }

            if (familyName.EndsWith("_UKA-UKAngles-Column"))
            {
                aResult.Add(typeof(AngleProfile));
                return aResult;
            }

            if (familyName.EndsWith("_C-Channels"))
            {
                aResult.Add(typeof(ChannelProfile));
                return aResult;
            }

            if (familyName.EndsWith("_UKPFC-ParallelFlangeChannels"))
            {
                aResult.Add(typeof(ChannelProfile));
                return aResult;
            }

            if (familyName.EndsWith("_UKPFC-ParallelFlangeChannels-Column"))
            {
                aResult.Add(typeof(ChannelProfile));
                return aResult;
            }

            if (familyName.EndsWith("_PFC-ParallelFlangeChannels"))
            {
                aResult.Add(typeof(ChannelProfile));
                return aResult;
            }

            if (familyName.EndsWith("_PFC-ParallelFlangeChannels-Column"))
            {
                aResult.Add(typeof(ChannelProfile));
                return aResult;
            }

            if (familyName.EndsWith("_U-ParallelFlangeChannels"))
            {
                aResult.Add(typeof(ChannelProfile));
                return aResult;
            }

            if (familyName.EndsWith("_U-Channels"))
            {
                aResult.Add(typeof(ChannelProfile));
                return aResult;
            }

            if (familyName.EndsWith("_Precast-SingleTee"))
            {
                aResult.Add(typeof(TSectionProfile));
                return aResult;
            }

            if (familyName.EndsWith("_UKT-UKTeesSplitfromUKC"))
            {
                aResult.Add(typeof(TSectionProfile));
                return aResult;
            }

            if (familyName.EndsWith("_UKT-UKTeesSplitfromUKC-Column"))
            {
                aResult.Add(typeof(TSectionProfile));
                return aResult;
            }

            if (familyName.EndsWith("_UKT-UKTeesSplitfromUKB"))
            {
                aResult.Add(typeof(TSectionProfile));
                return aResult;
            }

            if (familyName.EndsWith("_UKT-UKTeesSplitfromUKB-Column"))
            {
                aResult.Add(typeof(TSectionProfile));
                return aResult;
            }

            if (familyName.EndsWith("_T-TeesfromUniversalColumns"))
            {
                aResult.Add(typeof(TSectionProfile));
                return aResult;
            }

            if (familyName.EndsWith("_T-TeesfromUniversalColumns-Column"))
            {
                aResult.Add(typeof(TSectionProfile));
                return aResult;
            }

            if (familyName.EndsWith("_T-TeesfromUniversalBeams"))
            {
                aResult.Add(typeof(TSectionProfile));
                return aResult;
            }

            if (familyName.EndsWith("_T-TeesfromUniversalBeams-Column"))
            {
                aResult.Add(typeof(TSectionProfile));
                return aResult;
            }

            if (familyName.EndsWith("_MH-TeesfromH-Beams"))
            {
                aResult.Add(typeof(TSectionProfile));
                return aResult;
            }

            if (familyName.EndsWith("_T-Tees"))
            {
                aResult.Add(typeof(TSectionProfile));
                return aResult;
            }

            if (familyName.EndsWith("_MIPE-TeesfromIPE"))
            {
                aResult.Add(typeof(TSectionProfile));
                return aResult;
            }

            if (familyName.EndsWith("_SquareHollowSections"))
            {
                aResult.Add(typeof(BoxProfile));
                return aResult;
            }

            if (familyName.EndsWith("_SquareHollowSections-Column"))
            {
                aResult.Add(typeof(BoxProfile));
                return aResult;
            }

            if (familyName.EndsWith("_RectangularHollowSections"))
            {
                aResult.Add(typeof(BoxProfile));
                return aResult;
            }

            if (familyName.EndsWith("_RectangularHollowSections-Column"))
            {
                aResult.Add(typeof(BoxProfile));
                return aResult;
            }

            if (familyName.EndsWith("_SHS-SquareHollowSections(Cold)"))
            {
                aResult.Add(typeof(BoxProfile));
                return aResult;
            }

            if (familyName.EndsWith("_SHS-SquareHollowSections-Column(Cold)"))
            {
                aResult.Add(typeof(BoxProfile));
                return aResult;
            }

            if (familyName.EndsWith("_SHS-SquareHollowSections"))
            {
                aResult.Add(typeof(BoxProfile));
                return aResult;
            }

            if (familyName.EndsWith("_SHS-SquareHollowSections-Column"))
            {
                aResult.Add(typeof(BoxProfile));
                return aResult;
            }

            if (familyName.EndsWith("_RHS-RectangularHollowSections(Cold)"))
            {
                aResult.Add(typeof(BoxProfile));
                return aResult;
            }

            if (familyName.EndsWith("_RHS-RectangularHollowSections-Column(Cold)"))
            {
                aResult.Add(typeof(BoxProfile));
                return aResult;
            }

            if (familyName.EndsWith("_RHS-RectangularHollowSections"))
            {
                aResult.Add(typeof(BoxProfile));
                return aResult;
            }

            if (familyName.EndsWith("_RHS-RectangularHollowSections-Column"))
            {
                aResult.Add(typeof(BoxProfile));
                return aResult;
            }

            if (familyName.EndsWith("_RectangularandSquareHollowSections"))
            {
                aResult.Add(typeof(BoxProfile));
                return aResult;
            }

            if (familyName.EndsWith("_RectangularandSquareHollowSections-Column"))
            {
                aResult.Add(typeof(BoxProfile));
                return aResult;
            }

            if (familyName.EndsWith("_CircularHollowSections"))
            {
                aResult.Add(typeof(TubeProfile));
                return aResult;
            }

            if (familyName.EndsWith("_CircularHollowSections-Column"))
            {
                aResult.Add(typeof(TubeProfile));
                return aResult;
            }

            if (familyName.EndsWith("_CHS-CircularHollowSections(Cold)"))
            {
                aResult.Add(typeof(TubeProfile));
                return aResult;
            }

            if (familyName.EndsWith("_CHS-CircularHollowSections-Column(Cold)"))
            {
                aResult.Add(typeof(TubeProfile));
                return aResult;
            }

            if (familyName.EndsWith("_CHS-CircularHollowSections"))
            {
                aResult.Add(typeof(TubeProfile));
                return aResult;
            }

            if (familyName.EndsWith("_CHS-CircularHollowSections-Column"))
            {
                aResult.Add(typeof(TubeProfile));
                return aResult;
            }

            if (familyName.EndsWith("_CircularHollowSections"))
            {
                aResult.Add(typeof(TubeProfile));
                return aResult;
            }

            if (familyName.EndsWith("_Pipe-Column"))
            {
                aResult.Add(typeof(TubeProfile));
                return aResult;
            }

            return aResult;
        }

        /***************************************************/
    }
}