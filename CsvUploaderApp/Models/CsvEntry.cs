using System;
using System.ComponentModel;
using System.Configuration;

namespace CsvUploaderApp.Models
{
    public class CsvEntry
    {
        public string Parent { get; set; }
        public string Child { get; set; }
        public int Quantity { get; set; }
    }

    public static  class CsvEntryIndex
    {
       
        public static int ParentIndex { get; set; }
        public static int ChildIndex { get; set; }
        public static int QuantityIndex { get; set; }

        public static int TotalNumberOfColumns => Convert.ToInt32(ConfigurationManager.AppSettings["TotalNumberOfColumns"]);

        public static void SetColumnHeaderIndex(string[] parsedEntries)
        {

            for (var i = 0; i < TotalNumberOfColumns; i++)
            {
                SetIndex(parsedEntries[i], i);

            }
        }
        private static void SetIndex(string propertyName, int index)
        {
            if (string.Equals(propertyName, ParentPropertyName, StringComparison.CurrentCultureIgnoreCase))
            {
                ParentIndex = index;
            }
            else if (string.Equals(propertyName, ChildPropertyName, StringComparison.CurrentCultureIgnoreCase))
            {
                ChildIndex = index;
            }
            else if (string.Equals(propertyName, QuantityPropertyName, StringComparison.CurrentCultureIgnoreCase))
            {
                QuantityIndex = index;
            }
        }

        public static string ParentPropertyName => ConfigurationManager.AppSettings["ParentPropertyName"];

        public static string ChildPropertyName => ConfigurationManager.AppSettings["ChildPropertyName"];
        public static string QuantityPropertyName => ConfigurationManager.AppSettings["QuantityPropertyName"];
    }
}