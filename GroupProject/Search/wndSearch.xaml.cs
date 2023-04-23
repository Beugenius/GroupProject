using GroupProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GroupProject.Search
{
    /// <summary>
    /// Interaction logic for wndSearch.xaml
    /// </summary>

    public partial class wndSearch : Window
    {
        public Invoices SelectedInvoice = new(); // if no invoice is selected, make sure a new() Invoices is returned :) 
        /// <summary>
        /// References back to the SearchLogicClass 
        /// </summary>
        clsSearchLogic SearchLogicClass { get; set; } = new();
        /// <summary>
        /// Initializes the search window and loads the InvoiceDataGrid with the Invoice List from the main menu.
        /// </summary>
        public wndSearch()
        {
            InitializeComponent();
            SearchLogicClass.LoadInvoiceList();
            ReloadInvoiceDataGrid();
        }
        /// <summary>
        /// Promopts the SearchWindowButton to popup 
        /// </summary>
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
                    //SearchLogicClass.SelectedInvoice = InvoiceFromSearchWindow;
                    // set labels and date picker
                    //InvoiceDatePicker.SelectedDate = SearchLogicClass.SelectedInvoice.InvoiceDate;
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
        /// This method is called when the SearchWindow clear button is pressed and returns the SearchWindow InvoiceDataGrid to blank.
        /// </summary>
        private void SearchWindowclearButton_Click(object sender, RoutedEventArgs e)
        {
            //this.Close();
            SearchWindowDataGrid.ItemsSource = null;
            ReloadInvoiceDataGrid();
            

        }
        /// <summary>
        /// Selected Invoice is stored into SelectedInvoice once an invoice is clicked.
        /// </summary>
        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedInvoice = (Invoices)SearchWindowDataGrid.SelectedValue;
        }
        /// <summary>
        /// This method reloads the Price Combo Box whenever a change is made.
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void ReloadPriceComboBox(DateTime? d)
        {

            try
            {
                DateTime date = d ?? DateTime.Now;
                //SearchLogicClass.InvoiceNumbers = SearchLogicClass.GetInvoiceNumbers(date);
                SearchLogicClass.LoadInvoiceDateList(date);

                if (SearchLogicClass.InvoicePrices == null)
                {
                    SearchLogicClass.InvoicePrices = new List<Invoices>();
                }

                if (invoicePriceComboBox != null)
                {
                    invoicePriceComboBox.ItemsSource = SearchLogicClass.InvoicePrices_s;
                    invoicePriceComboBox.Items.Refresh();

                }
                try
                {
                    if (SearchWindowDataGrid != null)
                    {
                        SearchWindowDataGrid.ItemsSource = SearchLogicClass.InvoiceNumbers;
                        SearchWindowDataGrid.Items.Refresh();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
                }

                // ReloadPriceComboBox();

            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// This method reloads the Invoice Combo Box when a change is made.
        /// </summary>
        private void ReloadInvoiceComboBox(DateTime? d)
        {
            try
            {
                DateTime date = d ?? DateTime.Now;
                //SearchLogicClass.InvoiceNumbers = SearchLogicClass.GetInvoiceNumbers(date);
                SearchLogicClass.LoadInvoiceDateList(date);

                if (SearchLogicClass.InvoiceNumbers == null)
                {
                    SearchLogicClass.InvoiceNumbers = new List<Invoices>();
                }

                if (invoiceNumberComboBox != null)
                {
                    invoiceNumberComboBox.ItemsSource = SearchLogicClass.InvoiceNumbers_s;
                    //invoiceNumberComboBox.DisplayMemberPath = "Invoice Numbers";
                    invoiceNumberComboBox.Items.Refresh();
                }
                try
                {
                    if (SearchWindowDataGrid != null)
                    {
                        SearchWindowDataGrid.ItemsSource = SearchLogicClass.InvoiceNumbers;
                        SearchWindowDataGrid.Items.Refresh();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
                }

                // ReloadPriceComboBox();

            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// This method is to refresh the InvoiceData grid whenever a selection is made.
        /// </summary>
        private void ReloadInvoiceDataGrid()
        {
            try
            {
                //DateTime defaultDate = new DateTime(2023, 4, 22, 0, 0, 0);
                SearchWindowDataGrid.ItemsSource = SearchLogicClass.GetInvoices();
                SearchLogicClass.LoadInvoiceList();
                invoiceNumberComboBox.ItemsSource = SearchLogicClass.InvoiceNumbers_s;
                invoiceNumberComboBox.Items.Refresh();
                invoicePriceComboBox.ItemsSource = SearchLogicClass.InvoicePrices_s;
                invoicePriceComboBox.Items.Refresh();
            }
            catch (Exception ex)
            {
                // Throw the exception with a message 
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// Closes SearchWindow and calls ReloadInvoiceDataGrid 
        /// </summary>
        private void ViewSelectedButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
            //ReloadInvoiceDataGrid();
        }
        /// <summary>
        /// Stores sekected date when it is picked from the DatePicker
        /// </summary>
        private void DateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime? selectedDate = InvoiceDatePicker.SelectedDate;
            if (selectedDate.HasValue)
            {
                ReloadInvoiceComboBox(selectedDate);
                ReloadPriceComboBox(selectedDate);
            }
        }
        /// <summary>
        /// Method used to handle errors and specify where error is occuring
        /// </summary>
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
        /// <summary>
        /// This method is run when the Invoice combo box selection is changed. When a Invoice number is selected that invoice is shown in the datagrid.
        /// </summary>
        private void InvoiceComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                // Get the selected invoice number from the InvoiceComboBox
                string selectedInvoiceNumber = (string)invoiceNumberComboBox.SelectedItem;

                // Get the list of invoices corresponding to the selected invoice number
                List<Invoices> a = SearchLogicClass.GetInvoices(int.Parse(selectedInvoiceNumber));

                // Set the list of invoices as the ItemsSource of the SearchWindowDataGrid
                SearchWindowDataGrid.ItemsSource = a;

                // Refresh the SearchWindowDataGrid to display the updated list of invoices
                SearchWindowDataGrid.Items.Refresh();
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }

            //ReloadPriceComboBox();
        }
        /// <summary>
        /// This method is called when the Invoice Price is changed. This results in a list of invoices with the selected Price.
        /// </summary>
        private void InvoicePriceChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                // Get the selected invoice number from the InvoiceComboBox
                string selectedTotalCost = (string)invoicePriceComboBox.SelectedItem;

                // Get the list of invoices corresponding to the selected invoice number
                DateTime? selectedDate = InvoiceDatePicker.SelectedDate;

                if (selectedDate.HasValue)
                {
                    SearchLogicClass.LoadInvoiceDateCostList(int.Parse(selectedTotalCost), (DateTime)selectedDate);

                    // Set the list of invoices as the ItemsSource of the SearchWindowDataGrid
                    SearchWindowDataGrid.ItemsSource = SearchLogicClass.InvoicePrices;

                    // Refresh the SearchWindowDataGrid to display the updated list of invoices
                    SearchWindowDataGrid.Items.Refresh();
                }

            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
    }
}
