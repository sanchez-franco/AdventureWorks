using System;
using System.Configuration;
using System.Transactions;

namespace AdventureWorks.Business
{
    public static class TransactionHelper
    {
        public static int Timeout
        {
            get
            {
                int retValue;
                if (!int.TryParse(ConfigurationManager.AppSettings["Timeout"], out retValue))
                {
                    retValue = 60 * 15;
                }

                return retValue;
            }
        }

        public static TransactionScope CreateTransactionScope()
        {
            var transactionOptions = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
                Timeout = TimeSpan.FromSeconds(Timeout)
            };

            return new TransactionScope(TransactionScopeOption.Required, transactionOptions);
        }
    }
}