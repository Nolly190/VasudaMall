using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VasudaDataAccess.Model;

namespace VasudaDataAccess.Data_Access.Implementation
{
    public class ExchangeRateTableRepo : Repository<ExchangeRateTable>, IExchangeRateTable
    {
        public ExchangeRateTableRepo(DbContext context) : base(context)
        {
                
        }

        public DbContext Context
        {
            get
            {
                return dbcontext as DbContext;
            }
        }

        public List<ExchangeRateTable> GetExchangeRates()
        {
            return  dbcontext.Set<ExchangeRateTable>().Where(x => x.IsActive == true).ToList();
        }

        public ExchangeRateTable GetSingleRate(string baseCurrency, string convertedCurrency)
        {
            var rec = dbcontext.Set<ExchangeRateTable>().Where(x =>
                x.BaseCurrency.ToLower() == baseCurrency.ToLower() &&
                x.ConvertedCurrency.ToLower() ==convertedCurrency.ToLower() && x.IsActive).ToList();
            if (rec.Any())
            {
              return  rec.LastOrDefault();
            }

            return null;
        }
    }
}
