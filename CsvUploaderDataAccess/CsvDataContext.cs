using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace CsvUploaderDataAccess
{
    public class CsvDataContext: DbContext
    {
        public CsvDataContext()
            : base("name=CvscontentsDb")
        {
        }
        public DbSet<CsvFile> CsvFileContents { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

        }
    }
}
