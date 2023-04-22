using GroupProject.Shared;
using GroupProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace GroupProject.Items
{
    public class clsItemsLogic
    {

        /// <summary>
        /// References the class that accesses the database
        /// </summary>
        public DataAccessClass db = new DataAccessClass();
        /// <summary>
        /// references the class that holds all the sql statements
        /// </summary>
        public clsItemsSQL clsItemsSQL = new clsItemsSQL();
        /// <summary>
        /// Gets all of the items from ItemDesc table and puts them in a list
        /// </summary>
        /// <returns>list of ItemDesc</returns>
        /// <exception cref="Exception"></exception>
        public List<ItemDesc> GetItems()
        {
            try
            {
                List<ItemDesc> itemsList = new List<ItemDesc>();
                int itemsReturned = 0;
                var dataBaseItems = db.ExecuteSQLStatement(clsItemsSQL.GetItems(), ref itemsReturned).Tables[0].Rows;
                foreach (DataRow item in dataBaseItems)
                {
                    ItemDesc itemDesc = new ItemDesc();
                    itemDesc.ItemCode = item.ItemArray[0].ToString();
                    itemDesc.ItemDescription = item.ItemArray[1].ToString();
                    itemDesc.Cost = (Decimal)item.ItemArray[2];
                    itemsList.Add(itemDesc);
                }
                return itemsList;
            }
            catch (Exception ex)
            {
                // Throw the exception with a message 
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
            
        }
        /// <summary>
        /// Adds an item to the database
        /// </summary>
        /// <param name="item"></param>
        public void AddItem(ItemDesc item)
        {
            try
            {
                //no null items are added to the database
                if (item.ItemCode == "" || item.ItemDescription == "" || item.Cost == 0)
                {
                    return;
                }
                //check for valid info (no blank info, no copies of item code, correct length of new stuff)
                db.ExecuteNonQuery(clsItemsSQL.InsertItem(item.ItemCode, item.ItemDescription, item.Cost));
            }
            catch (Exception ex)
            {
                // Throw the exception with a message 
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// Replaces the old items information with the new item
        /// </summary>
        /// <param name="oldItem"></param>
        /// <param name="newItem"></param>
        /// <exception cref="Exception"></exception>
        public void EditItem(ItemDesc editItem)
        {
            try
            {
                //check valid info
                db.ExecuteNonQuery(clsItemsSQL.UpdateItem(editItem.ItemCode, editItem.ItemDescription, editItem.Cost));
            }
            catch (Exception ex)
            {
                // Throw the exception with a message 
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        
        /// <summary>
        /// Deletes the item if the item is not part of any invoice
        /// </summary>
        /// <param name="item"></param>
        public void DeleteItem(ItemDesc item)
        {
            try
            {
                if (!IsItemOnInvoice(item))
                {
                    db.ExecuteNonQuery(clsItemsSQL.DeleteItem(item.ItemCode));
                }
                else
                {
                    MessageBox.Show(ItemsOnInvoiceMessage(item));
                }
            }
            catch (Exception ex)
            {
                // Throw the exception with a message 
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// Checks if the item is on any invoices
        /// </summary>
        /// <param name="item"></param>
        /// <returns>bool</returns>
        /// <exception cref="Exception"></exception>
        public bool IsItemOnInvoice(ItemDesc item)
        {
            try
            {
                int itemsReturned = 0;
                var items = db.ExecuteSQLStatement(clsItemsSQL.GetInvoiceNum(item.ItemCode), ref itemsReturned);
                if (itemsReturned > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Throw the exception with a message 
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// returns a string with the list of invoice numbers the item is on
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string ItemsOnInvoiceMessage(ItemDesc item)
        {
            try
            {
                string invoiceNumber = "This item can't be deleted because it is on an invoce. \nThese include: ";
                int itemsReturned = 0;
                var dataBaseItems = db.ExecuteSQLStatement(clsItemsSQL.GetInvoiceNum(item.ItemCode), ref itemsReturned).Tables[0].Rows;
                foreach (DataRow items in dataBaseItems)
                {
                    ItemDesc itemDesc = new ItemDesc();
                    invoiceNumber += items.ItemArray[0].ToString();
                    invoiceNumber += " ";
                }
                return invoiceNumber;
            }
            catch (Exception ex)
            {
                // Throw the exception with a message 
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }

        }

    }
}
