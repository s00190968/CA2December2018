using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounts
{
    public class Account
    {
        public string AccountHolder { get; set; }
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public string AccountType { get; set; }
        public DateTime reviewDate { get; set; }

        public override string ToString()
        {
            return string.Format($"{AccountHolder} - {AccountNumber} - Review: {reviewDate}");
        }
    }
}
