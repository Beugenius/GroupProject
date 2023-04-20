using GroupProject.Shared;
using GroupProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GroupProject.Main
{
    public class clsMainLogic
    {
        /// <summary>
        /// Reference to the Main SQL strings 
        /// </summary>
        public clsMainSQL MainSQLClass = new();
        /// <summary>
        /// Reference to the class that accesses the database 
        /// </summary>
        public DataAccessClass DataAccessClass = new();
        /// <summary>
        /// The selected invoice to be referenced in the main window 
        /// </summary>
        public Invoices SelectedInvoice { get; set; } = new();
        /// <summary>
        /// The list of items to display in the main window
        /// </summary>
        public List<ItemDesc> ItemsList { get; set; } = new();
        /// <summary>
        /// The list of items to add to an invoice, stored here before saving to the database
        /// </summary>
        public List<ItemDesc> LineItemsToAddToInvoiceOnSaveList { get; set; } = new();
        /// <summary>
        /// The list of items to remove from an invoice, stored here before removing from the database  
        /// </summary>
        public List<ItemDesc> LineItemsToRemoveFromInvoiceOnSaveList { get; set; } = new();
        /// <summary>
        /// Main constructor that defaults the SelectedInvoice to new 
        /// </summary>
        public clsMainLogic()
        {
            SelectedInvoice = new Invoices();
        }

        #region Database Calls 
        /// <summary>
        /// Database call to retrieve all items. Converts items into ItemDesc type, and fills up a list of ItemDesc
        /// </summary>
        /// <returns>A list of ItemDesc</returns>
        public List<ItemDesc> GetAllItems()
        {
            try
            {
                List<ItemDesc> ItemsListToReturn = new();
                int ItemsReturnedInt = 0;
                var itemsFromDb = DataAccessClass.ExecuteSQLStatement(MainSQLClass.GetAllItemDescFromDatabase(), ref ItemsReturnedInt).Tables[0].Rows;
                foreach (DataRow item in itemsFromDb)
                {
                    ItemDesc ItemInRow = new ItemDesc()
                    {
                        ItemCode = item.ItemArray[0].ToString(),
                        ItemDescription = item.ItemArray[1].ToString(),
                        Cost = (Decimal)item.ItemArray[2]
                    };
                    ItemsListToReturn.Add(ItemInRow);
                }
                return ItemsListToReturn;
            }
            catch (Exception ex)
            {
                // Throw the exception with a message 
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// Creates a new invoice in the database 
        /// </summary>
        /// <param name="InvoiceDate">Date of the invoice</param>
        /// <param name="TotalCostInt">Total Cost of the invoice</param>
        /// <returns>The invoice that was created</returns>
        public Invoices CreateNewInvoice(DateTime InvoiceDate, int TotalCostInt)
        {
            try
            {
                int RowsAffectedInt = DataAccessClass.ExecuteNonQuery(MainSQLClass.CreateNewInvoice(InvoiceDate, TotalCostInt));
                // Get the invoices from the database, and find the most recently added one with matching criteria 
                var InvoiceFromDb = GetInvoices().Where(x => x.InvoiceDate == InvoiceDate && x.TotalCost == TotalCostInt).OrderByDescending(x => x.InvoiceNum).First();
                return InvoiceFromDb;
            }
            catch (Exception ex)
            {
                // Throw the exception with a message 
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// Gets all of the invoices in a list.  
        /// </summary>
        /// <returns>A list of Invoices</returns>
        public List<Invoices> GetInvoices()
        {
            try
            {
                List<Invoices> InvoicesToReturn = new();
                int InvoicesReturnedInt = 0;
                var InvoicesFromDb = DataAccessClass.ExecuteSQLStatement(MainSQLClass.GetInvoices(), ref InvoicesReturnedInt).Tables[0].Rows;
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
        /// Gets a list of all the items associated with an invoice
        /// </summary>
        /// <param name="invoiceNumber">Invoice number of the associated list</param>
        /// <returns>A list of ItemDesc associated with a particular invoice</returns>
        public List<ItemDesc> GetAllLineItemsByInvoiceNumber(int invoiceNumber)
        {
            try
            {
                List<ItemDesc> ItemsListToReturn = new();
                int ItemsReturnedInt = 0;
                var itemsFromDb = DataAccessClass.ExecuteSQLStatement(MainSQLClass.GetLineItemsByInvoiceNum(invoiceNumber), ref ItemsReturnedInt).Tables[0].Rows;
                foreach (DataRow item in itemsFromDb)
                {
                    ItemDesc ItemInRow = new ItemDesc()
                    {
                        ItemCode = item.ItemArray[0].ToString(),
                        LineItemNum = (int)item.ItemArray[1],
                        ItemDescription = item.ItemArray[2].ToString(),
                        Cost = (Decimal)item.ItemArray[3]
                    };
                    ItemsListToReturn.Add(ItemInRow);
                }
                return ItemsListToReturn;
            }
            catch (Exception ex)
            {
                // Throw the exception with a message 
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// Saves a line item associated with an invoice to the database 
        /// </summary>
        /// <param name="InvoiceNumberInt">The associated invoice number</param>
        /// <param name="LineItemNumber">The line item number to add into the database (needs to be indexed correctly!)</param>
        /// <param name="ItemCode">The code of the item to add</param>
        public void SaveLineItemToInvoice(int InvoiceNumberInt, int LineItemNumber, string ItemCode)
        {
            try
            {
                int RowsAffected = DataAccessClass.ExecuteNonQuery(MainSQLClass.CreateNewLineItem(InvoiceNumberInt, LineItemNumber, ItemCode));
            }
            catch (Exception ex)
            {
                // Throw the exception with a message 
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// Updates the total cost of the selected invoice 
        /// </summary>
        private void UpdateInvoiceTotalCost()
        {
            try
            {
                int RowsAffectedInt = DataAccessClass.ExecuteNonQuery(MainSQLClass.UpdateInvoiceTotalCost(SelectedInvoice.TotalCost, SelectedInvoice.InvoiceNum));
            }
            catch (Exception ex)
            {
                // Throw the exception with a message 
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// Deletes a line item that is associated with an invoice number
        /// </summary>
        /// <param name="InvoiceNumberInt"></param>
        /// <param name="LineItemNumberInt"></param>
        public void DeleteLineItemByInvoiceNum(int InvoiceNumberInt, int LineItemNumberInt)
        {
            try
            {
                var result = DataAccessClass.ExecuteNonQuery(MainSQLClass.DeleteLineItemByInvoiceId(InvoiceNumberInt, LineItemNumberInt));
            }
            catch (Exception ex)
            {
                // Throw the exception with a message 
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }


        #endregion

        #region Business Logic
        /// <summary>
        /// Loads the ItemsList for the ItemsComboBox 
        /// </summary>
        public void LoadItemsList()
        {
            try
            {
                ItemsList = GetAllItems();
            }
            catch (Exception ex)
            {
                // Throw the exception with a message 
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// Creates and adds an item to the unsaved list of items to add 
        /// </summary>
        /// <param name="ItemCodeString">Item code</param>
        /// <param name="ItemDescriptionString">Description of the item</param>
        /// <param name="CostDecimal">Cost of the item</param>
        public void AddLineItemToInvoiceUnsaved(string ItemCodeString, string ItemDescriptionString, Decimal CostDecimal)
        {
            try
            {
                ItemDesc ItemToAdd = new()
                {
                    ItemCode = ItemCodeString,
                    ItemDescription = ItemDescriptionString,
                    Cost = CostDecimal
                };
                // Add the item to the list, which is just for display purposes
                SelectedInvoice.LineItemsList.Add(ItemToAdd);
                // update total cost
                SelectedInvoice.TotalCost += (int)CostDecimal;
                // Add the item to the list to be added when saved (database update)
                // if line item to add is in the remove list, just remove from the remove list
                if (LineItemsToRemoveFromInvoiceOnSaveList.Contains(ItemToAdd))
                {
                    LineItemsToRemoveFromInvoiceOnSaveList.Remove(ItemToAdd);
                }
                // else queue to be added 
                else
                {
                    LineItemsToAddToInvoiceOnSaveList.Add(ItemToAdd);
                }
            }
            catch (Exception ex)
            {
                // Throw the exception with a message 
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }

        }
        /// <summary>
        /// Creates and adds an item to the unsaved list of items to remove 
        /// </summary>
        /// <param name="ItemCodeString">Item code</param>
        /// <param name="ItemDescriptionString">Description of the item</param>
        /// <param name="CostDecimal">Cost of the item</param>
        public void AddLineItemToBeRemovedFromInvoiceUnsaved(string ItemCodeString, string ItemDescriptionString, Decimal CostDecimal)
        {
            try
            {
                var ItemToRemove = SelectedInvoice.LineItemsList.Where(x => x.ItemCode == ItemCodeString).FirstOrDefault();
                // Add the item to the list, which is just for display purposes
                bool deleted = SelectedInvoice.LineItemsList.Remove(ItemToRemove);
                // update total cost 
                SelectedInvoice.TotalCost -= (int)CostDecimal;
                // If item to be removed is already in list to add, just remove from list to add
                if (LineItemsToAddToInvoiceOnSaveList.Contains(ItemToRemove))
                {
                    LineItemsToAddToInvoiceOnSaveList.Remove(ItemToRemove);
                }
                // else queue for removal 
                else
                {
                    LineItemsToRemoveFromInvoiceOnSaveList.Add(ItemToRemove);
                }
            }
            catch (Exception ex)
            {
                // Throw the exception with a message 
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// Saves the invoice 
        /// </summary>
        public void SaveInvoice()
        {
            try
            {
                foreach (var itemToRemove in LineItemsToRemoveFromInvoiceOnSaveList)
                {
                    // delete line item from db 
                    DeleteLineItemByInvoiceNum(SelectedInvoice.InvoiceNum, itemToRemove.LineItemNum);
                }
                // empty the list to remove 
                LineItemsToRemoveFromInvoiceOnSaveList = new();
                // index for adding line items 
                int index = GetAllLineItemsByInvoiceNumber(SelectedInvoice.InvoiceNum).OrderByDescending(x => x.LineItemNum).Select(x => x.LineItemNum).FirstOrDefault(0) + 1;
                foreach (var itemToAdd in LineItemsToAddToInvoiceOnSaveList)
                {
                    // Add to database 
                    SaveLineItemToInvoice(SelectedInvoice.InvoiceNum, index, itemToAdd.ItemCode);
                    ++index;
                }
                LineItemsToAddToInvoiceOnSaveList = new();
                // Update invoice total 
                SelectedInvoice.TotalCost = (int)GetTotalCost();
                UpdateInvoiceTotalCost();
            }
            catch (Exception ex)
            {
                // Throw the exception with a message 
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// Iterates through all of the items in the invoices line items list and determines total cost of the items 
        /// </summary>
        /// <returns>Total cost of items in selected invoices line items list</returns>
        public decimal GetTotalCost()
        {
            try
            {
                decimal TotalCostInt = 0;
                foreach (var item in SelectedInvoice.LineItemsList)
                {
                    TotalCostInt += item.Cost;
                }
                return TotalCostInt;
            }
            catch (Exception ex)
            {
                // Throw the exception with a message 
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// Returns a new empty invoice with the invoice number == -1 (-1 signifies that invoice is new) 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Invoices NewInvoiceUnsaved()
        {
            try
            {
                return new()
                {
                    InvoiceNum = -1
                };
            }
            catch (Exception ex)
            {
                // Throw the exception with a message 
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        #endregion
    }
}
