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
using WpfApp.User_Controls;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Keyboard simularator
        /// </summary>
        private InputSimulator sim;

        /// <summary>
        /// Predefined states to choose from
        /// </summary>
        public ObservableCollection<string> States { get; set; }

        /// <summary>
        /// Initialy chosen state
        /// </summary>
        public string SelectedState { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            sim = new InputSimulator();
            States = new ObservableCollection<string> { "Slovenská republika", "Česká republika", "Maďarsko", "Rakúsko", "Poľsko", "USA" };
            SelectedState = States[0];
            year.Text = "2021";
            dic.textBox.TextChanged += dic_TextChanged;

            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
        }

        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        private void fillFirstRowButton_Click(object sender, RoutedEventArgs e)
        {
            FillFirsRow();
        }

        private void insertNewCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            InsertNewCustomer();

            // Increment customer number
            if (customerNumber.Text.Length > 0) customerNumber.textBox.Text = (Int32.Parse(customerNumber.textBox.Text) + 1).ToString();
        }

        private void fillFormButton_Click(object sender, RoutedEventArgs e)
        {
            FillForm();

            // Increment invoice number
            if (invoiceNumber.Text.Length > 0) invoiceNumber.textBox.Text = (Int32.Parse(invoiceNumber.textBox.Text) + 1).ToString();

            // Clear form
            customerName.textBox.Text = "";

            ico.textBox.Text = "";
            dic.textBox.Text = "";

            amount.textBox.Text = "";
        }

        // Fill the first row of invoice (invoice ID), open customer database window and look up customer
        private void FillFirsRow()
        {
            const int delay = 100;

            // Switch window to invoice form (window must be open)
            IntPtr invoiceWindow = FindWindow(null, "Formulár knihy vyšlých faktúr");
            SetForegroundWindow(invoiceWindow);

            // Fill invoice indexes
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
            sim.Keyboard.Sleep(delay).TextEntry(year.Text.Substring(2, 2) + invoiceNumber.Text);
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
            sim.Keyboard.Sleep(delay).TextEntry(year.Text + invoiceNumber.Text); // Číslo faktúry
            sim.Keyboard.Sleep(delay + 100).KeyPress(VirtualKeyCode.RETURN);

            // Open customer database window and search window
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
            sim.Keyboard.Sleep(delay + 1500).KeyPress(VirtualKeyCode.RIGHT); // Wait for window to open
            sim.Keyboard.Sleep(delay).ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.F7);

            // Search for customer name in database of current customers
            sim.Keyboard.Sleep(delay).TextEntry(customerName.Text); // Názov odberateľa
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
        }

        /// <summary>
        /// Insert new customer to the database. Must have customer database window open and it must be the last window in focus.
        /// </summary>
        private void InsertNewCustomer()
        {
            const int delay = 100;

            // Switch back to the customer database window
            sim.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LMENU, VirtualKeyCode.TAB);

            // Open window for adding new customers to the database
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.TAB); // Needed only if the customer database window was closed during addition of this invoice
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.INSERT);

            // Format customer ID to have 4 digit (fill with zeros at the beggining until it is 4 digit long)
            if (customerNumber.Text.Length < 4)
            {
                for (int i = 0; i < 4 - customerNumber.Text.Length; i++)
                {
                    customerNumber.Text = customerNumber.Text.Insert(0, "0");
                }
            }

            sim.Keyboard.Sleep(delay + 500); // Wait for window to open

            // Fill customer ID
            //sim.Keyboard.Sleep(delay + 500).TextEntry(customerNumber.Text); // Číslo odberateľa // DOESN'T WORK, have to use the loop to input the ID
            for (int i = 0; i < 4; i++)
            {
                sim.Keyboard.Sleep(delay).TextEntry(customerNumber.Text[i]); // Číslo odberateľa
            }
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);

            // Fill customer name
            sim.Keyboard.Sleep(delay).TextEntry(customerName.Text); // Názov odberateľa
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);

            // Skip fields we don't need to fill
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);

            // Fill state
            sim.Keyboard.Sleep(delay).TextEntry(state.Text); // Štát
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);

            // Fill IČO if needed
            if (ico.Text.Length > 0) sim.Keyboard.Sleep(delay).TextEntry(ico.Text);
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN); // IČO

            // Fill DIČ if needed
            if (ico.Text.Length > 0) sim.Keyboard.Sleep(delay).TextEntry(dic.Text);
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN); // DIČ

            // Fill IČ DPH if needed
            if (dic.Text.Length > 0)
            {
                sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
                sim.Keyboard.Sleep(delay).TextEntry(icDph.Text);
            }
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN); // IČ DPH

            // Fill account constant
            sim.Keyboard.Sleep(delay).TextEntry("3110001"); // ÚČet
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);

            // Fill type of document constant
            sim.Keyboard.Sleep(delay).TextEntry("VFA"); // Druh dokladu
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);

            // Fill group of customer constant
            sim.Keyboard.Sleep(delay).TextEntry("00"); // Skupina odberateľov
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
        }

        // Fill the rest of the invoice. Must have customer database window open and it must be the last window in focus.
        private void FillForm()
        {
            const int delay = 100;

            // Switch back to the customer database window
            sim.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LMENU, VirtualKeyCode.TAB);

            // Choose customer
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.TAB); // Extra enter, one enter gets "eaten" after Alt+Tab-ing
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);

            // Skip unnecessary fields
            sim.Keyboard.Sleep(delay + 300).KeyPress(VirtualKeyCode.RETURN); // Wait for window to close
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
            //sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN); // WARNING: If Cislo odberatela window was already opened once and closed, this is then NECESSARY

            // Fill invoice amount
            sim.Keyboard.Sleep(delay).TextEntry(amount.Text); // Fakturovane eura
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
            
            // Skip unnecessary fields
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);

            // Format dates
            var dateOfPaymentText = DateTime.Parse(date1Picker.Text).ToString("dd.MM.yyyy");
            var dueDateText = DateTime.Parse(date2Picker.Text).ToString("dd.MM.yyyy");

            // Fill dates
            sim.Keyboard.Sleep(delay).TextEntry(dateOfPaymentText); // Datum dodania/platby
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);

            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);

            sim.Keyboard.Sleep(delay).TextEntry(dueDateText); // Datum splatnosti
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);

            // Skip unnecessary fields
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);

            // Fill invoice type
            sim.Keyboard.Sleep(delay).TextEntry("VYR"); // Druh faktury
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);

            // Skip unnecessary fields
            sim.Keyboard.Sleep(delay).KeyPress(VirtualKeyCode.RETURN);
        }

        // Event handler for closing application when Esc key is pressed.
        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }

        // Set date2 value to date1.
        private void dateCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            date2Picker.SelectedDate = DateTime.Parse(date1Picker.Text);
            date2Picker.IsEnabled = false;
        }

        // Set date2 value to date1 and offset it by 15 days.
        private void dateCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            date2Picker.SelectedDate = DateTime.Parse(date1Picker.Text).AddDays(15);
            date2Picker.IsEnabled = true;
        }

        // Set date2 value to date1 if check box is checked or set date2 value to date1 and offset it by 15 days.
        private void date1Picker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if(dateCheckBox.IsChecked ?? false)
            {
                date2Picker.SelectedDate = DateTime.Parse((sender as DatePicker).Text);
            }
            else
            {
                date2Picker.SelectedDate = DateTime.Parse((sender as DatePicker).Text).AddDays(15);
            }
        }

        // Update IČ DPh field if the user is typing in DIČ field and format it (prepend SK at the start).
        private void dic_TextChanged(object sender, TextChangedEventArgs e)
        {
            string icDphEdited = "SK" + (sender as TextBox).Text;
            icDph.textBox.Text = icDphEdited;
        }
    }
}
