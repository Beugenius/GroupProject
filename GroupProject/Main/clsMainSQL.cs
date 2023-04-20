using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace GroupProject.Main
{
    public class clsMainSQL
    {
        /// <summary>
        /// Constructs a SQL string for updating the total cost of an invoice 
        /// </summary>
        /// <param name="iTotalCost">Total cost of the invoice </param>
        /// <param name="iInvoiceNumber">Invoice number</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string UpdateInvoiceTotalCost(int iTotalCost, int iInvoiceNumber)
        {
            try
            {
                return $"UPDATE Invoices SET TotalCost = {iTotalCost} WHERE InvoiceNum = {iInvoiceNumber}";
            }
            catch (Exception ex)
            {
                // Throw the exception with a message 
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// Constructs a SQL string for creating a new line item link in the database 
        /// </summary>
        /// <param name="iInvoiceNum">Invoice number</param>
        /// <param name="iLineItemNum">The line item number to be added</param>
        /// <param name="sItemCode">The code of the item</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string CreateNewLineItem(int iInvoiceNum, int iLineItemNum, string sItemCode)
        {
            try
            {
                return $"INSERT INTO LineItems(InvoiceNum, LineItemNum, ItemCode) Values({iInvoiceNum}, {iLineItemNum}, '{sItemCode}')";
            }
            catch (Exception ex)
            {
                // Throw the exception with a message 
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// Constructs a SQL string for creating a new invoice in the database 
        /// </summary>
        /// <param name="InvoiceDate">Date of the invoice </param>
        /// <param name="iTotalCost">Total cost of the invoice </param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string CreateNewInvoice(DateTime InvoiceDate, int iTotalCost)
        {
            try
            {
                return $"INSERT INTO Invoices(InvoiceDate, TotalCost) Values(#{InvoiceDate.Date.ToString("M/dd/yyyy")}#, {iTotalCost})";
            }
            catch (Exception ex)
            {
                // Throw the exception with a message 
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// Constructs a SQL string for deleting a line item in the database 
        /// </summary>
        /// <param name="iInvoiceNum">Invoice number of the associated invoice with which the line item will be removed</param>
        /// <param name="iLineItemNum">The line item number to be removed</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string DeleteLineItemByInvoiceId(int iInvoiceNum, int iLineItemNum)
        {
            try
            {
                return $"DELETE FROM LineItems WHERE InvoiceNum = {iInvoiceNum} And LineItemNum = {iLineItemNum}";
            }
            catch (Exception ex)
            {
                // Throw the exception with a message 
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// Constructs a SQL string for gathering all items from the database 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string GetAllItemDescFromDatabase()
        {
            try
            {
                return "SELECT ItemCode, ItemDesc, Cost from ItemDesc";
            }
            catch (Exception ex)
            {
                // Throw the exception with a message 
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// Constructs a SQL string for getting all of the line items from the database associated with a particular invoice 
        /// </summary>
        /// <param name="iInvoiceNum">Associated invoice number</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string GetLineItemsByInvoiceNum(int iInvoiceNum)
        {
            try
            {
                return $"SELECT LineItems.ItemCode, LineItems.LineItemNum, ItemDesc.ItemDesc, ItemDesc.Cost FROM LineItems, ItemDesc " +
                          $"Where LineItems.ItemCode = ItemDesc.ItemCode And LineItems.InvoiceNum = {iInvoiceNum}";
                
            }
            catch (Exception ex)
            {
                // Throw the exception with a message 
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }

        }
        /// <summary>
        /// Constructs a string to get all of the invoices from the database. Used in the main window solely for comparison to retrieve most
        /// recently inserted invoice (on creation of new) 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string GetInvoices()
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
    }
}
