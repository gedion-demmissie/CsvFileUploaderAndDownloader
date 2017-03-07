using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvUploaderBusinessLayer.Models
{
    public class CsvContentException : Exception
    {
        public CsvContentException()
            : base() { }

        public CsvContentException(string message)
            : base(message) { }

        public CsvContentException(string format, params object[] args)
            : base(string.Format(format, args)) { }

        public CsvContentException(string message, Exception innerException)
            : base(message, innerException) { }

        public CsvContentException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException) { }
    }
}
