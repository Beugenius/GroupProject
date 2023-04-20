using GroupProject.Items;
using GroupProject.Search;
using GroupProject.ViewModels;
using System;
using System.Linq;
using System.Windows;

namespace GroupProject.Main
{
    /// <summary>
    /// Interaction logic for wndMain.xaml
    /// </summary>
    public partial class wndMain : Window
    {
        public clsMainLogic MainLogicClass { get; set; }
        public bool IsCreatingNewInvoiceBool { get; set; }
        public wndMain()
        {
            InitializeComponent();
            MainLogicClass = new();
            MainLogicClass.LoadItemsList();
            ReloadItemsComboBox();
        }
        #region Menu

        private void SearchMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wndSearch SearchWindow = new();
                SearchWindow.ShowDialog();
                var InvoiceFromSearchWindow = SearchWindow.SelectedInvoice;
                // default / no selected invoice defaults to invoice num == -1 
                if (InvoiceFromSearchWindow != null && InvoiceFromSearchWindow.InvoiceNum != -1)
                {
                    MainLogicClass.SelectedInvoice = InvoiceFromSearchWindow;
                    // set labels and date picker
                    InvoiceDatePicker.SelectedDate = MainLogicClass.SelectedInvoice.InvoiceDate;
                    InvoiceNumberDisplayLabel.Content = MainLogicClass.SelectedInvoice.InvoiceNum;
                    TotalCostDisplayLabel.Content = $"${MainLogicClass.SelectedInvoice.TotalCost}";
                    // Load line items list
                    ReloadLineItems();
                    ReloadInvoiceDataGrid();
                    EditInvoiceButton.IsEnabled = true;
                    SetToReadOnly();
                }
                // otherwise, do nothing
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something broke!");
            }
        }

        private void EditMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wndItems ItemsWindow = new();
                ItemsWindow.ShowDialog();
                bool ItemsChanged = true;// TODO - set this in edit window if changed ItemsWindow.ItemsChanged;
                if (ItemsChanged)
                {
                    // reload items list

                    ReloadItemsComboBox();
                    // reload line items 
                    ReloadLineItems();
                    // reload selected invoice datagrid 
                    ReloadInvoiceDataGrid();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something broke!");
            }
        }

        #endregion

        #region Buttons / Controls

        private void CreateNewInvoiceButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainLogicClass.SelectedInvoice = MainLogicClass.NewInvoiceUnsaved();
                InvoiceNumberDisplayLabel.Content = "TBD";
                TotalCostDisplayLabel.Content = "$0";
                EditInvoiceButton.IsEnabled = true;
                InvoiceDatePicker.SelectedDate = null;
                ReloadInvoiceDataGrid();
                SetToReadOnly();
                // only allow user to select a date when creating new invoice 
                InvoiceDatePicker.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something broke!");
            }
        }

        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            // if an item is selected only 
            if (ItemsComboBox.SelectedItem != null)
            {
                ItemDesc SelectedItem = (ItemDesc)ItemsComboBox.SelectedItem;
                //CreateNewLineItem(int iInvoiceNum, int iLineItemNum, int iItemCode)
                MainLogicClass.AddLineItemToInvoiceUnsaved(SelectedItem.ItemCode, SelectedItem.ItemDescription, SelectedItem.Cost);
                // reload invoice datagrid 
                ReloadInvoiceDataGrid();
                // Display Total Cost
                TotalCostDisplayLabel.Content = $"${MainLogicClass.SelectedInvoice.TotalCost}";
            }
        }

        private void RemoveItemButton_Click(object sender, RoutedEventArgs e)
        {
            // if an item is selected only 
            if (ItemsComboBox.SelectedItem != null)
            {
                ItemDesc SelectedItem = (ItemDesc)ItemsComboBox.SelectedItem;
                //CreateNewLineItem(int iInvoiceNum, int iLineItemNum, int iItemCode)
                if (MainLogicClass.SelectedInvoice.LineItemsList.Any(x => x.ItemCode == SelectedItem.ItemCode))
                {
                    MainLogicClass.AddLineItemToBeRemovedFromInvoiceUnsaved(SelectedItem.ItemCode, SelectedItem.ItemDescription, SelectedItem.Cost);
                    // reload invoice datagrid 
                    ReloadInvoiceDataGrid();
                    // Display Total Cost
                    TotalCostDisplayLabel.Content = $"${MainLogicClass.SelectedInvoice.TotalCost}";
                }
            }
        }

        private void EditInvoiceButton_Click(object sender, RoutedEventArgs e)
        {
            SetToEdit();
        }

        private void SaveInvoiceButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedDate = InvoiceDatePicker.SelectedDate;
            if (selectedDate is not null)
            {
                if (MainLogicClass.SelectedInvoice != null && MainLogicClass.SelectedInvoice.InvoiceNum != -1)
                {
                    MainLogicClass.SaveInvoice();
                    ReloadLineItems();
                    // display total cost 
                    TotalCostDisplayLabel.Content = $"${MainLogicClass.SelectedInvoice.TotalCost}";
                    ReloadInvoiceDataGrid();
                }
                else if (MainLogicClass.SelectedInvoice != null && MainLogicClass.SelectedInvoice.InvoiceNum == -1)
                {
                    DateTime NonNullDate = (DateTime)selectedDate;
                    // save temporary line items added 
                    var UnsavedLineItems = MainLogicClass.SelectedInvoice.LineItemsList;
                    // create the invoice and setting it as the selected invoice 
                    MainLogicClass.SelectedInvoice = MainLogicClass.CreateNewInvoice(NonNullDate, (int)MainLogicClass.GetTotalCost());
                    // set line items added before creation of invoice 
                    MainLogicClass.SelectedInvoice.LineItemsList = UnsavedLineItems;
                    // save the contents of the invoice 
                    MainLogicClass.SaveInvoice();
                    // display total cost 
                    ReloadLineItems();
                    TotalCostDisplayLabel.Content = $"${MainLogicClass.SelectedInvoice.TotalCost}";
                    InvoiceNumberDisplayLabel.Content = MainLogicClass.SelectedInvoice.InvoiceNum;
                    ReloadInvoiceDataGrid();
                }
                SetToReadOnly();
            }
            else
            {
                MessageBox.Show("Must select a date!");
            }

        }

        private void ItemsComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ItemDesc ItemSelected = (ItemDesc)ItemsComboBox.SelectedItem;
            IndividualItemCostDisplayLabel.Content = $"${ItemSelected.Cost}";
        }

        #endregion

        #region Helpers
        private void ReloadInvoiceDataGrid()
        {
            InvoiceDataGrid.ItemsSource = MainLogicClass.SelectedInvoice.LineItemsList;
            InvoiceDataGrid.Items.Refresh();
        }

        private void ReloadItemsComboBox()
        {
            MainLogicClass.ItemsList = MainLogicClass.GetAllItems();
            ItemsComboBox.ItemsSource = MainLogicClass.ItemsList;
            ItemsComboBox.DisplayMemberPath = "ItemDescription";
            ItemsComboBox.Items.Refresh();
        }

        private void SetToReadOnly()
        {
            SaveInvoiceButton.IsEnabled = false;
            InvoiceDatePicker.IsEnabled = false;
            AddItemButton.IsEnabled = false;
            RemoveItemButton.IsEnabled = false;
        }

        private void SetToEdit()
        {
            SaveInvoiceButton.IsEnabled = true;
            AddItemButton.IsEnabled = true;
            RemoveItemButton.IsEnabled = true;
        }

        private void ReloadLineItems()
        {
            MainLogicClass.SelectedInvoice.LineItemsList = MainLogicClass.GetAllLineItemsByInvoiceNumber(MainLogicClass.SelectedInvoice.InvoiceNum);
        }
        #endregion

    }
}
