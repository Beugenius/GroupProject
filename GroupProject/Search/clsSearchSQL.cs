using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GroupProject.Search
{
    internal class clsSearchSQL
    {
        /* public static string GetInvoices()
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

         public static string GetInvoicesNumDate(string InvoiceID, string InvoiceDate)
        {
            try
            {
                string sSQL = "SELECT * FROM Invoices WHERE InvoiceNum = sInvoiceID AND " + "InvoiceDate = " + InvoiceDate;
                return sSQL;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                   MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

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

        public static string GetInvoicesTotalDate(string TotalCost, string InvoiceDate )
         {
             try
             {
                 string sSQL = ""SELECT * FROM Invoices WHERE TotalCost = TotalCost AND " + "InvoiceDate = " + InvoiceDate;";
                 return sSQL;
             }
             catch (Exception ex)
             {
                 throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
             }
         }


          public static string GetInvoicesDate(string InvoiceDate )
         {
             try
             {
                 string sSQL = ""SELECT * FROM Invoices WHERE InvoiceDate = " + InvoiceDate;
                 return sSQL;
             }
             catch (Exception ex)
             {
                 throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
             }
         }


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
         } */
    }
}