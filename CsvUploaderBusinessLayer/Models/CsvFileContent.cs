using System.Collections.Generic;

namespace CsvUploaderBusinessLayer.Models
{
    public class CsvFileContent
    {
        public List<CsvEntry> CsvEntries { get; set; }
        public Dictionary<string,HashSet<WeightedChildNode>> ParentsWithChildrenAdjecencyList { get; set; }
    }
}