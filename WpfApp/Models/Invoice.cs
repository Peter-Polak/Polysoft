using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp.Models
{
    public class Invoice : ObservableObject
    {
        #region Properties

        public int Year { get; set; } = Int32.Parse(Properties.Settings.Default.Year);
        public string Number { get; set; } = Properties.Settings.Default.InvoiceNumber;

        public Customer Customer { get; set; }
        public double Amount { get; set; }
        public DateTime DateOfPayment { get; set; }
        public DateTime DueDate { get; set; }

        #endregion
    }
}
