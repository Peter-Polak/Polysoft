using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WindowsInput;
using WindowsInput.Native;
using WpfApp.UserControls;
using WpfApp.ViewModels;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var invoiceViewModel = new InvoiceViewModel();
            DataContext = invoiceViewModel;
        }

        ///// <summary>
        ///// Event handler for closing application when Esc key is pressed.
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void HandleEsc(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.Escape)
        //        Close();
        //}

        //// Set date2 value to date1.
        //private void dateCheckBox_Checked(object sender, RoutedEventArgs e)
        //{
        //    date2Picker.SelectedDate = DateTime.Parse(date1Picker.Text);
        //    date2Picker.IsEnabled = false;
        //}

        ///// <summary>
        ///// Set date2 value to date1 and offset it by 15 days.
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void dateCheckBox_Unchecked(object sender, RoutedEventArgs e)
        //{
        //    date2Picker.SelectedDate = DateTime.Parse(date1Picker.Text).AddDays(15);
        //    date2Picker.IsEnabled = true;
        //}

        ///// <summary>
        ///// Set date2 value to date1 if check box is checked or set date2 value to date1 and offset it by 15 days.
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void date1Picker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if(dateCheckBox.IsChecked ?? false)
        //    {
        //        date2Picker.SelectedDate = DateTime.Parse((sender as DatePicker).Text);
        //    }
        //    else
        //    {
        //        date2Picker.SelectedDate = DateTime.Parse((sender as DatePicker).Text).AddDays(15);
        //    }
        //}

        ///// <summary>
        ///// Update IČ DPh field if the user is typing in DIČ field and format it (prepends SK at the start).
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void dic_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    string icDphEdited = "SK" + (sender as TextBox).Text;
        //    icDph.Text = icDphEdited;
        //}
    }
}
