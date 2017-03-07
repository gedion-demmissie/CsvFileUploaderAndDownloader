using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvUploaderDataAccess
{
    public class CsvFile
    {
        public int Id { get; set; }
        public byte[] Content { get; set; }
        public string FileName { get; set; }

    }
}
