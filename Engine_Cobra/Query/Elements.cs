﻿using System;
using System.Linq;
using System.Collections.Generic;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using BH.oM.Base;
using BH.oM.Adapters.Revit.Settings;
using BH.oM.DataManipulation.Queries;


namespace BH.UI.Cobra.Engine
{
    public static partial class Query
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/
        
        public static IEnumerable<Element> Elements(this RevitSettings revitSettings, UIDocument uIDocument)
        {
            if (uIDocument == null || revitSettings == null || revitSettings.SelectionSettings == null)
                return null;

            IEnumerable<Element> aElements = Elements(revitSettings.SelectionSettings, uIDocument);
            if (aElements == null)
                return null;

            if (revitSettings.WorksetSettings == null)
                return aElements;

            List<Element> aElementList = new List<Element>();
            foreach (Element aElement in aElements)
                if (AllowElement(revitSettings.WorksetSettings, aElement))
                    aElementList.Add(aElement);

            return aElementList;
        }

        /***************************************************/

        public static IEnumerable<Element> Elements(this SelectionSettings selectionSettings, UIDocument uIDocument)
        {
            if (uIDocument == null || selectionSettings == null)
                return null;

            Dictionary<int, Element> aDictionary_Elements = new Dictionary<int, Element>();
            if (selectionSettings.ElementIds != null && selectionSettings.ElementIds.Count() > 0)
            {
                foreach(int aId in selectionSettings.ElementIds)
                {
                    ElementId aElementId = new ElementId(aId);
                    Element aElement = uIDocument.Document.GetElement(aElementId);
                    if (aElement != null && !aDictionary_Elements.ContainsKey(aElement.Id.IntegerValue) && AllowElement(selectionSettings, uIDocument, aElement))
                        aDictionary_Elements.Add(aElement.Id.IntegerValue, aElement);
                }
            }

            if (selectionSettings.UniqueIds != null && selectionSettings.UniqueIds.Count() > 0)
            {
                foreach (string aUniqueId in selectionSettings.UniqueIds)
                {
                    if (string.IsNullOrEmpty(aUniqueId))
                        continue;

                    Element aElement = uIDocument.Document.GetElement(aUniqueId);
                    if (aElement != null && !aDictionary_Elements.ContainsKey(aElement.Id.IntegerValue) && AllowElement(selectionSettings, uIDocument, aElement))
                        aDictionary_Elements.Add(aElement.Id.IntegerValue, aElement);
                }
            }

            return aDictionary_Elements.Values;
        }

        /***************************************************/

        public static IEnumerable<Element> Elements(this Document document, Type type)
        {
            if (document == null || type == null)
                return null;

            IEnumerable<Type> aTypes = null;
            if (BH.Engine.Adapters.Revit.Query.IsAssignableFromByFullName(type, typeof(Element)))
                aTypes = new List<Type>() { type };
            else if (BH.Engine.Adapters.Revit.Query.IsAssignableFromByFullName(type, typeof(BHoMObject)))
                aTypes = RevitTypes(type);

            if (aTypes == null || aTypes.Count() == 0)
                return null;

            if (aTypes.Count() == 1)
                return new FilteredElementCollector(document).OfClass(aTypes.First());
            else
                return new FilteredElementCollector(document).WherePasses(new LogicalOrFilter(aTypes.ToList().ConvertAll(x => new ElementClassFilter(x) as ElementFilter)));
        }

        /***************************************************/

        public static IEnumerable<Element> Elements(this Document document, string categoryName)
        {
            if (document == null || string.IsNullOrEmpty(categoryName))
                return null;

            BuiltInCategory aBuiltInCategory = Query.BuiltInCategory(document, categoryName);
            if (aBuiltInCategory == Autodesk.Revit.DB.BuiltInCategory.INVALID)
                return null;

            return new FilteredElementCollector(document).OfCategory(aBuiltInCategory);
        }

        /***************************************************/

        public static IEnumerable<Element> Elements(this Document document, bool activeWorkset, string worksetName = null)
        {
            if (document == null)
                return null;

            List<WorksetId> aWorksetIdList = new List<WorksetId>();
            if(!string.IsNullOrEmpty(worksetName))
            {
                WorksetId aWorksetId = Query.WorksetId(document, worksetName);
                if (aWorksetId != null && aWorksetIdList.Find(x => x == aWorksetId) == null)
                    aWorksetIdList.Add(aWorksetId);
            }

            if(activeWorkset)
            {
                WorksetId aWorksetId = Query.ActiveWorksetId(document);
                if (aWorksetId != null && aWorksetIdList.Find(x => x == aWorksetId) == null)
                    aWorksetIdList.Add(aWorksetId);
            }

            if (aWorksetIdList == null || aWorksetIdList.Count == 0)
                return null;

            if(aWorksetIdList.Count == 1)
                return new FilteredElementCollector(document).WherePasses(new ElementWorksetFilter(aWorksetIdList.First(), false)).ToElements();
            else
                return new FilteredElementCollector(document).WherePasses(new LogicalOrFilter(aWorksetIdList.ConvertAll(x => new ElementWorksetFilter(x, false) as ElementFilter))).ToElements();
        }

        /***************************************************/

        public static IEnumerable<Element> Elements(this FilterQuery filterQuery, UIDocument uIDocument)
        {
            if (uIDocument == null || filterQuery == null)
                return null;

            Dictionary<int, Element> aDictionary_Elements = new Dictionary<int, Element>();

            IEnumerable<Element> aElements = null;

            Document aDocument = uIDocument.Document;

            //Type
            if (filterQuery.Type != null)
            {
                aElements = Elements(uIDocument.Document, filterQuery.Type);
                if (aElements != null)
                {
                    foreach (Element aElement in aElements)
                        if (!aDictionary_Elements.ContainsKey(aElement.Id.IntegerValue))
                            aDictionary_Elements.Add(aElement.Id.IntegerValue, aElement);
                }
            }

            //Workset
            string aWorksetName = BH.Engine.Adapters.Revit.Query.WorksetName(filterQuery);
            bool aActiveWorkset = BH.Engine.Adapters.Revit.Query.ActiveWorkset(filterQuery);
            aElements = Elements(aDocument, aActiveWorkset, aWorksetName);
            if(aElements != null)
            {
                foreach (Element aElement in aElements)
                    if (!aDictionary_Elements.ContainsKey(aElement.Id.IntegerValue))
                        aDictionary_Elements.Add(aElement.Id.IntegerValue, aElement);
            }

            //Category
            string aCategoryName = BH.Engine.Adapters.Revit.Query.CategoryName(filterQuery);
            if (!string.IsNullOrEmpty(aCategoryName))
            {
                aElements = Elements(aDocument, aCategoryName);
                if(aElements != null)
                {
                    foreach(Element aElement in aElements)
                        if (!aDictionary_Elements.ContainsKey(aElement.Id.IntegerValue))
                            aDictionary_Elements.Add(aElement.Id.IntegerValue, aElement);
                }
            }

            //IncludeSelected
            if (BH.Engine.Adapters.Revit.Query.IncludeSelected(filterQuery) && uIDocument.Selection != null)
            {
                ICollection<ElementId> aElementIdCollection = uIDocument.Selection.GetElementIds();
                if (aElementIdCollection != null)
                    foreach (ElementId aElementId in aElementIdCollection)
                        if(!aDictionary_Elements.ContainsKey(aElementId.IntegerValue))
                        {
                            Element aElement = aDocument.GetElement(aElementId);
                            if(aElement != null)
                                aDictionary_Elements.Add(aElementId.IntegerValue, aElement);
                        }
            }

            //ElementIds
            IEnumerable<int> aElementIds = BH.Engine.Adapters.Revit.Query.ElementIds(filterQuery);
            if(aElementIds != null)
            {
                foreach (int aId in aElementIds)
                {
                    ElementId aElementId = new ElementId(aId);
                    Element aElement = aDocument.GetElement(aElementId);
                    if (aElement != null && !aDictionary_Elements.ContainsKey(aElement.Id.IntegerValue))
                        aDictionary_Elements.Add(aElement.Id.IntegerValue, aElement);
                }
            }

            //UniqueIds
            IEnumerable<string> aUniqueIds = BH.Engine.Adapters.Revit.Query.UniqueIds(filterQuery);
            if(aUniqueIds != null)
            {
                foreach (string aUniqueId in aUniqueIds)
                {
                    Element aElement = aDocument.GetElement(aUniqueId);
                    if (aElement != null && !aDictionary_Elements.ContainsKey(aElement.Id.IntegerValue))
                        aDictionary_Elements.Add(aElement.Id.IntegerValue, aElement);
                }
            }

            return aDictionary_Elements.Values;
        }

        /***************************************************/
    }
}