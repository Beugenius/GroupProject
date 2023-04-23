using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using GroupProject.Main;
using GroupProject.Shared;
using GroupProject.ViewModels;

namespace GroupProject.Search
{
    /// <summary>
    /// Holds all the business logic for the Search Window
    /// </summary>
    internal class clsSearchLogic
    {
        /// <summary>
        /// References Search SQL class
        /// </summary>
        public clsSearchSQL SearchSQLClass;
        /// <summary>
        /// References DataAccessClass to access the database
        /// </summary>
        public DataAccessClass dac = new();
        /// <summary>
        /// List of Invoices that are displayed in Search window
        /// </summary>
        public List<Invoices> InvoiceList { get; set; } = new();
        /// <summary>
        /// List of Invoice numbers (ID's) that are displayed in Search Window.
        /// </summary>
        public List<Invoices> InvoiceNumbers { get; set; } = new();
        public List<string> InvoiceNumbers_s { get; set; } = new();
        /// <summary>
        /// List of Invoice Prices that are displayed.
        /// </summary>
        public List<Invoices> InvoicePrices { get; set; } = new();
        /// <summary>
        /// List of Invoice Prices that are displayed.
        /// </summary>
        public HashSet<string> InvoicePrices_s { get; set; } = new();


        /// <summary>
        /// This calls the GetInvoices method which grabs the Invoices from the database and fills up the InvoiceList with this information.
        /// </summary>
        public List<Invoices> GetInvoices()
        {
            try
            {
                List<Invoices> InvoicesToReturn = new();
                int InvoicesReturnedInt = 0;
                var InvoicesFromDb = dac.ExecuteSQLStatement(clsSearchSQL.GetInvoices(), ref InvoicesReturnedInt).Tables[0].Rows;
                foreach (DataRow invoice in InvoicesFromDb)
                {
                    Invoices ItemInRow = new Invoices()
                    {
                        InvoiceNum = (int)invoice.ItemArray[0],
                        InvoiceDate = (DateTime)invoice.ItemArray[1],
                        TotalCost = (int)invoice.ItemArray[2]
                    };
                    InvoicesToReturn.Add(ItemInRow);
                }
                return InvoicesToReturn;
            }
            catch (Exception ex)
            {
                // Throw the exception with a message 
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// Fills the Invoices List with all invoices specified in the GetSpecificInvoices method in the SQL class.
        /// </summary>
        /// <param name="InvoiceID"></param> Stores InvoiceID
        public List<Invoices> GetInvoices(int InvoiceID)
        {
            try
            {
                List<Invoices> InvoicesToReturn = new();
                int InvoicesReturnedInt = 0;
                var InvoicesFromDb = dac.ExecuteSQLStatement(clsSearchSQL.GetSpecificInvoices(InvoiceID.ToString()), ref InvoicesReturnedInt).Tables[0].Rows;
                foreach (DataRow invoice in InvoicesFromDb)
                {
                    Invoices ItemInRow = new Invoices()
                    {
                        InvoiceNum = (int)invoice.ItemArray[0],
                        InvoiceDate = (DateTime)invoice.ItemArray[1],
                        TotalCost = (int)invoice.ItemArray[2]
                    };
                    InvoicesToReturn.Add(ItemInRow);
                }
                return InvoicesToReturn;
            }
            catch (Exception ex)
            {
                // Throw the exception with a message 
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// This calls the GetInvoicesDate SQL query and fills the Invoice list with invoices ordered on a certain date
        /// </summary>
        /// <param name="invoice_d"></param> Invoice date passed in
        public List<Invoices> GetInvoiceNumbers(DateTime invoice_d)
        {
            try
            {
                List<Invoices> InvoicesToReturn = new();
                int InvoicesReturnedInt = 0;
                var InvoicesFromDb = dac.ExecuteSQLStatement(clsSearchSQL.GetInvoicesDate(invoice_d.ToString()), ref InvoicesReturnedInt).Tables[0].Rows;
                foreach (DataRow invoice in InvoicesFromDb)
                {
                    Invoices ItemInRow = new Invoices()
                    {
                        InvoiceNum = (int)invoice.ItemArray[0],
                        InvoiceDate = (DateTime)invoice.ItemArray[1],
                        TotalCost = (int)invoice.ItemArray[2]
                    };
                    InvoicesToReturn.Add(ItemInRow);
                }
                return InvoicesToReturn;
            }
            catch (Exception ex)
            {
                // Throw the exception with a message 
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// This calls the GetInvoicesNumDate SQL query and makes a list of invoices with a certain ID and order date
        /// </summary>
        /// <param name="invoice_d"></param> Invoice date passed in
        /// <param name="InvoiceID"></param> Invoide ID passed in
        public List<Invoices> GetInvoicesTotalDate(int InvoiceCost, DateTime invoice_d)
        {
            try
            {
                List<Invoices> InvoicesToReturn = new();
                int InvoicesReturnedInt = 0;
                var InvoicesFromDb = dac.ExecuteSQLStatement(clsSearchSQL.GetInvoicesTotalDate(InvoiceCost.ToString(), invoice_d.ToString()), ref InvoicesReturnedInt).Tables[0].Rows;
                foreach (DataRow invoice in InvoicesFromDb)
                {
                    Invoices ItemInRow = new Invoices()
                    {
                        InvoiceNum = (int)invoice.ItemArray[0],
                        InvoiceDate = (DateTime)invoice.ItemArray[1],
                        TotalCost = (int)invoice.ItemArray[2]
                    };
                    InvoicesToReturn.Add(ItemInRow);
                }
                return InvoicesToReturn;
            }
            catch (Exception ex)
            {
                // Throw the exception with a message 
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }


        /// <summary>
        /// Loads the InvoiceList for the InvoiceNumberComboBox 
        /// </summary>
        public void LoadInvoiceList()
        {
            try
            {
                InvoiceList = GetInvoices();
                InvoiceNumbers_s = new List<string>();
                InvoicePrices_s = new HashSet<string>();
                List<int> temp = new List<int>();
                for (int i = 0; i < InvoiceList.Count; i++)
                {
                    InvoiceNumbers_s.Add(InvoiceList[i].InvoiceNum.ToString());
                    temp.Add(InvoiceList[i].TotalCost);
                    

                }
                temp.Sort();
                for(int i = 0; i < temp.Count; i++)
                {
                    InvoicePrices_s.Add(temp[i].ToString());
                }
            }
            catch (Exception ex)
            {
                // Throw the exception with a message 
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Loads up Invoice Date List 
        /// </summary>
        /// <param name="invoice_d"></param> Holds date value
        public void LoadInvoiceDateList(DateTime invoice_d)
        {
            try
            {
                InvoiceNumbers = GetInvoiceNumbers(invoice_d);
                InvoicePrices = GetInvoiceNumbers(invoice_d);
                InvoiceNumbers_s = new List<string>();
                InvoicePrices_s = new HashSet<string>();

                for (int i = 0; i < InvoiceNumbers.Count; i++)
                {
                    InvoiceNumbers_s.Add(InvoiceNumbers[i].InvoiceNum.ToString());
                    InvoicePrices_s.Add(InvoiceNumbers[i].TotalCost.ToString());

                }
            }
            catch (Exception ex)
            {
                // Throw the exception with a message 
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// Loads InvoiceNumbersList with the cost of each invoice in the InvoiceCostComboBox
        /// </summary>
        /// <param name="invoice_d"></param>Holds Invoice Date
        /// <param name="InvoiceID"></param> Holds Invoice ID
        public void LoadInvoiceDateCostList(int InvoiceCost, DateTime invoice_d)
        {
            try
            {
                InvoicePrices = GetInvoicesTotalDate(InvoiceCost, invoice_d);
                InvoicePrices_s = new HashSet<string>();
                for (int i = 0; i < InvoicePrices.Count; i++)
                {
                    InvoicePrices_s.Add(InvoiceNumbers[i].TotalCost.ToString());
                }
            }
            catch (Exception ex)
            {
                // Throw the exception with a message 
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
    }
}
