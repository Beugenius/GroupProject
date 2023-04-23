using GroupProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
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

namespace GroupProject.Items
{
    
    /// <summary>
    /// Interaction logic for wndItems.xaml
    /// </summary>
    public partial class wndItems : Window
    {
        /// <summary>
        /// Allows the Main page to know that items have been altered in any way so they can update the display
        /// </summary>
        public bool ItemsChanged = false;
        /// <summary>
        /// Allows access to the items logic class
        /// </summary>
        public clsItemsLogic clsItemsLogic = new clsItemsLogic();
        /// <summary>
        /// Allows access to the itemsDesc class
        /// </summary>
        public ItemDesc ItemDesc;
        /// <summary>
        /// Enum that determines which button was pushed last
        /// </summary>
        public enum itemEnum { Add, Edit, Delete, None}
        /// <summary>
        /// Enum that determines which button action needs to be completed
        /// </summary>
        itemEnum item = new itemEnum();
        /// <summary>
        /// initializes the items window
        /// </summary>
        public wndItems()
        {
            InitializeComponent();
            ItemDataGrid.ItemsSource = clsItemsLogic.GetItems();
            item = itemEnum.None;
        }
        /// <summary>
        /// enables add item mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                item = itemEnum.Add;
                txtbxCode.IsEnabled = true;
                txtbxCost.IsEnabled = true;
                txtbxDestription.IsEnabled = true;
                txtbxCode.Text = "";
                txtbxCost.Text = "";
                txtbxDestription.Text = "";

            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
            
        }
        /// <summary>
        /// enables edit item mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                item = itemEnum.Edit;
                txtbxCode.IsEnabled = false;
                txtbxCost.IsEnabled = true;
                txtbxDestription.IsEnabled = true;


            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
            
        }
        /// <summary>
        /// Enables delete item mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                item = itemEnum.Delete;
                txtbxCode.IsEnabled = false;
                txtbxCost.IsEnabled = false;
                txtbxDestription.IsEnabled = false;

            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
            
        }
        /// <summary>
        /// Saves the items into the database that have been added, deleted, or edited. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (item == itemEnum.Add)
                {
                    ItemDesc newItem = new ItemDesc();
                    newItem.ItemCode = txtbxCode.Text;
                    decimal.TryParse(txtbxCost.Text, out decimal cost);   //Need to only allow numbers and decmial
                    newItem.Cost = cost;
                    newItem.ItemDescription = txtbxDestription.Text;
                    clsItemsLogic.AddItem(newItem);
                    ItemsChanged = true;
                    ReloadDataGrid();
                }
                else if (item == itemEnum.Edit && ItemDataGrid.SelectedItem != null)
                {
                    ItemDesc editItem = new ItemDesc();
                    editItem.ItemCode = txtbxCode.Text;
                    decimal.TryParse(txtbxCost.Text, out decimal cost);   //Need to only allow numbers and decmial
                    editItem.Cost = cost;
                    editItem.ItemDescription = txtbxDestription.Text;
                    clsItemsLogic.EditItem(editItem);
                    ItemsChanged = true;
                    ReloadDataGrid();
                }
                else if (item == itemEnum.Delete)
                {
                    ItemDesc deleteItem = new ItemDesc();
                    deleteItem.ItemCode = txtbxCode.Text;
                    decimal.TryParse(txtbxCost.Text, out decimal cost);   //Need to only allow numbers and decmial
                    deleteItem.Cost = cost;
                    deleteItem.ItemDescription = txtbxDestription.Text;
                    txtbxCode.Text = "";
                    txtbxCost.Text = "";
                    txtbxDestription.Text = "";
                    clsItemsLogic.DeleteItem(deleteItem);
                    ItemsChanged = true;
                    ReloadDataGrid();
                }
                item = itemEnum.None;
                txtbxCode.IsEnabled= false;
                txtbxCost.IsEnabled= false;
                txtbxDestription.IsEnabled= false;
                ItemDataGrid.IsReadOnly = true;
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }
        /// <summary>
        /// updates the text boxes data to match the current selected item 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if ((item == itemEnum.Edit || item == itemEnum.Delete) && ItemDataGrid.SelectedItem != null)
                {
                    if (item == itemEnum.Edit)
                    {
                        txtbxCost.IsEnabled = true;
                        txtbxDestription.IsEnabled = true;

                    }
                    ItemDesc seletedItem = (ItemDesc)ItemDataGrid.SelectedItem;
                    txtbxCode.Text = seletedItem.ItemCode;
                    txtbxCost.Text = seletedItem.Cost.ToString();
                    txtbxDestription.Text = seletedItem.ItemDescription;
                }

            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }
        /// <summary>
        /// reloads the data into the ItemDataGrid
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void ReloadDataGrid()
        {
            try
            {
                ItemDataGrid.ItemsSource = clsItemsLogic.GetItems();
                ItemDataGrid.Items.Refresh();
            }
            catch (Exception ex)
            {
                // Throw the exception with a message 
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// Handles all errors that appear
        /// </summary>
        /// <param name="sClass"></param>
        /// <param name="sMethod"></param>
        /// <param name="sMessage"></param>
        public void HandleError(string sClass, string sMethod, string sMessage)
        {
            try
            {
                MessageBox.Show(sClass + "." + sMethod + " -> " + sMessage);
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("C:\\Error.txt", Environment.NewLine + "Handle Error Exception: " + ex.Message);
            }
        }
        /// <summary>
        /// Prevents textbox from having anything but numbers and decimal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtbxCost_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox text = sender as TextBox;
            string cost = txtbxCost.Text;
            bool afterDecimal = false;
            int afterTheDecimal = 2;
            if (e.Key == Key.Decimal || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || (e.Key <= Key.D0 && e.Key >= Key.D9))
            {
                
            }
            else
            {
                
                foreach (char letter in cost)
                {
                    if (letter >= 48 && letter <= 57)
                    {

                    }
                    else
                    {
                        cost = cost.Replace(letter.ToString(), "");
                    }
                }
                
            }
            txtbxCost.Text = cost;
        }


    }
}
