using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsvUploaderBusinessLayer.Models;
using CsvUploaderBusinessLayer.Utilities;
using CsvUploaderDataAccess;

namespace CsvUploaderBusinessLayer
{
    public class CsvUploadManager : ICsvUploadManager
    {

        private readonly ICsvContentDataRepository _iCsvContentDataRepository;
        private readonly CsvFileContent _csvFileContent;
        private Dictionary<string, HashSet<WeightedChildNode>> _adjencyListofCsvContent;
        private  List<string> _backcycleFormingVerticesNames;
        private  bool _firstCycleDetected;
        private readonly char[] _separationCharacters ={ ',', '\r', '\n' };

        public CsvUploadManager(ICsvContentDataRepository iCsvContentDataRepository)
        {
            this._iCsvContentDataRepository = iCsvContentDataRepository;
            this._csvFileContent = new CsvFileContent();
        }

        public void PersistUploadedCsvContent(string fileBaseFileName, byte[] contentBytes)
        {
            var str = System.Text.Encoding.UTF8.GetString(contentBytes, 0, contentBytes.Length);
            if (!string.IsNullOrEmpty(str))
            {
                var cellEntries = str.Split(_separationCharacters, StringSplitOptions.RemoveEmptyEntries);
                //Upload Content
                UploadCsvContent(cellEntries);

                //Validate the input and throw exception if non valid case occurs
                ValidateCycleExistence();

                Save(fileBaseFileName, contentBytes);
            }
        }

        //public byte[] GetCsvContentBytes()
        //{
        //   var csvEdgeEntries = _iCsvContentDataRepository.GetCsvEntries();
        //    var csvRows = new List<string>
        //    {
        //        $"{CsvEntryIndex.ParentPropertyName},{CsvEntryIndex.ChildPropertyName},{CsvEntryIndex.QuantityPropertyName}"
        //    };

        //    //Load Edge Content
        //    csvEdgeEntries.ForEach(x => csvRows.Add( x.Parent + "," + x.Child + "," + x.Quantity));

        //   return  new System.Text.UTF8Encoding().GetBytes(string.Join("\r\n", csvRows));
        //}

        public List<CsvFile> GetFiles()
        {
            return _iCsvContentDataRepository.GetFiles();
        }

        public CsvFile GetFile(int id)
        {
            return _iCsvContentDataRepository.GetCsvFile(id);
        }


        private void UploadCsvContent(string[] parsedEntries)
        {
            //Fix the column headers
            CsvEntryIndex.SetColumnHeaderIndex(parsedEntries);

            //Create AdjecencyList representation of the Csv file content
            this._adjencyListofCsvContent = _csvFileContent.PopulateEntireParentsAndChildren(parsedEntries);
        }

        private void Save(string fileName, byte[] contentBytes)
        {
            /*
            //Parse the cellEntry to CsvEdge entry
            var csvEntries = _csvFileContent.PopulateCsvEntries(parsedEntries);

            //Persist it to the Datastore
            _iCsvContentDataRepository.Save(csvEntries);
            */
            _iCsvContentDataRepository.Save(fileName, contentBytes);
        }

        private HashSet<string> FindAllCvsVerticesNameEntries()
        {
            var csvVertices = new HashSet<string>();
            foreach (var adjecencycontent in _adjencyListofCsvContent)
            {
                csvVertices.UnionWith(new[] { adjecencycontent.Key });
                csvVertices.UnionWith(adjecencycontent.Value.Select(x => x.Name));
            }
            return csvVertices;
        }

        private void ValidateCycleExistence()
        {
           
            var visitedNodes =new Dictionary<string,bool>();
            var recursionStackSimulator = new Dictionary<string, bool>();

            //Reset the container that supposed to hold cycle forming nodes and associated flag
            _backcycleFormingVerticesNames= new List<string>();
            _firstCycleDetected = false;
            


            //Collect all vertices
            var allVerticesNames = FindAllCvsVerticesNameEntries();
            foreach (var vertexName in allVerticesNames)
            {
                
                if (IsBackCycleFormed(vertexName, visitedNodes, recursionStackSimulator))
                {
                    var stringBuilder = FormatFirstDetectredCycle();

                    var cycleFormedExceptionMessage=
                       $"Cycle is formed between Vertices : {stringBuilder}";
                   throw new CsvContentException(cycleFormedExceptionMessage);
                }
            }
            
        }

        private StringBuilder FormatFirstDetectredCycle()
        {
            StringBuilder stringBuilder = new StringBuilder();
            HashSet<string> verticesFormingCycle= new HashSet<string>();
            stringBuilder.Append(_backcycleFormingVerticesNames.ElementAt(0));
            verticesFormingCycle.Add(_backcycleFormingVerticesNames.ElementAt(0));
            for (var index = 1; index < _backcycleFormingVerticesNames.Count; index++)
            {
                stringBuilder.Append($" => {_backcycleFormingVerticesNames.ElementAt(index)}");

                if (verticesFormingCycle.Contains(_backcycleFormingVerticesNames.ElementAt(index)))
                {
                    break;                    
                }
                verticesFormingCycle.Add(_backcycleFormingVerticesNames.ElementAt(index));

            }
            return stringBuilder;
        }

        private bool IsBackCycleFormed(string vertexName, Dictionary<string, bool> visitedNodes,
            Dictionary<string, bool> recursionStackSimulator)
        {
            // if the current vertex is not yet visited
            if (!visitedNodes.ContainsKey(vertexName))
            {
                //Mark the current vertex as visited and part of recursion stack 
                visitedNodes[vertexName] = true;
                recursionStackSimulator[vertexName] = true;
               
                //If the vertex doesn't have outgoing edge
                if (!_adjencyListofCsvContent.ContainsKey(vertexName))
                {
                    recursionStackSimulator.Remove(vertexName);
                    return false;
                }

                //Recusrse for all vertices adjecent to the current vertex
                var adjecentVertices = _adjencyListofCsvContent[vertexName];
                  
                if (adjecentVertices != null)
                {
                    foreach (var adjecentVertex in adjecentVertices)
                    {
                        if (!visitedNodes.ContainsKey(adjecentVertex.Name) &&
                            IsBackCycleFormed(adjecentVertex.Name, visitedNodes, recursionStackSimulator))
                        {
                            _backcycleFormingVerticesNames.Add(vertexName);
                            _backcycleFormingVerticesNames.Add(adjecentVertex.Name);
                            return true;
                        }
                        if (recursionStackSimulator.ContainsKey(adjecentVertex.Name))
                        {
                            _backcycleFormingVerticesNames.Add(vertexName);
                            _backcycleFormingVerticesNames.Add(adjecentVertex.Name);
                            return true;
                        }
                    }

                }
            }

            recursionStackSimulator.Remove(vertexName);
            return false;
        }
    }
}
