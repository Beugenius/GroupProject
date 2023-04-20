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
