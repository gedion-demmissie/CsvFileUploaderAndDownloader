using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvUploaderDataAccess
{
    public class CsvContentDataRepository:ICsvContentDataRepository
    {
        public void Save(string fileName, byte[] contentBytes)
        {
            using (CsvDataContext cntx = new CsvDataContext())
            {
                if (cntx.CsvFileContents.Any(x => x.FileName == fileName))
                {
                    throw new Exception($"Csv file with filename {fileName} already exists");
                }
                CsvFile cdFileContent = new CsvFile
                {
                    Content = contentBytes,
                    FileName = fileName
                };
                cntx.CsvFileContents.Add(cdFileContent);
                cntx.SaveChanges();
            }
        }
        public List<CsvFile> GetFiles()
        {
            using (CsvDataContext cntx = new CsvDataContext())
            {
                return cntx.CsvFileContents.ToList();
            }
        }

        public CsvFile GetCsvFile(int   id)
        {
            using (CsvDataContext cntx = new CsvDataContext())
            {
                return cntx.CsvFileContents.FirstOrDefault(fileContent => fileContent.Id == id);
            }
        }
    }
}
