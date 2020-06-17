using System;

namespace VasudaDataAccess.Data_Access
{
    
    interface IUnitOfWork :IDisposable
    {
        void Complete();
    }
}
