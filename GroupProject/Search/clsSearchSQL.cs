using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GroupProject.Search
{   /// <summary>
    /// SearchSQL class holds all SQL statements needs for the SearchWindow 
    /// </summary>
    public class clsSearchSQL
    {
        /// <summary>
        /// Selects all information from Invoices table in database and returns it as sSQL
        /// </summary>
        public static string GetInvoices()
        {
            try
            {
                string sSQL = "SELECT * FROM Invoices";
                return sSQL;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                   MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// Selects all information from Invoice table  with certain InvoiceID's and returns it as a string named sSQL
        /// </summary>
        /// <param name="sInvoiceID"></param>Passes sInvoiceID to help filter
        public static string GetSpecificInvoices(string sInvoiceID)
        {
            try
            {
                string sSQL = "SELECT * FROM Invoices WHERE InvoiceNum = " + sInvoiceID;
                return sSQL;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                   MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// Selects specific Invoices from Invoice table that have a certain InvoiceID and Invoice Date
        /// </summary>
        /// <param name="InvoiceID"></param>InvoiceID is passed in to help filter
        /// <param name="InvoiceDate"></param>InvoiceDate is passed in to help filter invoices ordered by date
        public static string GetInvoicesNumDate(string InvoiceID, string InvoiceDate)
        {
            try
            {
                //string sSQL = "SELECT * FROM Invoices WHERE InvoiceNum = sInvoiceID AND " + "InvoiceDate = " + InvoiceDate;
                string sSQL = "SELECT * FROM Invoices WHERE InvoiceNum = sInvoiceID AND " + "InvoiceDate = '{InvoiceDate}'";
                return sSQL;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                   MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// Filters and grabs Invoices based on certain InvoiceNumbers, InvoiceID's and InvoiceCosts
        /// </summary>
        /// <param name="InvoiceID"></param> Passes in Invoid ID to help filter based on ID
        /// <param name="InvoiceDate"></param> Passes in InvoiceDate to help filter based on order date
        /// <param name="TotalCost"></param> Passes in TotalCost to help filter based on order cost
        public static string InvoicesNumDateCost(string InvoiceID, string InvoiceDate, string TotalCost)
        {
            try
            {
                string sSQL = "SELECT * FROM Invoices WHERE InvoiceNum = sInvoiceID AND " + "InvoiceDate = InvoiceDate AND " + "TotalCost = " + TotalCost;
                return sSQL;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                   MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// Gets Invoices from Database with a certain cost. Helps to populate InvoiceCostComboBox
        /// </summary>
        /// <param name="TotalCost"></param> Stores TotalCost in a string
        public static string GetInvoicesCost(string TotalCost)
        {
            try
            {
                string sSQL = "SELECT * FROM Invoices WHERE TotalCost = " + TotalCost;
                return sSQL;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                   MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// Gets Invoices based on total cost and order date
        /// </summary>
        /// <param name="TotalCost"></param> Total cost stored in string
        /// <param name="InvoiceDate"></param> Invoice date stores in string
        public static string GetInvoicesTotalDate(string TotalCost, string InvoiceDate)
        {
            try
            {
                // TotalCost TotalCost"
                string sSQL = "SELECT * FROM Invoices WHERE TotalCost = " + TotalCost + " AND " + "InvoiceDate = #" + InvoiceDate + "#";
                return sSQL;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                   MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Grabs all Invoices ordered on selected date and returns them as a string
        /// </summary>
        /// <param name="InvoiceDate"></param> Selected date is passed in through here
        public static string GetInvoicesDate(string InvoiceDate)
        {
            try
            {
                string sSQL = "SELECT * FROM Invoices WHERE InvoiceDate = #" + InvoiceDate + "#";
                return sSQL;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                   MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Selects InvoiceNumbers once and puts then in order
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string OrdByInvoiceNum()
        {
            try
            {
                string sSQL = "SELECT DISTINCT(InvoiceNum) FROM Invoices ORDER BY InvoiceNum";
                return sSQL;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                   MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// Selects InvoiceDates and the list is ordered according to the dates
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string OrdByInvoiceDate()
        {
            try
            {
                string sSQL = "SELECT DISTINCT(InvoiceDate) FROM Invoices ORDER BY InvoiceDate";
                return sSQL;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                   MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// Selects all costs and orders the list according to the invoice cost.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string OrdByTotalCost()
        {
            try
            {
                string sSQL = "SELECT DISTINCT(TotalCost) FROM Invoices ORDER BY TotalCost";
                return sSQL;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                   MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
    }
}