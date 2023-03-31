using GroupProject.Search;
using System;
using System.Collections.Generic;
using System.Linq;
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
            // pass the selected invoice id as a reference during window creation
            // set referenced value in search window so when that window closes, the value is already here and ready to use 
            // reload main window with selected invoice id regardless if one was actually selected or not 
        }

        private void EditMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // pass boolean value by reference ItemsHaveChanged or something along those lines
            // If any items have changed, requery, re-load combo boxes
            // If no changes have been made, do nothing 
        }
    }
}
