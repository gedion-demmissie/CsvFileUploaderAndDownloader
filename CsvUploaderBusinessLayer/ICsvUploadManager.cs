using System.Collections.Generic;
using CsvUploaderBusinessLayer.Models;
using CsvUploaderDataAccess;

namespace CsvUploaderBusinessLayer
{
    public interface ICsvUploadManager
    {
        void PersistUploadedCsvContent(string fileBaseFileName, byte[] contentBytes);
       // byte[] GetCsvContentBytes();
        List<CsvFile> GetFiles();
        CsvFile GetFile(int id);

    }
}