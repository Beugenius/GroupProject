using GroupProject.Items;
using GroupProject.Search;
using System;
using System.Windows;

namespace GroupProject.Main
{
    /// <summary>
    /// Interaction logic for wndMain.xaml
    /// </summary>
    public partial class wndMain : Window
    {
        public wndMain()
        {
            InitializeComponent();
        }

        private void SearchMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wndSearch SearchWindow = new();
                SearchWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something broke!"); 
            }
            // pass clsMainLogic type to selected window upon creation wndSearch = new(clsMainLogicObject); 
            // clsMainLogic int SelectedInvoice will store the invoice id when in the search window
            // any change in search window will reflect in the clsMainLogic 
            // reload main window with selected invoice id regardless if one was actually selected or not 
        }

        private void EditMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wndItems ItemsWindow = new();
                ItemsWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something broke!");
            }
            // clsMainLogic bool HasItemsChanged will be accessible
            // if any items change, boolean variable = true, else = false
            // if true, back in main window, reload combo boxes containing new items 
            // to refresh combo boxes, requery listitems, then redisplay in combo boxes 
        }
    }
}
