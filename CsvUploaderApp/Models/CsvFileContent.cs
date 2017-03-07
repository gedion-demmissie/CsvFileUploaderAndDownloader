using System.Collections.Generic;
using CsvUploaderBusinessLayer.Models;

namespace CsvUploaderApp.Models
{
    public class CsvFileContentModel
    {
        //  public List<CsvEntry> CsvEntries { get; set; }
        //  public Dictionary<string,HashSet<WeightedChildNode>> ParentsWithChildrenAdjecencyList { get; set; }
        public int Id { get; set; }
        public string FileName { get; set; }
    }
}