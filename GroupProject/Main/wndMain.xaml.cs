using GroupProject.Items;
using GroupProject.Search;
using GroupProject.ViewModels;
using System;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace GroupProject.Main
{
    /// <summary>
    /// Interaction logic for wndMain.xaml
    /// </summary>
    public partial class wndMain : Window
    {
        /// <summary>
        /// Reference to the class that contains all of the main window business logic 
        /// </summary>
        public clsMainLogic MainLogicClass { get; set; }
        /// <summary>
        /// Constructor for main window 
        /// Initializes component, sets the instance of the main logic business class to a new instance,
        /// Loads the list of items from the database, and then fills up the items combo box 
        /// </summary>
        public wndMain()
        {
            InitializeComponent();
            MainLogicClass = new();
            MainLogicClass.LoadItemsList();
            ReloadItemsComboBox();
        }
        #region Menu
        /// <summary>
        /// Method for accessing the search window. If user closes window without selecting an invoice, nothing happens 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wndSearch SearchWindow = new();
                SearchWindow.ShowDialog();
                // get the invoice from the search window as temporary 
                var InvoiceFromSearchWindow = SearchWindow.SelectedInvoice;
                // default / no selected invoice defaults to invoice num == -1 
                if (InvoiceFromSearchWindow != null && InvoiceFromSearchWindow.InvoiceNum != -1)
                {
                    // set the SelectedInvoice to the temporary invoice in here 
                    // since confirmed that an invoice was selected
                    MainLogicClass.SelectedInvoice = InvoiceFromSearchWindow;
                    // set labels and date picker
                    InvoiceDatePicker.SelectedDate = MainLogicClass.SelectedInvoice.InvoiceDate;
                    InvoiceNumberDisplayLabel.Content = MainLogicClass.SelectedInvoice.InvoiceNum;
                    TotalCostDisplayLabel.Content = $"${MainLogicClass.SelectedInvoice.TotalCost}";
                    // Load line items list
                    ReloadLineItems();
                    // Reload the invoice data grid 
                    ReloadInvoiceDataGrid();
                    // Allow editing button to be enabled
                    EditInvoiceButton.IsEnabled = true;
                    // Disabled all other controls 
                    SetToReadOnly();
                }
                // otherwise, do nothing (intentional) 
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }
        /// <summary>
        /// Method for accessing the edit window. If anything is edited, this method re-loads all of the list items 
        /// in an invoice and reloads the items list combo box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region Buttons / Controls
        /// <summary>
        /// Method for creating a new (unsaved) invoice. Sets SelectedInvoice to an instance of a new Invoices object. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateNewInvoiceButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Set SelectedInvoice to instance of new Invoices from Main logic class 
                MainLogicClass.SelectedInvoice = MainLogicClass.NewInvoiceUnsaved();
                // set labels 
                InvoiceNumberDisplayLabel.Content = "TBD";
                TotalCostDisplayLabel.Content = "$0";
                // Allow editing option 
                EditInvoiceButton.IsEnabled = true;
                // remove the date in the datepicker 
                InvoiceDatePicker.SelectedDate = null;
                // reload the invoice data grid based off of the new invoice
                ReloadInvoiceDataGrid();
                // set to read only 
                SetToReadOnly();
                // only allow user to select a date when creating new invoice 
                InvoiceDatePicker.IsEnabled = true;
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }
        /// <summary>
        /// Method for adding an item into an invoice. Calls MainLogicClass for adding an item (unsaved) 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }
        /// <summary>
        /// Method for removing an item from an invoice. Calls MainLogicClass for removing an item (unsaved)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveItemButton_Click(object sender, RoutedEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }
        /// <summary>
        /// Enables controls for editing 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditInvoiceButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Enable controls in SetToEdit method
                SetToEdit();
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }
        /// <summary>
        /// Method for when save button is clicked. Determines whether or not the invoice is new or old, then makes appropriate calls
        /// dependent on new or old. After method calls to main logic class are made, reloads invoice, invoice line items, and invoice datagrid 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveInvoiceButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // only time date will be null is during new invoice or on startup 
                var selectedDate = InvoiceDatePicker.SelectedDate;
                if (selectedDate is not null)
                {
                    if (MainLogicClass.SelectedInvoice != null && MainLogicClass.SelectedInvoice.InvoiceNum != -1)
                    {
                        // if existing, just save to the database as current state
                        MainLogicClass.SaveInvoice();
                        ReloadLineItems();
                        // display total cost 
                        TotalCostDisplayLabel.Content = $"${MainLogicClass.SelectedInvoice.TotalCost}";
                        ReloadInvoiceDataGrid();
                    }
                    // else if invoice num == -1, it is a new invoice 
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
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }

        }
        /// <summary>
        /// On change method for when an item is selected from the items combo box. Displays the cost of the item 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemsComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                if (ItemsComboBox.SelectedItem != null)  //added if statement
                {
                    ItemDesc ItemSelected = (ItemDesc)ItemsComboBox.SelectedItem;
                    IndividualItemCostDisplayLabel.Content = $"${ItemSelected.Cost}";
                }
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region Helpers
        /// <summary>
        /// Reloads the invoice data grid 
        /// </summary>
        private void ReloadInvoiceDataGrid()
        {
            try
            {
                InvoiceDataGrid.ItemsSource = MainLogicClass.SelectedInvoice.LineItemsList;
                InvoiceDataGrid.Items.Refresh();
            }
            catch (Exception ex)
            {
                // Throw the exception with a message 
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// Reloads the main logic class items list and re-binds the items list to the items combo box (reload) 
        /// </summary>
        private void ReloadItemsComboBox()
        {
            try
            {
                MainLogicClass.ItemsList = MainLogicClass.GetAllItems();
                ItemsComboBox.ItemsSource = MainLogicClass.ItemsList;
                ItemsComboBox.DisplayMemberPath = "ItemDescription";
                ItemsComboBox.Items.Refresh();
            }
            catch (Exception ex)
            {
                // Throw the exception with a message 
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// Sets the application to read only mode by disabling editing controls 
        /// </summary>
        private void SetToReadOnly()
        {
            try
            {
                SaveInvoiceButton.IsEnabled = false;
                InvoiceDatePicker.IsEnabled = false;
                AddItemButton.IsEnabled = false;
                RemoveItemButton.IsEnabled = false;
            }
            catch (Exception ex)
            {
                // Throw the exception with a message 
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// Sets the application to edit mode by enabling editing controls
        /// </summary>
        private void SetToEdit()
        {
            try
            {
                SaveInvoiceButton.IsEnabled = true;
                AddItemButton.IsEnabled = true;
                RemoveItemButton.IsEnabled = true;
            }
            catch (Exception ex)
            {
                // Throw the exception with a message 
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// Reloads the selected invoices line items. Used for when items are removed, added, or changed
        /// </summary>
        private void ReloadLineItems()
        {
            try
            {
                MainLogicClass.SelectedInvoice.LineItemsList = MainLogicClass.GetAllLineItemsByInvoiceNumber(MainLogicClass.SelectedInvoice.InvoiceNum);
            }
            catch (Exception ex)
            {
                // Throw the exception with a message 
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// This method handles exceptions thrown by displaying a message box to the user with
        /// a description of the exception and it's "stack trace"
        /// </summary>
        /// <param name="sClass"></param>
        /// <param name="sMethod"></param>
        /// <param name="sMessage"></param>
        private void HandleError(string sClass, string sMethod, string sMessage)
        {
            try
            {
                MessageBox.Show(sClass + "." + sMethod + " -> " + sMessage);
            }
            catch (System.Exception ex)
            {
                System.IO.File.AppendAllText(@"C:\Error.txt", Environment.NewLine + "HandleError Exception: " + ex.Message);
            }
        }
        #endregion

    }
}
