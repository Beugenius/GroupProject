using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace GroupProject.ViewModels
{
    public class Invoices
    {
        public int InvoiceNum { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int TotalCost { get; set; }
    }
}
