using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalExpenseTracker.Models
{
    public class Debtitem
    {
        public int DebtId { get; set; }
        public string DebtSource { get; set; }
        public decimal DebtAmount { get; set; }
        public DateTime DebtDueDate { get; set; }
        public DateTime DebtTakenDate { get; set; } // New field for the date the debt was taken
        public string Status { get; set; } = "Pending"; // Status is always "Pending"
    }
}