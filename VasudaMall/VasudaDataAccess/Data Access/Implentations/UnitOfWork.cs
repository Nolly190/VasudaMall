using System.Data.Entity;

namespace VasudaDataAccess.Data_Access.Implentations
{
    public class UnitOfWork : IUnitOfWork
    {
        public DbContext _dbContext { get; set; }
        public UnitOfWork(DbContext context)
        {
            _dbContext = context;
            CategoryTable = new CategoryTableRepo(context); 

        }

        public ICategoryTable CategoryTable { get; }
        public void Complete()
        {
           _dbContext.SaveChanges();
        }
        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}