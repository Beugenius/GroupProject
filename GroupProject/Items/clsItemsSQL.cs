using GroupProject.ViewModels;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace GroupProject.Items
{
    public class clsItemsSQL
    {
        public string GetItems() 
        { 
            try
            {
                return "select ItemCode, ItemDesc, Cost from ItemDesc";
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        public string GetInvoiceNum(string sItemCode)
        {
            try
            {
                return $"select distinct(InvoiceNum) from LineItems where ItemCode = '{sItemCode}'";
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        public string UpdateItem(string sItemCode, string sItemDescription, decimal iCost)
        {
            try
            {
                return $"Update ItemDesc Set ItemDesc = '{sItemDescription}', Cost = {iCost} where ItemCode = '{sItemCode}'";
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        public string InsertItem(string sItemCode, string sItemDescription, decimal iCost)
        {
            try
            {
                return $"Insert into ItemDesc(ItemCode, ItemDesc, Cost) Values('{sItemCode}', '{sItemDescription}', {iCost})";
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        public string DeleteItem(string sItemCode)
        {
            try
            {
                return $"Delete from ItemDesc Where ItemCode = '{sItemCode}'";
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
    }
}
