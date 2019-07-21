using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using webapitestsql.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Runtime.Serialization.Json;

namespace webapitestsql.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private IDataRepository repositury;

        public ValuesController(IDataRepository repo)
        {
            repositury = repo;
        }

        [HttpGet]
        /// <summary>
        /// Полный список операций по счету
        /// </summary>
        /// <param name="id">Индентификатор пользователя</param>
        /// <returns>object список по счёту</returns>
        [HttpGet("{id}/history")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public object Get(int id)
        {
            var accountHistory = from b in repositury.Account where b.ID == id select new { b.Name, b.Ballance, Transaction = from bb in b.AccountHistories orderby bb.ChangeAt descending select bb };
            if (accountHistory != null)
                return JsonConvert.SerializeObject(accountHistory, Formatting.Indented);
            else
            {
                SucessResult sucessResult = new SucessResult();
                sucessResult.Result = "Пользователь не обнаружен.";
                sucessResult.Status = "Error";
                return JsonConvert.SerializeObject(sucessResult);
            }
        }
        /// <summary>
        /// Внесение денег на счет
        /// </summary>
        /// <param name="account_id">Целевой счёт</param>
        /// <param name="amount">Сумма</param>
        /// <returns>текущий баланс счета</returns>
        [HttpPost("{account_id}/top-up")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public object TopUp(int account_id, [FromBody] decimal amount)
        {

            AccountHistory ah = new AccountHistory();
            ah.AccountID = account_id;
            ah.Amount = amount;
            ah.ChangeAt = DateTime.Now;
            ah.ID = 0;
            SucessResult sucessResult = new SucessResult();
            var tes= repositury.CreateAccountHistory(ah);
            if (tes.Item2 != "OK"){
                sucessResult.Status = tes.Item2; sucessResult.Result = "Контрагент не создан";
            } else {
                sucessResult.Status = tes.Item2; sucessResult.Result = tes.Item1.ToString(); }
            
            object result = JsonConvert.SerializeObject(sucessResult);
            return result;
        }

        /// <summary>
        /// Снятие денег со счета
        /// </summary>
        /// <param name="acount_id">Исходный счёт</param>
        /// <param name="amount">Сумма</param>
        /// <returns>текущий баланс счета</returns>
        [HttpPost("{acount_id}/withdraw")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public object GetWithdraw(int acount_id, [FromBody]decimal amount)
        {
            AccountHistory ah = new AccountHistory();
            ah.Amount = amount;
            ah.ChangeAt = DateTime.Now;
            ah.AccountID = acount_id;
            ah.ID = 0;
            decimal tes = repositury.UpdateAccount(ah);
            if (tes > 0)
            {
                SucessResult sucessResult = new SucessResult();
                sucessResult.Result = tes.ToString();
                sucessResult.Status = "Ok";
                return JsonConvert.SerializeObject(sucessResult);
            }
            else
            {
                SucessResult sucessResult = new SucessResult();
                sucessResult.Result = "Пользователь не существует или отрицательный балланс!";
                sucessResult.Status = "Error";
                return JsonConvert.SerializeObject(sucessResult);
            }
        }
        /// <summary>
        /// Перемещение денег между счетами
        /// </summary>
        /// <param name="source_account_id">Исходный счёт</param>
        /// <param name="destination_account_id">Целевой счёт</param>
        /// <param name="amount">Сумма перевода</param>
        /// <returns>object</returns>
        [HttpPost("{source_account_id}/transfer/{destination_account_id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public object GetTransfer(int source_account_id, int destination_account_id, [FromBody] decimal amount)
        {
            Account sourceAccount = repositury.GetAccountByID(source_account_id);
            Account destinationAccount = repositury.GetAccountByID(destination_account_id);
            if(sourceAccount != null && destinationAccount != null) {
            
                if ((sourceAccount.Ballance - amount) > 0)
                {
                    destinationAccount.Ballance += amount;
                    sourceAccount.Ballance -= amount;
                    repositury.UpdateAccount(sourceAccount);
                    repositury.UpdateAccount(destinationAccount);
                    Transfers transfers = new Transfers(sourceAccount.Ballance, destinationAccount.Ballance);
                    return JsonConvert.SerializeObject(transfers);
                }
                else
                {
                    SucessResult sucessResult = new SucessResult();
                    sucessResult.Status = "Error";
                    sucessResult.Result = "Балланс не может быть отрицательным";
                    return JsonConvert.SerializeObject(sucessResult);
                }
            } else
            {
                SucessResult sucessResult = new SucessResult();
                sucessResult.Status = "Error";
                sucessResult.Result = "Контрагент не существует";
                return JsonConvert.SerializeObject(sucessResult);
            }
        }
    }
}
