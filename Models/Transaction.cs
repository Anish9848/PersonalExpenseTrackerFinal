using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalExpenseTracker.Models
{
    public class Transactionitem
    {
        public int transactionId { get; set; }
        public string transactionTitle { get; set; }  // Add Title
        public string transactionDescription { get; set; }
        public decimal transactionAmount { get; set; }
        public DateTime transactionDate { get; set; }
        public string transactionTag { get; set; }  // Add Tag (or you can use a Tag ID reference)
        public string transactionType { get; set; }
    }
}