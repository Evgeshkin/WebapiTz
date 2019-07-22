using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace webapitestsql.Models
{
    public class EDDataRepository : IDataRepository
    {
        private ApplicationDbContext dbcontext;
        public EDDataRepository(ApplicationDbContext ctx)
        {
            dbcontext = ctx;
        }
        public IQueryable<AccountHistory> AccountHistorys => dbcontext.AccountHistory;

        public IQueryable<Account> Account => dbcontext.Account;

        public (decimal, string) CreateAccountHistory(AccountHistory accountHistory)
        {
            Account account = GetAccountByID(accountHistory.AccountID);
            if (account == null) return (0,"Error");
            account.Ballance =(accountHistory.Amount>0)? account.Ballance + accountHistory.Amount : account.Ballance - accountHistory.Amount;
            dbcontext.Account.Add(account);
            dbcontext.Update(account);
            dbcontext.AccountHistory.Add(accountHistory);
            _ = dbcontext.SaveChanges();
            string succes = JsonConvert.SerializeObject(accountHistory);
            return (account.Ballance, "OK");
        }

        public void DeleteAccountHistory(int id)
        {
            throw new NotImplementedException();
        }

        public Account GetAccountByID(int id) => dbcontext.Account.Find(id);

        public AccountHistory GetAccountHistoey(int id) => dbcontext.AccountHistory.Find(id);

        public decimal UpdateAccount(AccountHistory accountHistory)
        {
            Account account = GetAccountByID(accountHistory.AccountID);
            if (account == null) return 0;
            account.Ballance = (accountHistory.Amount > 0) ? account.Ballance - accountHistory.Amount : account.Ballance + accountHistory.Amount;
            if (account.Ballance < 0)
                return 0;
            dbcontext.Account.Add(account);
            dbcontext.Update(account);
            dbcontext.AccountHistory.Add(accountHistory);
            dbcontext.SaveChanges();
            return account.Ballance;
        }

        public decimal UpdateAccount(Account item)
        {

            dbcontext.Account.Add(item);
            dbcontext.Update(item);
            dbcontext.SaveChanges();
            return 1;
        }
        public decimal UpdateAccount(List<Account> account)
        {
            foreach(Account accounts in account)
            {
                dbcontext.Account.Add(accounts);
                dbcontext.Update<Account>(accounts);
            }
            
            return 1;
        }
    }
}
