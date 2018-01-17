﻿using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH = BHoM.Geometry;

namespace Revit2017_Adapter.Geometry
{
    public class GeometryUtils
    {
        public const double FeetToMetre = 0.3048;
        public const double MetreToFeet = 3.28084;

        public static string PointLocation(BH.Point point, int decimals)
        {
            return Math.Round(point.X, decimals) + ";" + Math.Round(point.Y, decimals) + ";" + Math.Round(point.Z, decimals);
        }

        public static string PointLocation(BH.Vector point, int decimals)
        {
            return Math.Round(point.X, decimals) + ";" + Math.Round(point.Y, decimals) + ";" + Math.Round(point.Z, decimals);
        }

        public static BH.Vector ConvertVector(XYZ v)
        {
            return new BHoM.Geometry.Vector(v.X, v.Y, v.Z);
        }

        public static BH.Point Convert(XYZ point, int rounding = 9)
        {
            return new BHoM.Geometry.Point(Math.Round(point.X * FeetToMetre, rounding), Math.Round(point.Y * FeetToMetre, rounding), Math.Round(point.Z * FeetToMetre, rounding));
        }

        public static XYZ Convert(BH.Point point)
        {
            return new XYZ(point.X * MetreToFeet, point.Y * MetreToFeet, point.Z * MetreToFeet);
        }

        //internal static IList<GeometryObject> Convert(BH.Mesh mesh, ElementId materialId)
        //{
        //    TessellatedShapeBuilder builder = new TessellatedShapeBuilder();

        //    builder.OpenConnectedFaceSet(true);

        //    List<XYZ> vertices = new List<XYZ>();
        //    foreach (BH.Face face in mesh.Faces)
        //    {
        //        vertices.Clear();
        //        for (int i = 0; i < face.Indices.Length; i++)
        //        {
        //            vertices.Add(GeometryUtils.Convert(mesh.Vertices[face.Indices[i]]));
        //        }
        //        builder.AddFace(new TessellatedFace(vertices, materialId));
        //    }

        //    builder.CloseConnectedFaceSet();
            
        //    TessellatedShapeBuilderResult result = builder.Build(TessellatedShapeBuilderTarget.Solid, TessellatedShapeBuilderFallback.Abort, ElementId.InvalidElementId);

        //    return result.GetGeometricalObjects();
        //}

        internal static List<BH.Point> Convert(IEnumerable<XYZ> points, int rounding)
        {
            List<BH.Point> bhPoints = new List<BHoM.Geometry.Point>();
            foreach (XYZ point in points)
            {
                bhPoints.Add(Convert(point, rounding));
            }
            return bhPoints;
        }

        internal static BH.Curve Convert(Curve curve, int rounding)
        {
            if (curve is Line)
            {
                return new BH.Line(Convert(curve.GetEndPoint(0), rounding), Convert(curve.GetEndPoint(1), rounding));
            }
            else if (curve is Arc)
            {
                return new BH.Arc(Convert(curve.GetEndPoint(0), rounding), Convert(curve.GetEndPoint(1), rounding), Convert(curve.Evaluate(0.5, true), rounding));
            }
            else if (curve is NurbSpline)
            {
                NurbSpline spline = curve as NurbSpline;
                return BH.NurbCurve.Create(Convert(spline.CtrlPoints, rounding), spline.Degree, spline.Knots.Cast<double>().ToArray(), spline.Weights.Cast<double>().ToArray());
            }
            else if (curve is Ellipse)
            {
                Ellipse ellipse = curve as Ellipse;
                return new BH.Circle((ellipse.RadiusX + ellipse.RadiusY) / 2, new BH.Plane(Convert(ellipse.Center, rounding), new BH.Vector(Convert(ellipse.Normal, rounding))));
            }
            return null;
        }

        public static bool IsHorizontal(BH.Plane p)
        {
            return BH.Vector.VectorAngle(p.Normal, BH.Vector.ZAxis()) < Math.PI / 48;          
        }

        public static bool IsVertical(BH.Plane p)
        {
            double angle = BH.Vector.VectorAngle(p.Normal, BH.Vector.ZAxis());
            return angle > Math.PI / 2 - Math.PI / 48 && angle < Math.PI / 2 + Math.PI / 48;
        }

        public static BH.Group<BH.Curve> SnapTo(BH.Group<BH.Curve> curves, BH.Plane plane, double tolerance)
        {
            BH.Group<BH.Curve> result = new BH.Group<BH.Curve>();
            for(int i = 0; i < curves.Count; i++)
            {
                List<BH.Curve> segments = curves[i].Explode();
                for (int j = 0; j < segments.Count; j++)
                {
                    if (segments[j] is BH.Line)
                    {
                        BH.Point p1 = segments[j].StartPoint;
                        BH.Point p2 = segments[j].EndPoint;
                        double distance1 = Math.Abs(plane.DistanceTo(p1));
                        double distance2 = Math.Abs(plane.DistanceTo(p2));
                        if (distance1 > 0 && distance1 < tolerance)
                        {
                            if (distance2 > 0 && distance2 < tolerance)
                            {
                                segments[j].Project(plane);
                            }
                            else
                            {
                                p1.Project(plane);
                                segments[j] = new BH.Line(p1, p2);
                            }
                        }
                        else if (distance2 > 0 && distance2 < tolerance)
                        {
                            p2.Project(plane);
                            segments[j] = new BH.Line(p1, p2);
                        }
                    }
                }
                result.AddRange(BH.Curve.Join(segments));
            }
            return result;
        }
    }
}