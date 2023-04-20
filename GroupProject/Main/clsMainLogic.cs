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
        public clsMainSQL MainSQLClass = new();
        public DataAccessClass DataAccessClass = new();
        public Invoices SelectedInvoice { get; set; } = new();
        public List<ItemDesc> ItemsList { get; set; }
        public List<ItemDesc> LineItemsToAddToInvoiceOnSaveList { get; set; } = new();
        public List<ItemDesc> LineItemsToRemoveFromInvoiceOnSaveList { get; set; } = new();
        public clsMainLogic()
        {
            SelectedInvoice = new Invoices();
        }

        public List<ItemDesc> GetAllItems()
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
            int i = 0;
            return ItemsListToReturn;
        }
        public void LoadItemsList()
        {
            ItemsList = GetAllItems();
        }

        public List<ItemDesc> GetAllLineItemsByInvoiceNumber(int invoiceNumber)
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

        public void LoadInvoiceItemsAndSetTotalCostOfInvoice()
        {
            SelectedInvoice.LineItemsList = GetAllLineItemsByInvoiceNumber(SelectedInvoice.InvoiceNum);
            Decimal TotalCost = 0.00M;
            foreach (var item in SelectedInvoice.LineItemsList)
            {
                TotalCost += item.Cost;
            }
            SelectedInvoice.TotalCost = (int)TotalCost;
        }

        public void SaveLineItemToInvoice(int InvoiceNumberInt, int LineItemNumber, string ItemCode)
        {
            int RowsAffected = DataAccessClass.ExecuteNonQuery(MainSQLClass.CreateNewLineItem(InvoiceNumberInt, LineItemNumber, ItemCode));
        }

        public void AddLineItemToInvoiceUnsaved(string ItemCodeString, string ItemDescriptionString, Decimal CostDecimal)
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

        public void AddLineItemToBeRemovedFromInvoiceUnsaved(string ItemCodeString, string ItemDescriptionString, Decimal CostDecimal)
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

        public void SaveInvoice()
        {
            foreach (var itemToRemove in LineItemsToRemoveFromInvoiceOnSaveList)
            {
                // delete line item from db 
                DeleteLineItemByInvoiceNum(SelectedInvoice.InvoiceNum, itemToRemove.LineItemNum);
            }
            // empty the list to remove 
            LineItemsToRemoveFromInvoiceOnSaveList = new();
            // index for adding line items 
            int index = GetAllLineItemsByInvoiceNumber(SelectedInvoice.InvoiceNum).OrderByDescending(x=>x.LineItemNum).Select(x=>x.LineItemNum).FirstOrDefault(0) + 1;
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

        private void UpdateInvoiceTotalCost()
        {
            int RowsAffectedInt = DataAccessClass.ExecuteNonQuery(MainSQLClass.UpdateInvoiceTotalCost(SelectedInvoice.TotalCost, SelectedInvoice.InvoiceNum));
        }
        public void DeleteLineItemByInvoiceNum(int InvoiceNumberInt, int LineItemNumberInt)
        {
            var result = DataAccessClass.ExecuteNonQuery(MainSQLClass.DeleteLineItemByInvoiceId(InvoiceNumberInt, LineItemNumberInt));
        }
        public decimal GetTotalCost()
        {
            decimal TotalCostInt = 0;
            foreach (var item in SelectedInvoice.LineItemsList)
            {
                TotalCostInt += item.Cost; 
            }
            return TotalCostInt; 
        }
        public Invoices NewInvoiceUnsaved()
        {
            return new()
            {
                InvoiceNum = -1
            };
        }
        public Invoices CreateNewInvoice(DateTime InvoiceDate, int TotalCostInt)
        {
            int RowsAffectedInt = DataAccessClass.ExecuteNonQuery(MainSQLClass.CreateNewInvoice(InvoiceDate, TotalCostInt));
            var InvoiceFromDb = GetInvoices().Where(x=>x.InvoiceDate == InvoiceDate && x.TotalCost == TotalCostInt).OrderByDescending(x=>x.InvoiceNum).First();
            return InvoiceFromDb;
        }

        public List<Invoices> GetInvoices()
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

    }
}
