using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvUploaderDataAccess
{
    public interface ICsvContentDataRepository
    {
       void Save(string fileName, byte[] contentBytes);
        List<CsvFile> GetFiles();
        CsvFile GetCsvFile(int id);
    }
}
