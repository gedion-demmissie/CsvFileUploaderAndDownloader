using System;
using System.Collections.Generic;
using System.Web.WebPages;
using CsvUploaderBusinessLayer.Models;

namespace CsvUploaderBusinessLayer.Utilities
{
    public static class CsvFileContentUtility
    {
        public static List<CsvEntry> PopulateCsvEntries(this CsvFileContent csvFileContent, string[] parsedEntries)
        {
            csvFileContent.CsvEntries= new List<CsvEntry>();
            csvFileContent.ParentsWithChildrenAdjecencyList = new Dictionary<string, HashSet<WeightedChildNode>>();
            for (int index = CsvEntryIndex.TotalNumberOfColumns; index <= parsedEntries.Length - CsvEntryIndex.TotalNumberOfColumns; index+=CsvEntryIndex.TotalNumberOfColumns)
            {
                var csvEntry = new CsvEntry
                {
                    Child = parsedEntries[index + CsvEntryIndex.ChildIndex],
                    Parent = parsedEntries[index + CsvEntryIndex.ParentIndex],
                    Quantity = Convert.ToInt32(parsedEntries[index + CsvEntryIndex.QuantityIndex])
                };
                csvFileContent.CsvEntries.Add(csvEntry);
            }
           
            return csvFileContent.CsvEntries;
        }

        public static Dictionary<string, HashSet<WeightedChildNode>> PopulateEntireParentsAndChildren(this CsvFileContent csvFileContent, string[] parsedEntries)
        {
            csvFileContent.CsvEntries = new List<CsvEntry>();
            csvFileContent.ParentsWithChildrenAdjecencyList = new Dictionary<string, HashSet<WeightedChildNode>>();
            for (int index = CsvEntryIndex.TotalNumberOfColumns; index <= parsedEntries.Length - CsvEntryIndex.TotalNumberOfColumns; index += CsvEntryIndex.TotalNumberOfColumns)
            {
                var parentName = parsedEntries[index + CsvEntryIndex.ParentIndex].Trim();
                ValidateEntry(parentName, CsvEntryIndex.ParentPropertyName);

                var childtName = parsedEntries[index + CsvEntryIndex.ChildIndex].Trim();
               ValidateEntry(childtName, CsvEntryIndex.ChildPropertyName);

                var quantityEntry = parsedEntries[index + CsvEntryIndex.QuantityIndex].Trim();
                ValidateQuanty(quantityEntry);
               
                var child = new WeightedChildNode
                {
                    Name = parsedEntries[index + CsvEntryIndex.ChildIndex].Trim(),
                    Quantity = Convert.ToInt32(parsedEntries[index + CsvEntryIndex.QuantityIndex].Trim())
                };

                if (csvFileContent.ParentsWithChildrenAdjecencyList.ContainsKey(parentName))
                {
                    csvFileContent.ParentsWithChildrenAdjecencyList[parentName].Add(child);
                }
                else
                {
                    var children = new HashSet<WeightedChildNode> { child };
                    csvFileContent.ParentsWithChildrenAdjecencyList.Add(parentName, children);
                }
            }

            return csvFileContent.ParentsWithChildrenAdjecencyList;
        }

        private static void ValidateEntry(string entryName, string entrycolumnName)
        {
            if (string.IsNullOrWhiteSpace(entryName) || entryName==string.Empty)
            {
                throw new CsvContentException($"Invalid {entrycolumnName} entry name");
            }

        }
        private static void ValidateQuanty(string quantityEntry)
        {
            ValidateEntry(quantityEntry, CsvEntryIndex.QuantityPropertyName);
            if (!quantityEntry.IsInt())
            {
                throw new CsvContentException($"non numeirc {CsvEntryIndex.QuantityPropertyName}");
            }

        }
    }
}