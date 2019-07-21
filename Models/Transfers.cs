using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapitestsql.Models
{
    public class Transfers
    {
        public decimal source_balance { get; set; }
        public decimal destination_balance { get; set; }

        public Transfers(decimal sourcebalance, decimal destinationbalance)
        {
            this.source_balance = sourcebalance;
            this.destination_balance = destinationbalance;
        }
    }
}
