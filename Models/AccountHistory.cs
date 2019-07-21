using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace webapitestsql.Models
{
    [DataContract]
    [Table("dt_AccountHistory")]
    public class AccountHistory
    {

        [Column("id")]
        public int ID { get; set; }
        [Column("account_id")]
        public int AccountID { get; set; }
        [DataMember(Name = "Transaction")]
        [Column("amount")]
        public decimal Amount { get; set; }
        [DataMember(Name = "Date Time")]
        [Column("changed_at")]
        public DateTime ChangeAt { get; set; }
    }
}
