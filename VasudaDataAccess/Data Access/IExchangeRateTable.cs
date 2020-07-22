using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VasudaDataAccess.Model;

namespace VasudaDataAccess.Data_Access
{
    public interface IExchangeRateTable : IRepository<ExchangeRateTable>
    {
        List<ExchangeRateTable> GetExchangeRates();
        ExchangeRateTable GetSingleRate(string baseCurrency, string convertedCurrency);
    }
}
