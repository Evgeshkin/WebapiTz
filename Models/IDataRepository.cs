using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapitestsql.Models
{
    public interface IDataRepository
    {
        IQueryable<AccountHistory> AccountHistorys { get; }
        IQueryable<Account> Account { get; }
        AccountHistory GetAccountHistoey(int id);
        Account GetAccountByID(int id);
        decimal UpdateAccount(AccountHistory accountHistory);
       decimal UpdateAccount(Account account);
        decimal UpdateAccount(List<Account> account);
        (decimal, string) CreateAccountHistory(AccountHistory accountHistory);
        void DeleteAccountHistory(int id);
    }
}
