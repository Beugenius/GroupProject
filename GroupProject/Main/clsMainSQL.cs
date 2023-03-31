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
                return $"UPDATE Invoices SET {iTotalCost} = 1200 WHERE InvoiceNum = {iInvoiceNumber}";
            }
            catch (Exception ex)
            {
                // Throw the exception with a message 
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        public string CreateNewLineItem(int iInvoiceNum, int iLineItemNum, int iItemCode)
        {
            try
            {
                return $"INSERT INTO LineItems({iInvoiceNum}, {iLineItemNum}, {iItemCode}) Values({iInvoiceNum}, {iLineItemNum}, '{iItemCode}')";
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
                return $"INSERT INTO Invoices({InvoiceDate}, {iTotalCost}) Values(#{InvoiceDate.Date.ToString("M/dd/yyyy")}#, {iTotalCost})";
            }
            catch (Exception ex)
            {
                // Throw the exception with a message 
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        public string DeleteLineItemByInvoiceId(int iInvoiceNum)
        {
            try
            {
                return $"DELETE FROM LineItems WHERE InvoiceNum = {iInvoiceNum}";
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
                return $"SELECT LineItems.ItemCode, ItemDesc.ItemDesc, ItemDesc.Cost FROM LineItems, ItemDesc " +
                          $"Where LineItems.ItemCode = ItemDesc.ItemCode And LineItems.InvoiceNum = {iInvoiceNum}";
                
            }
            catch (Exception ex)
            {
                // Throw the exception with a message 
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }

        }
        
    }
}
