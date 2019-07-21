using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace webapitestsql.Models
{
    [DataContract]
    [Table("dt_Account")]
    public class Account
    {
        public int ID { get; set; }
        [Column("account_number")]
        [DataMember(Name = "Name user")]
        public string Name { get; set; }
        [DataMember(Name = "Ballance")]
        [Column("balance")]
        public decimal Ballance { get; set; }
        public List<AccountHistory> AccountHistories { get; set; }
    }
}
