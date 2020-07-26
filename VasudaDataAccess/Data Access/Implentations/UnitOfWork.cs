using System.Data.Entity;
using VasudaDataAccess.Model;

namespace VasudaDataAccess.Data_Access.Implentations
{
    public class UnitOfWork : IUnitOfWork
    {
        public DbContext _dbContext { get; set; }
        public UnitOfWork(DbContext context)
        {
            _dbContext = context;
            CategoryTable = new CategoryTableRepo(context);
            ExchangeRateTable = new ExchangeRateTableRepo(context);
            ImageTable = new ImageTableRepo(context);
            NotificationTable = new NotificationTableRepo(context);
            OrderTable = new OrderTableRepo(context);
            PaymentHistoryTable = new PaymentHistoryTableRepo(context);
            PriceTable = new PriceTableRepo(context);
            ProductTable = new ProductTableRepo(context);
            SettingTable = new SettingTableRepo(context);
            SubCategoryTable = new SubCategoryTableRepo(context);
            SupportTable = new SupportTableRepo(context);
            VendorTable = new VendorTableRepo(context);
            WithdrawalDetailsTable = new WithdrawalDetailsTableRepo(context);
            BankTable = new BankTableRepo(context);
            ContactTable = new ContactTableRepo(context);
            AspNetUser = new AspNetUserRepo(context);
            WithdrawalRequestTable = new WithdrawalRequestTableRepo(context);
            FundingRequestTable = new FundingRequestTableRepo(context);
            SystemAccountTable = new SystemAccountTableRepo(context);
            ChatTable = new ChatTableRepo(context);
        }

        public IAspNetUser AspNetUser { get; }
        public ISystemAccountTable SystemAccountTable { get; }
        public IFundingRequestTable FundingRequestTable { get; }
        public IWithdrawalRequestTable WithdrawalRequestTable { get; }
        public IContactTable ContactTable { get; }
        public ICategoryTable CategoryTable { get; }
        public IExchangeRateTable ExchangeRateTable { get; }
        public IImageTable ImageTable { get; }
        public INotificationTable NotificationTable { get; }
        public IOrderTable OrderTable { get; }
        public IPaymentHistoryTable PaymentHistoryTable { get; }
        public IPriceTable PriceTable { get; }
        public IProductTable ProductTable { get; }
        public ISettingTable SettingTable { get; }
        public ISubCategoryTable SubCategoryTable { get; }
        public ISupportTable SupportTable { get; }
        public IVendorTable VendorTable { get; }
        public IWithdrawalDetailsTable WithdrawalDetailsTable { get; }
        public IBankTable BankTable { get; }
        public IChatTable ChatTable { get; }
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